using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour, ITickable, IInitializable
{
    private Camera _mainCamera;
    private GameManager _gameManager;

    private float _pinchZoomSpeed = 0.1f;
    private float _panSpeed = 0.01f;

    [Inject]
    public void Construct([Inject(Id = "MainCamera")] Camera camera, GameManager gameManager)
    {
        _mainCamera = camera;
        _gameManager = gameManager;
    }

    public void Initialize()
    {
        Debug.Log("PlayerController initialized.");
    }

    public void Tick()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Tap
            if (touch.phase == TouchPhase.Ended && touch.tapCount == 1)
            {
                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject hitObject = hit.collider.gameObject;

                    if (hitObject.CompareTag("Poop"))
                    {
                        Destroy(hitObject);
                        _gameManager.AddCoins(5);
                    }
                    else if (hitObject.CompareTag("Pet"))
                    {
                        ZoomToTarget(hitObject.transform.position);
                    }
                }
            }

            // Pan
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                _mainCamera.transform.Translate(-delta.x * _panSpeed, -delta.y * _panSpeed, 0);
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevDistance = (t0Prev - t1Prev).magnitude;
            float currentDistance = (t0.position - t1.position).magnitude;

            float delta = currentDistance - prevDistance;

            _mainCamera.fieldOfView -= delta * _pinchZoomSpeed;
            _mainCamera.fieldOfView = Mathf.Clamp(_mainCamera.fieldOfView, 20f, 60f);
        }
    }

    private void ZoomToTarget(Vector3 targetPosition)
    {
        Vector3 offset = new Vector3(0, 2f, -3f);
        _mainCamera.transform.position = targetPosition + offset;
        _mainCamera.transform.LookAt(targetPosition);
    }
}
