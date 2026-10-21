using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a handmade script to generate a certain conveyor this shouldn't be used more than once
/// </summary>
public class ConveyorGenerator : MonoBehaviour
{
    public GameObject conveyorPrefab;
    public GameObject itemPrefab;
    private Vector2Int size = new Vector2Int(10, 10);
    
    private List<ConveyorSection> _chain = new();
    private List<ConveyorRenderer> _conveyorRenderers = new();
    public List<ItemDataQuantity> itemsToGenerate = new();

    private void Start()
    {
        GenerateConveyors();
        GenerateItems();
    }

    private void GenerateConveyors()
    {
        ConveyorSection section = new ConveyorSection();
        
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if(i != 0 && i != size.x - 1 && j != 0 && j != size.y - 1) continue;

                if (section.GetSectionFull())
                {
                    _chain.Add(section);
                    section = new ConveyorSection();
                }

               
                Conveyor conveyor = new Conveyor();
               
                ConveyorRenderer conveyorRenderer = Instantiate(conveyorPrefab).GetComponent<ConveyorRenderer>();
                conveyorRenderer.transform.position = new Vector3(transform.position.x + i, transform.position.y + j, 0);
               
                conveyorRenderer.SetConveyor(conveyor);
                _conveyorRenderers.Add(conveyorRenderer);

                Vector2 moveDir = Vector2.zero;

                if (j == 0)
                    moveDir = Vector2.right;
                else if (i == size.x - 1)
                    moveDir = Vector2.up;
                else if (j == size.y - 1)
                    moveDir = Vector2.left;
                else if (i == 0)
                    moveDir = Vector2.down;

                conveyorRenderer.SetDirection(moveDir);
                   
                section.AddConveyorable(conveyor);
            }
        }
        
        _chain.Add(section);
        GameManager.Instance.conveyorManager.AddNewChain(_chain);
    }

    private void GenerateItems()
    {
        List<ConveyorRenderer> availableConveyors =
            _conveyorRenderers.Where(cr => cr.GetConveyor().HasSpace()).ToList();

        if (availableConveyors.Count == 0) return;

        foreach (var itemQuantity in itemsToGenerate)
        {
            for (int i = 0; i < itemQuantity.Quantity; i++)
            {
                if (availableConveyors.Count == 0) break;

                int randomIndex = Random.Range(0, availableConveyors.Count);
                ConveyorRenderer targetRenderer = availableConveyors[randomIndex];
                Conveyor conveyor = targetRenderer.GetConveyor();

                GameObject itemObj = Instantiate(itemPrefab);
                ConveyorItemRenderer itemRenderer = itemObj.GetComponent<ConveyorItemRenderer>();

                if (itemQuantity.ItemData is BoxData boxData && boxData.openItem is MachineData machine)
                {
                    UnboxableConveyorItem unboxableConveyorItem = itemObj.AddComponent<UnboxableConveyorItem>();
                    unboxableConveyorItem.OnRightClickEvent += () => MachinePlace.Place(conveyor, machine);
                    
                    itemRenderer = unboxableConveyorItem;
                }
                
                itemRenderer.SetSprite(itemQuantity.ItemData.icon);

                int slotIndex = conveyor.GetSize();
                float slotSpacing = 0.25f;
                Vector3 basePos = targetRenderer.transform.position;
                Vector3 slotOffset = new Vector3(targetRenderer.GetDirection().x, targetRenderer.GetDirection().y, 0) * (slotIndex - 1) * slotSpacing;                
                
                itemObj.transform.position = basePos + slotOffset;

                conveyor.AddItemQuantityToPart(new ItemQuantity(ItemFactory.CreateItemFromSO(itemQuantity.ItemData), 1));

                if (conveyor.HasSpace()) continue;

                availableConveyors.RemoveAt(randomIndex);
            }
        }
    }

    private int SumConveyorPool(List<ConveyorSection> sections) {
        int conveyorPool = 0;

        foreach (var section in sections)
        {
            foreach (var conveyor in section.GetConveyors())
            {
                conveyorPool += conveyor.GetSize();
            }
        }

        return conveyorPool;
    }
}
