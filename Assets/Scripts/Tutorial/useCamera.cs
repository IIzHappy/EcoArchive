using UnityEngine;

public class useCamera : instruction
{
    [SerializeField] GameObject _camera;
    public override void Update()
    {
        if (_triggered) return;
        if (_camera.activeInHierarchy)
        {
            _triggered = true;
            StartCoroutine(tutorial.Instance.NextInstruction(_nextWaitTime));
        }
    }
}
