using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel
{
    public int Level = 1;
    public int Coins = 100;

    public List<PetModel> Pets = new();
    public List<InventoryItemModel> Inventory = new();

    public AvatarCustomization Customization = new();
    public TankModel Tank = new();
    public YardModel Yard = new();
}