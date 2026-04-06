using UnityEngine;

public class SlowSoundRange : MonoBehaviour
{
    [SerializeField] Collider area;

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SendMessageUpwards("AddSlowAnimal", other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.SendMessageUpwards("RemoveSlowAnimal", other.gameObject);
    }
}