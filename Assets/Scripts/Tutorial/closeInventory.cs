using UnityEngine;

public class closeInventory : instruction
{
    [SerializeField] GameObject _inventory;
    public override void Update()
    {
        if (_triggered) return;
        if (!_inventory.activeInHierarchy)
        {
            _triggered = true;
            StartCoroutine(tutorial.Instance.NextInstruction(_nextWaitTime));
        }
    }
}
