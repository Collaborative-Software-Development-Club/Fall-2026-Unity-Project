using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public Vector2 MoveDir = Vector2.right;
    public int MoveSpeed = 1;
    
    public bool SpawnTestItems = false;
    public int TestAmount;
    public ItemData TestItem;
    public ItemHolder itemHolder;

    private void Start()
    {
        if (!SpawnTestItems) return;
        
        for (int i = 0; i < TestAmount; i++)
        {
            ItemFactory.CreateItemFromSO(TestItem);
            var newObj =  Instantiate(itemHolder);
            newObj.name = name + " Test Item " + i;
            itemHolder.transform.position = transform.position;
        }
    }

    private List<ItemHolder>  _items = new List<ItemHolder>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.name);
        
        if (!other.TryGetComponent(out ItemHolder itemHolder)) return;
        
        other.transform.parent = itemHolder.transform;
        _items.Add(itemHolder);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ItemHolder itemHolder)) return;
        
        other.transform.parent = null;
        _items.Remove(itemHolder);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        foreach (var item in _items)
            item.transform.localPosition += new Vector3(MoveDir.x, MoveDir.y, 0f) * MoveSpeed * Time.fixedDeltaTime;
    }

    public List<ItemHolder> GetItemHolders()
    {
        return _items;
    }
}
