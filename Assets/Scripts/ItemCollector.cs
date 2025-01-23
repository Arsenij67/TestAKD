using UnityEngine;
using Zenject;

public class ItemCollector : MonoBehaviour
{
    [Inject] private Bag bag;

    private void CollectFromShilf(Vector2 coordinates)
    {
        Ray ray = Camera.main.ScreenPointToRay(coordinates);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 4f))
        {

            Item item = hitInfo.transform.GetComponent<Item>();
            if (item != null)
            {
                bag.AddItem(item);
            }

        }
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) // Используйте GetMouseButtonDown для однократного срабатывания
        {
            CollectFromShilf(Input.mousePosition);
            TakeToPickup(Input.mousePosition);
        }
#elif PLATFORM_ANDROID
        // На Android используем ввод касания
        if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Обрабатываем палец
                if (touch.phase == TouchPhase.Began)
                {
                    CollectFromShilf(touch.position);
                    TakeToPickup(touch.position);
                }
            }
#endif

    }

    private void TakeToPickup(Vector2 coordinates)
    {
        Ray ray = Camera.main.ScreenPointToRay(coordinates);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 8f))
        {
            PickupContainer item = hitInfo.transform.GetComponent<PickupContainer>();
            if (item != null)
            {

                bag.RemoveLastItem();
            }


        }
    }
}