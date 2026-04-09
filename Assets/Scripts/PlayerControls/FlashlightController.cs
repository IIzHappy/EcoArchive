using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] Light light;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            light.enabled = !light.enabled;
        }
    }
}
