using System;

[Serializable]
public class InventoryItemModel
{
    public string ItemId;
    public string ItemType; // "Toy", "Treat", "Material"
    public int Quantity;
}
