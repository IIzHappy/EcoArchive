using UnityEngine;

public class checkPhoto : instruction
{
    public override void Update()
    {
        if (_triggered) return;

        if (Collection.Instance._photos.Count > 0)
        {
            _triggered = true;
            StartCoroutine(tutorial.Instance.NextInstruction(_nextWaitTime));
        }
    }
}
