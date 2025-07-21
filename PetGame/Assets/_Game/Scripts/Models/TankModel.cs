using System;
using System.Collections.Generic;

[Serializable]
public class TankModel
{
    public int ArmorLevel { get; set; }
    public int WeaponLevel { get; set; }

    public List<string> EquippedModules { get; set; } = new(); // e.g. ["Turret", "Drone"]
    public List<string> EquippedSkills { get; set; } = new(); // e.g. ["Shield", "Teleport"]
}
