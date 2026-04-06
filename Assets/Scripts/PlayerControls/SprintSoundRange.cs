using UnityEngine;

public class SprintSoundRange : MonoBehaviour
{
    [SerializeField] Collider area;

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SendMessageUpwards("AddSprintAnimal", other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.SendMessageUpwards("RemoveSprintAnimal", other.gameObject);
    }
}
