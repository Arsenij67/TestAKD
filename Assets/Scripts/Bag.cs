
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class Bag : MonoBehaviour
{
    [Inject] private List<Item> items;
    private  const float MaxWeight = 30; // максимальный вес сумки
    private float freeWeight = MaxWeight;
    private float throwPower = 100f;
    [SerializeField] private TMP_Text textFullness;
    [SerializeField] private Transform placeItemSpawn;

    public void AddItem(Item item)
    {
        if ((freeWeight - item.Weight) >= 0)
        {
            freeWeight -= item.Weight;
            items.Add(item);
            UpdateUIFullness(string.Format($"{MaxWeight - freeWeight}/{MaxWeight}"));
            Destroy(item.transform.gameObject);
        }
         
     
    }

    public void RemoveLastItem()
    {
        if (items.Count()==0) return;
        Debug.Log(000);
        Item item = items.Last();
        GameObject go = PullItem(item);
        ThrowItem(go);
        freeWeight += item.Weight;
        items.Remove(item);
        UpdateUIFullness(string.Format($"{MaxWeight - freeWeight}/{MaxWeight}"));
        
    }

    private GameObject PullItem(Item item)
    {
      return Instantiate(item.scriptableItem.Prefab, placeItemSpawn.position,Quaternion.identity);
       
    }

    private void ThrowItem(GameObject itemObject)
    {
        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        rb.AddForce(-Vector3.forward * 4,ForceMode.Impulse);
    }

    private void UpdateUIFullness(string updatedText)
    {
        textFullness.text = updatedText;
    }


}
