using System;

[Serializable]
public class PetModel
{
    public string PetId { get; set; } // Unique cat ID
    public string Name { get; set; }
    public string Breed { get; set; }

    public int PetLevel { get; set; } = 1;
    public float Hunger { get; set; } = 100f;
    public float Happiness { get; set; } = 100f;
    public float BondLevel { get; set; } = 0f;

}
