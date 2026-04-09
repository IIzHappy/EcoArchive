using UnityEngine;

public class SprintSoundRange : MonoBehaviour
{
    [SerializeField] Collider area;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Animal")) this.gameObject.SendMessageUpwards("AddSprintAnimal", other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Animal")) this.gameObject.SendMessageUpwards("RemoveSprintAnimal", other.gameObject);
    }
}
