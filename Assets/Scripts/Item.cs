
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Item : MonoBehaviour
{
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    public ScriptableItem scriptableItem;

    public Material material;
    public float Weight => scriptableItem.Weight;
}
