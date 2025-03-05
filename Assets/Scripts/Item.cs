
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public ScriptableItem scriptableItem;

    public float Weight => scriptableItem.Weight;
}
