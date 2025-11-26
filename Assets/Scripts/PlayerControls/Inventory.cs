using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> _items = new List<GameObject>();
    [SerializeField] CameraController _cameraController;
    public int _curItem;

    private void Update()
    {
        if (_curItem == 0 && _cameraController.VF) return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            previousItem();
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            nextItem();
        }
    }

    void nextItem()
    {
        _items[_curItem].SetActive(false);
        _curItem = (_curItem + 1) % _items.Count;
        _items[_curItem].SetActive(true);
    }

    void previousItem()
    {
        _items[_curItem].SetActive(false);
        _curItem = (_curItem + _items.Count - 1) % _items.Count;
        _items[_curItem].SetActive(true);
    }

    void setItem(int item)
    {
        _items[_curItem].SetActive(false);
        _curItem = item-1;
        _items[_curItem].SetActive(true);
    }
}
