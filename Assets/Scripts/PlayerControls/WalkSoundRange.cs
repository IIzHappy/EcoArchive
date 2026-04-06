using UnityEngine;

public class WalkSoundRange : MonoBehaviour
{
    [SerializeField] Collider area;

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SendMessageUpwards("AddWalkAnimal", other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.SendMessageUpwards("RemoveWalkAnimal", other.gameObject);
    }
}