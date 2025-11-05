using System.Collections;
using UnityEngine;

public class NetController : MonoBehaviour
{
    public bool _swinging;
    public float _swingTime = 0.7f;
    public float _swingResetTime = 0.3f;

    [SerializeField] Collider _collider;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_swinging)
        {
            Debug.Log("net swing");
            Swing(_swingTime);
        }
    }

    IEnumerator Swing(float duration)
    {
        _swinging = true;
        _collider.enabled = true;
        //net swing animation
        yield return new WaitForSeconds(duration);
        _collider.enabled = false;
        SwingReset(_swingResetTime);
    }
    IEnumerator SwingReset(float duration)
    {
        yield return new WaitForSeconds(duration);
        _swinging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bugs")
        {
            Collection.Instance.AddBug(other.GetComponent<Collectables>()._bug);
            Destroy(other.gameObject);
        }
    }
}
