using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class tutorial : MonoBehaviour
{
    public static tutorial Instance { get; private set; }

    int _instruction;

    [SerializeField] List<GameObject> _ui;

    void Awake()
    {
        Instance = this;
    }
    public IEnumerator NextInstruction(float waitForTime)
    {
        _instruction++;
        if (_instruction < _ui.Count)
        {
            yield return new WaitForSeconds(waitForTime);

            Debug.Log(2);
            _ui[_instruction].SetActive(true);
            _ui[_instruction-1].SetActive(false);
        }
    }
}
