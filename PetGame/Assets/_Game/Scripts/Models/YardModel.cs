using System;
using System.Collections.Generic;

[Serializable]
public class YardModel
{
    public int YardLevel = 1;
    public List<YardTile> Tiles = new();
    public List<PlacedToyModel> PlacedToys = new();
}

[Serializable]
public class YardTile
{
    public int X { get; set; }
    public int Y { get; set; }
    public string TerrainType { get; set; } // e.g. grass, water, sand
    public bool IsUnlocked { get; set; }
}
