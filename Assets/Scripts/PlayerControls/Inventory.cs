using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<GameObject> _items = new List<GameObject>();
    public int _curItem;

    void nextItem()
    {
        _curItem = (_curItem + 1) % _items.Count;
        //code to take out next item
    }

    void previousItem()
    {
        _curItem = (_curItem + -_items.Count - 1) % _items.Count;
    }
}
