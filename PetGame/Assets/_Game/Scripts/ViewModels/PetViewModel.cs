using UnityEngine;
using System;

public class PetViewModel
{
    private readonly PetModel _model;

    public event Action<float> OnHungerChanged;
    public event Action<float> OnHappinessChanged;
    public event Action<int> OnLevelChanged;

    public PetViewModel(PetModel model)
    {
        _model = model;
    }

    public int PetLevel
    {
        get => _model.PetLevel;
        set
        {
            if (_model.PetLevel != value)
            {
                _model.PetLevel = value;
                OnLevelChanged?.Invoke(value);
            }
        }
    }

    public float Hunger
    {
        get => _model.Hunger;
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            if (Mathf.Abs(_model.Hunger - value) > 0.01f)
            {
                _model.Hunger = value;
                OnHungerChanged?.Invoke(value);
            }
        }
    }

    public float Happiness
    {
        get => _model.Happiness;
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            if (Mathf.Abs(_model.Happiness - value) > 0.01f)
            {
                _model.Happiness = value;
                OnHappinessChanged?.Invoke(value);
            }
        }
    }

    //public Animator Animator => _model.Animator;
    //public Renderer VisualRenderer => _model.VisualRenderer;

    public void ChangeHunger(float amount) => Hunger += amount;
    public void ChangeHappiness(float amount) => Happiness += amount;
    public void LevelUp() => PetLevel++;
}
