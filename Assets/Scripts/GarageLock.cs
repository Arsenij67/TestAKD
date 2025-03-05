using Zenject;
using UnityEngine;

public class GarageLock : MonoBehaviour
{
    [Inject] private Animation animOpen;
    public void OpenDoor()
    {
        animOpen.Play();
        ChangeAppearance();
    }

    private void ChangeAppearance()
    { 
        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(0.3f, 0.3f, 0.3f);
    }
}
