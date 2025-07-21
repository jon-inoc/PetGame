using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour, IInitializable, ITickable, IDisposable
{
    private PlayerModel _playerData;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        LoadPlayerData();

        // If this is a new player (just created), seed mock data and save immediately
        if (IsNewPlayer())
        {
            SeedMockData();
            SaveProgress();
        }

        Debug.Log($"Game started. Coins: {_playerData.Coins}, Level: {_playerData.Level}, Pets: {_playerData.Pets.Count}");
    }

    private bool IsNewPlayer()
    {
        // Assuming a new player has level == 1 and coins == 0 means uninitialized
        // or any other logic to detect empty data
        return _playerData.Coins == 0 && _playerData.Level == 1 && _playerData.Pets.Count == 0;
    }

    private void SeedMockData()
    {
        _playerData.Coins = 100;
        _playerData.Level = 1;

        // Add a sample pet
        _playerData.Pets.Add(new PetModel
        {
            PetId = "cat001",
            Name = "Whiskers",
            Breed = "Siamese",
            PetLevel = 1,
            Hunger = 80f,
            Happiness = 90f,
            BondLevel = 10f
        });

        // Setup Tank
        _playerData.Tank = new TankModel
        {
            ArmorLevel = 1,
            WeaponLevel = 1,
            EquippedModules = new System.Collections.Generic.List<string> { "Turret", "Drone" },
            EquippedSkills = new System.Collections.Generic.List<string> { "Shield" }
        };

        // Setup Yard with 2 tiles and one placed toy
        _playerData.Yard = new YardModel
        {
            YardLevel = 1,
            Tiles = new System.Collections.Generic.List<YardTile>
            {
                new YardTile { X = 0, Y = 0, TerrainType = "Grass", IsUnlocked = true },
                new YardTile { X = 1, Y = 0, TerrainType = "Grass", IsUnlocked = false }
            },
            PlacedToys = new System.Collections.Generic.List<PlacedToyModel>
            {
                new PlacedToyModel { ToyId = "toy001", X = 0, Y = 0, IsActive = true }
            }
        };

        // Setup Inventory with some toys and treats
        _playerData.Inventory = new System.Collections.Generic.List<InventoryItemModel>
        {
            new InventoryItemModel { ItemId = "toy002", ItemType = "Toy", Quantity = 2 },
            new InventoryItemModel { ItemId = "treat001", ItemType = "Treat", Quantity = 5 }
        };

        // Setup Avatar Customization
        _playerData.Customization = new AvatarCustomization
        {
            OutfitId = "outfit001",
            HatId = "hat001",
            AccessoryId = "collar001"
        };
    }

    public void Tick()
    {
        // Per-frame updates if needed
    }

    public void AddCoins(int amount)
    {
        _playerData.Coins += amount;
        Debug.Log($"Coins increased: {_playerData.Coins}");
    }

    public void IncreaseLevel(int amount = 1)
    {
        _playerData.Level += amount;
        Debug.Log($"Player level increased to: {_playerData.Level}");
    }

    public void AddPet(PetModel newPet)
    {
        _playerData.Pets.Add(newPet);
        Debug.Log($"Added new pet: {newPet.Name}");
    }

    public void SaveProgress()
    {
        if (_playerData == null)
        {
            Debug.LogWarning("Attempted to save null PlayerModel. Creating new PlayerModel.");
            _playerData = new PlayerModel();
        }

        string json = JsonUtility.ToJson(_playerData, true);
        PlayerPrefs.SetString("player", json);
        PlayerPrefs.Save();
        Debug.Log("Progress saved.");
    }

    public void Dispose()
    {
        SaveProgress();
        Debug.Log("GameManager disposed.");
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("player"))
        {
            string json = PlayerPrefs.GetString("player");
            _playerData = JsonUtility.FromJson<PlayerModel>(json);

            if (_playerData == null)
            {
                Debug.LogWarning("PlayerModel JSON was invalid. Creating new PlayerModel.");
                _playerData = new PlayerModel();
            }
        }
        else
        {
            _playerData = new PlayerModel();
        }
    }

    public PlayerModel GetPlayerData() => _playerData;

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveProgress();
        }
    }
}
