using UnityEngine;
using UnityEngine.InputSystem;

public class spawnBone : instruction
{
    [SerializeField] GameObject _bone;
    [SerializeField] GameObject _crosshair;
    private void OnEnable()
    {
        _bone.SetActive(true);
    }

    public override void Update()
    {
        if (_crosshair.activeInHierarchy && Input.GetKeyDown(KeyCode.E)) StartCoroutine(tutorial.Instance.NextInstruction(_nextWaitTime));
    }
}
