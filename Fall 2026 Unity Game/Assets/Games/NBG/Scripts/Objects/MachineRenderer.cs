using UnityEngine;

public class MachineRenderer : PlaceableObj
{
    private const float TickSeconds = 0.5f;

    private Machine _machine;
    private GameObject _itemPrefab;
    private float _elapsed;

    public Machine GetMachine => _machine;

    public void SetMachine(Machine machine)
    {
        _machine = machine;
    }

    public void Initialize(Machine machine)
    {
        Initialize(machine, GetDirection(), ConveyorGenerator.ItemPrefab);
    }

    public void Initialize(Machine machine, Vector2 beltDirection, GameObject itemPrefab)
    {
        _machine = machine;
        _itemPrefab = itemPrefab;
        SetDirection(beltDirection.sqrMagnitude > 0.01f ? beltDirection.normalized : Vector2.right);
        SetSprite(machine.GetIcon());

        if (_itemPrefab == null)
            Debug.LogError("Machine has no item prefab, so it cannot spawn output sprites.", this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_machine == null) return;
        if (!other.TryGetComponent(out ConveyorItemRenderer itemRenderer)) return;
        if (itemRenderer.WasReleasedByMachine) return;
        if (itemRenderer is UnboxableConveyorItem) return;
        if (itemRenderer.Item == null || itemRenderer.Item.GetData() == null) return;

        itemRenderer.DetachFromBelt();
        other.transform.position = transform.position;
        if (other.attachedRigidbody != null)
        {
            other.attachedRigidbody.position = transform.position;
            other.attachedRigidbody.linearVelocity = Vector2.zero;
        }

        bool needsInputSlot = itemRenderer.Item is Processable;
        if (needsInputSlot && !_machine.HasSpace()) return;

        Item intake = ItemFactory.CreateItemFromSO(itemRenderer.Item.GetData());
        if (intake == null || !_machine.AddItemToInput(intake, 1)) return;

        itemRenderer.Destroy();
    }

    private void FixedUpdate()
    {
        if (_machine == null) return;

        _elapsed += Time.fixedDeltaTime;
        if (_elapsed >= TickSeconds)
        {
            _elapsed = 0f;
            _machine.PerformOperation();
        }

        ReleaseOutputs();
    }

    private void ReleaseOutputs()
    {
        if (_itemPrefab == null) return;

        ItemQuantity output = _machine.RemoveLastItemFromPart();
        while (output != null && !output.IsEmpty() && output.Item != null)
        {
            int count = Mathf.Max(1, output.Quantity);
            for (int i = 0; i < count; i++)
            {
                Item visualItem = i == 0 ? output.Item : ItemFactory.CreateItemFromSO(output.Item.GetData());
                if (visualItem == null) continue;
                SpawnOutput(visualItem, i, count);
            }

            output = _machine.RemoveLastItemFromPart();
        }
    }

    private void SpawnOutput(Item item, int index, int count)
    {
        Vector2 direction = GetDirection();
        Vector2 side = new Vector2(-direction.y, direction.x);
        float spread = (index - (count - 1) * 0.5f) * 0.2f;
        Vector3 position = transform.position + (Vector3)(direction + side * spread);

        GameObject itemObj = Instantiate(_itemPrefab, position, Quaternion.identity);
        itemObj.name = item.GetName();

        if (itemObj.TryGetComponent(out Rigidbody2D body))
        {
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
        }

        ConveyorItemRenderer itemRenderer = itemObj.GetComponent<ConveyorItemRenderer>();
        if (itemRenderer == null)
        {
            Debug.LogError("Item prefab is missing ConveyorItemRenderer.", this);
            Destroy(itemObj);
            return;
        }

        itemRenderer.SetItem(item);
        Sprite icon = item.GetIcon();
        if (icon != null)
        {
            itemRenderer.SetSprite(icon);
            itemRenderer.SetColor(Color.white);
        }
        else
        {
            itemRenderer.SetColor(ColorForItem(item));
        }

        itemRenderer.MarkReleasedByMachine();
    }

    // Added by ai interesting idk if we wanna keep but this is gonna for testing
    // TODO: Decide if this should be kept
    private static Color ColorForItem(Item item)
    {
        int hash = 0;
        string itemName = item.GetName() ?? string.Empty;
        foreach (char character in itemName)
            hash = hash * 31 + character;

        float hue = (hash & 255) / 255f;
        return Color.HSVToRGB(hue, 0.8f, 1f);
    }
}
