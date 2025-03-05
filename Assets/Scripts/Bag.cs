using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Bag : MonoBehaviour
{
    [Inject] private List<Item> items;

    private const float MaxWeight = 30f; // Максимальный вес сумки
    private float freeWeight = MaxWeight;

    [SerializeField] private TMP_Text  textFullness;
    [SerializeField] private Transform placeItemSpawn;
    [SerializeField] private Button buttonThrow;
    private void Start()
    {
        UpdateUI();
    }
    public void AddItem(Item item)
    {
        if (CanAddItem(item))
        {
            AddItemToBag(item);
            UpdateUI();
            Destroy(item.gameObject);
        }
    }

    public void RemoveLastItem()
    {
        if (IsBagEmpty()) return;

        Item lastItem = GetLastItem();
        GameObject itemObject = SpawnItem(lastItem);
        ThrowItem(itemObject);

        RemoveItemFromBag(lastItem);
        UpdateUI();
    }

    private bool CanAddItem(Item item)
    {
        return freeWeight - item.Weight >= 0;
    }

    private void AddItemToBag(Item item)
    {
        freeWeight -= item.Weight;
        items.Add(item);
    }

    private bool IsBagEmpty()
    {
        return !items.Any();
    }

    private Item GetLastItem()
    {
        return items.Last();
    }

    private GameObject SpawnItem(Item item)
    {
        item.scriptableItem.Prefab.GetComponent<Renderer>().material = item.material;
        return Instantiate(item.scriptableItem.Prefab, placeItemSpawn.position, Quaternion.identity);
    }

    private void ThrowItem(GameObject itemObject)
    {
        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        rb.AddForce(-transform.forward * 4, ForceMode.Impulse);
    }

    private void RemoveItemFromBag(Item item)
    {
        freeWeight += item.Weight;
        items.Remove(item);
    }

    private void UpdateUI()
    {
        textFullness.text = $"{MaxWeight - freeWeight}/{MaxWeight}";

        if (items.Count() <= 0)
        {
            HideButtonThrow();
        }
        else
        {
            ShowButtonThrow();
        }
    }
    private void HideButtonThrow()
    {
        buttonThrow.gameObject.SetActive(false);
    }
    private void ShowButtonThrow()
    {
        buttonThrow.gameObject.SetActive(true);
    }
}