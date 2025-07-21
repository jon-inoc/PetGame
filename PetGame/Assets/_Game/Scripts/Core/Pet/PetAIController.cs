using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PetAIController : MonoBehaviour
{
    private PetViewModel _viewModel;
    private PetStateConfig _config;

    public void Construct(PetViewModel viewModel, PetStateConfig config)
    {
        _viewModel = viewModel;
        _config = config;
    }

    #region States
    private StateMachine _stateMachine;
    private StateMachine.State _idle, _roam, _eat, _hungry, _interact, _play, _howl;
    #endregion

    [Header("General Attributes")]
    [SerializeField] private string _currentState;
    [SerializeField] private int _viewDistance = 10;
    [SerializeField] private float _viewAngleInDegrees = 45f;
    [SerializeField] private PetTriggerSensor _triggerSensor;

    [Header("Movement")]
    [SerializeField] private float _alertSpeed = 5f;
    [SerializeField] private float _eatSpeed = 4f;
    [SerializeField] private float _wallAvoidanceRadius = 1.5f;
    [SerializeField] private Vector2 _groundSize = new Vector2(10f, 10f);
    private Vector3 _currentRoamTarget;
    private bool _hasRoamTarget = false;

    [Header("Timers and Trackers")]
    private float _startWaitTime;
    private float _interactCooldownEnd = 0f;
    private float _eatStartTime = -1f;
    private bool _isEating = false;
    private bool _isInPlayCooldown = false;
    private Vector3? _interactTargetPosition;
    private Vector3? _foodTargetPosition;

    [Header("Howl")]
    [SerializeField] private float _howlRadius = 35f;
    [SerializeField] private LayerMask _layerToHowl;

    private float _playStartTime = -1f;
    private PetAIController _currentPlayPartner;
    private readonly HashSet<PetAIController> _recentPartners = new();

    private void Awake()
    {
        if (_triggerSensor != null)
        {
            _triggerSensor.OnInteractableEnter += obj => D($"Interactable detected: {obj.name}");
            _triggerSensor.OnPetEnter += TryPlayWithNearbyPet;
            _triggerSensor.OnWallEnter += obj => D($"Wall detected: {obj.name}");
        }
    }

    private void Start()
    {
        CreateStates();
        _stateMachine.ready = true;
    }

    private void Update()
    {
        _currentState = _stateMachine?.CurrentState?.ToString();
        _stateMachine.Update();
    }

    private void CreateStates()
    {
        _stateMachine = new StateMachine();

        _idle = _stateMachine.CreateState("Idle");
        _idle.OnEnter = () =>
        {
            _startWaitTime = Time.time + _config.idleWaitTime;
            _hasRoamTarget = false;
        };
        _idle.OnFrame = HandleIdleState;

        _howl = _stateMachine.CreateState("Howl");
        _howl.OnEnter = () => D("Howling...");
        _howl.OnFrame = () => _stateMachine.TransitionTo(_roam);

        _roam = _stateMachine.CreateState("Roam");
        _roam.OnEnter = () => ChooseNewRoamTarget();
        _roam.OnFrame = HandleRoamState;

        _interact = _stateMachine.CreateState("Interact");
        _interact.OnEnter = () =>
        {
            _interactTargetPosition = GetClosestInteractablePosition();
            _startWaitTime = 0f;
        };
        _interact.OnFrame = HandleInteractState;

        _hungry = _stateMachine.CreateState("Hungry");
        _hungry.OnEnter = () => D("Pet is hungry!");
        _hungry.OnFrame = HandleHungryState;

        _eat = _stateMachine.CreateState("Eat");
        _eat.OnEnter = () =>
        {
            _foodTargetPosition = null;
            _isEating = false;
            _eatStartTime = -1f;
        };
        _eat.OnFrame = HandleEatState;

        _play = _stateMachine.CreateState("Play");
        _play.OnEnter = () =>
        {
            D("Started playing!");
            _playStartTime = Time.time;
        };
        _play.OnFrame = () =>
        {
            if (Time.time >= _playStartTime + _config.playDuration)
            {
                StartCoroutine(StartPlayCooldown());
                _stateMachine.TransitionTo(UnityEngine.Random.value < 0.5f ? _idle : _roam);
            }
        };
        _play.OnExit = () => _currentPlayPartner = null;
    }

    #region State Handlers

    private void HandleIdleState()
    {
        if (_viewModel.Hunger < _config.hungryThreshold)
        {
            _stateMachine.TransitionTo(_hungry);
            return;
        }

        if (Time.time >= _startWaitTime)
        {
            if (UnityEngine.Random.value < _config.howlChance)
                _stateMachine.TransitionTo(_howl);
            else
                _stateMachine.TransitionTo(_roam);
        }
    }

    private void HandleRoamState()
    {
        if (!_hasRoamTarget)
        {
            ChooseNewRoamTarget();
            return;
        }

        MoveToward(_currentRoamTarget, _config.roamSpeed);
        RotateTowards(_currentRoamTarget);

        if (Vector3.Distance(transform.position, _currentRoamTarget) < 0.3f)
        {
            _stateMachine.TransitionTo(_idle);
            _hasRoamTarget = false;
        }

        if (_viewModel.Hunger < _config.hungryThreshold && GetAvailableFoodPlate() != null)
        {
            _stateMachine.TransitionTo(_eat);
        }

        if (Time.time >= _interactCooldownEnd && GetClosestInteractable() != null)
        {
            _stateMachine.TransitionTo(_interact);
        }
    }

    private void HandleInteractState()
    {
        if (_interactTargetPosition == null)
        {
            _stateMachine.TransitionTo(_roam);
            return;
        }

        MoveToward(_interactTargetPosition.Value, _alertSpeed);
        RotateTowards(_interactTargetPosition.Value);

        if (Vector3.Distance(transform.position, _interactTargetPosition.Value) < 0.3f)
        {
            if (_startWaitTime <= 0)
                _startWaitTime = Time.time + _config.interactDuration;

            if (Time.time >= _startWaitTime)
            {
                _viewModel.ChangeHunger(-10f);
                _viewModel.ChangeHappiness(10f);
                _interactCooldownEnd = Time.time + _config.interactCooldownSecs;
                _stateMachine.TransitionTo(_roam);
            }
        }
    }

    private void HandleHungryState()
    {
        if (GetAvailableFoodPlate() != null)
        {
            _stateMachine.TransitionTo(_eat);
        }
        else
        {
            _stateMachine.TransitionTo(_roam);
        }
    }

    private void HandleEatState()
    {
        if (_foodTargetPosition == null && !_isEating)
        {
            var plate = GetAvailableFoodPlate();
            if (plate == null)
            {
                _stateMachine.TransitionTo(_roam);
                return;
            }

            _foodTargetPosition = plate.transform.position;
        }

        if (!_isEating)
        {
            MoveToward(_foodTargetPosition.Value, _eatSpeed);
            RotateTowards(_foodTargetPosition.Value);

            if (Vector3.Distance(transform.position, _foodTargetPosition.Value) < 0.3f)
            {
                var plate = GetAvailableFoodPlate();
                if (plate != null)
                {
                    plate.ConsumeFood();
                    _viewModel.ChangeHunger(_config.hungerRestoreAmount);
                    _viewModel.ChangeHappiness(5f);
                }

                _isEating = true;
                _eatStartTime = Time.time;
            }
        }
        else
        {
            if (Time.time >= _eatStartTime + _config.eatDuration)
            {
                _isEating = false;
                _foodTargetPosition = null;
                _stateMachine.TransitionTo(_roam);
            }
        }
    }

    #endregion

    #region Helpers

    private void TryPlayWithNearbyPet(GameObject otherPetObj)
    {
        if (_isInPlayCooldown || _stateMachine.CurrentState == _play)
            return;

        PetAIController otherPet = otherPetObj.GetComponent<PetAIController>();
        if (otherPet == null || otherPet == this || otherPet._isInPlayCooldown)
            return;

        if (_recentPartners.Contains(otherPet) || otherPet._recentPartners.Contains(this))
            return;

        if (otherPet._currentPlayPartner == this)
            return;

        _currentPlayPartner = otherPet;
        otherPet._currentPlayPartner = this;

        _stateMachine.TransitionTo(_play);
        otherPet._stateMachine.TransitionTo(otherPet._play);
    }

    private IEnumerator StartPlayCooldown()
    {
        _isInPlayCooldown = true;

        if (_currentPlayPartner != null)
        {
            _recentPartners.Add(_currentPlayPartner);
            StartCoroutine(ClearRecentPartner(_currentPlayPartner));
        }

        yield return new WaitForSeconds(_config.playCooldown);
        _isInPlayCooldown = false;
    }

    private IEnumerator ClearRecentPartner(PetAIController partner)
    {
        yield return new WaitForSeconds(_config.playCooldown);
        _recentPartners.Remove(partner);
    }


    private void ChooseNewRoamTarget()
    {
        const int maxAttempts = 20;
        int attempts = 0;
        Vector3 candidate;

        do
        {
            float halfX = _groundSize.x / 2f;
            float halfZ = _groundSize.y / 2f;

            float randomX = UnityEngine.Random.Range(-halfX, halfX);
            float randomZ = UnityEngine.Random.Range(-halfZ, halfZ);

            candidate = new Vector3(randomX, transform.position.y, randomZ);

            attempts++;
        }
        while ((IsNearWall(candidate) || !IsInsideGround(candidate)) && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            D("Failed to find valid roam target after max attempts", true);
            candidate = transform.position; // fallback: stay put
        }

        _currentRoamTarget = candidate;
        _hasRoamTarget = true;
        D($"New roam target set at {_currentRoamTarget}");
    }

    private bool IsNearWall(Vector3 position)
    {
        foreach (var wallObj in _triggerSensor.Walls)
        {
            if (Vector3.Distance(wallObj.transform.position, position) < _wallAvoidanceRadius)
                return true;
        }
        return false;
    }

    private bool IsInsideGround(Vector3 position)
    {
        float halfX = _groundSize.x / 2f;
        float halfZ = _groundSize.y / 2f;

        return position.x >= -halfX && position.x <= halfX &&
               position.z >= -halfZ && position.z <= halfZ;
    }

    private void MoveToward(Vector3 target, float speed)
    {
        var current = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        var direction = (target - current).normalized;
        transform.position = current + direction * speed * Time.deltaTime;
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0; // keep horizontal rotation only
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    private Vector3? GetClosestInteractablePosition()
    {
        GameObject closest = GetClosestInteractable();
        if (closest != null)
        {
            var pos = closest.transform.Find("Position");
            if (pos != null) return pos.position;
        }
        return null;
    }

    private GameObject GetClosestInteractable()
    {
        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var obj in _triggerSensor.Interactables)
        {
            if (obj == null) continue;
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj;
            }
        }

        return closest;
    }

    private FoodPlate GetAvailableFoodPlate()
    {
        foreach (GameObject plateObj in _triggerSensor.FoodPlates)
        {
            if (plateObj == null) continue;
            FoodPlate plate = plateObj.GetComponent<FoodPlate>();
            if (plate != null && plate.HasFood) return plate;
        }
        return null;
    }

    private void D(string msg, bool isError = false)
    {
        if (isError)
            Debug.LogError($"<<{name}>> [PetAIController] {msg}");
        else
            Debug.Log($"<<{name}>> [PetAIController] {msg}");
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    Gizmos.DrawWireSphere(transform.position, _howlRadius);

    //    // Draw roam target
    //    if (_hasRoamTarget)
    //    {
    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawSphere(_currentRoamTarget, 0.2f);
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        if (_triggerSensor != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_triggerSensor.transform.position,
                _triggerSensor.GetComponent<SphereCollider>().radius);
        }

        // Draw ground bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.up * transform.position.y, new Vector3(_groundSize.x, 0.1f, _groundSize.y));
    }

    #endregion
}
