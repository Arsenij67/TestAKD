using UnityEngine;
using Zenject;

public class ItemCollector : MonoBehaviour
{
    [Inject] private Bag bag;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#elif PLATFORM_ANDROID
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 inputPosition = Input.mousePosition;
            ProcessInput(inputPosition);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 inputPosition = touch.position;
                ProcessInput(inputPosition);
            }
        }
    }

    private void ProcessInput(Vector2 inputPosition)
    {
        CollectFromShelf(inputPosition);
        OpenGarage(inputPosition);
    }

    private void CollectFromShelf(Vector2 coordinates)
    {
        if (TryGetItemAtCoordinates(coordinates, 4f, out Item item))
        {
            bag.AddItem(item);
        }
    }
    private void OpenGarage(Vector2 coordinates)
    {
        if (TryGetLockAtCoordinates(coordinates, 8f, out GarageLock locker))
        {
            locker.OpenDoor();
        }
    }

    public void OnPutDownToPickup()
    {

        bag.RemoveLastItem();
      
    }

    private bool TryGetItemAtCoordinates(Vector2 coordinates, float maxDistance, out Item item)
    {
        item = null;
        if (TryRaycast(coordinates, maxDistance, out RaycastHit hitInfo))
        {
            item = hitInfo.transform.GetComponent<Item>();
        }
        return item != null;
    }

    private bool TryGetLockAtCoordinates(Vector2 coordinates, float maxDistance, out GarageLock pickupContainer)
    {
        pickupContainer = null;
        if (TryRaycast(coordinates, maxDistance, out RaycastHit hitInfo))
        {
            pickupContainer = hitInfo.transform.GetComponent<GarageLock>();
        }
        return pickupContainer != null;
    }

    private bool TryRaycast(Vector2 coordinates, float maxDistance, out RaycastHit hitInfo)
    {
        Ray ray = Camera.main.ScreenPointToRay(coordinates);
        return Physics.Raycast(ray, out hitInfo, maxDistance);
    }
}