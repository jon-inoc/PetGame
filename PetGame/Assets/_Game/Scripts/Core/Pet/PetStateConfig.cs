using UnityEngine;

[CreateAssetMenu(fileName = "PetStateConfig", menuName = "PetSimulation/Pet State Config")]
public class PetStateConfig : ScriptableObject
{
    [Header("Idle State")]
    public float idleWaitTime = 2f;
    [Range(0f, 1f)] public float howlChance = 0.3f;

    [Header("Roam State")]
    public float roamSpeed = 6f;
    public float roamPauseTime = 1.5f;

    [Header("Eat State")]
    public float eatDuration = 3f;
    public float hungerRestoreAmount = 30f;

    [Header("Hungry State")]
    public float hungryThreshold = 50f;
    public float hungrySearchSpeed = 5f;

    [Header("Interact State")]
    public float interactCooldownSecs = 10f;
    public float interactDuration = 2f;

    [Header("Play State")]
    public float playDuration = 4f;
    public float playCooldown = 10f;

    [Header("Poop State")]
    public float poopCooldown = 20f;
    public int coinsReward = 5;
}
