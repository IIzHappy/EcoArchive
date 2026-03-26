using UnityEngine;

[ExecuteAlways]
public class PlayerTracker : MonoBehaviour
{
    public Material grassMat;
    
    void Update()
    {
        grassMat.SetVector("_PlayerPosition", gameObject.transform.position);
    }
}
