using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class instruction : MonoBehaviour
{
    [SerializeField] protected List<KeyCode> _key;
    //set to -1 if press, time to hold if hold
    [SerializeField] protected List<float> _holdTime;

    [SerializeField] protected float _nextWaitTime = 5f;

    protected bool _triggered;

    public virtual void Update()
    {
        if (_triggered) return;

        bool next = true;
        for (int i = 0; i < _key.Count; i++)
        {
            if (Input.GetKey(_key[i])) {
                _holdTime[i] = Mathf.Clamp(_holdTime[i] - Time.deltaTime, 0, _holdTime[i]);
            }
            if (_holdTime[i] != 0)
            {
                next = false;
            }
        }
        if (next)
        {
            _triggered = true;
            StartCoroutine(tutorial.Instance.NextInstruction(_nextWaitTime));
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            bool _skipParent = true;
            foreach (Transform child in childTransforms)
            {
                if (_skipParent)
                {
                    _skipParent = false;
                    return;
                }
                child.gameObject.SetActive(false);
            }
        }

    }
}
