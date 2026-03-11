using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventFeed : MonoBehaviour
{
    public static EventFeed Instance { get; private set; }
    [SerializeField] Transform feed;
    [SerializeField] GameObject notifPrefab;
    [SerializeField] float notifLifetime = 5;

    private void Start()
    {
        Instance = this;
    }

    public void makeNotif(Sprite icon, string text)
    {
        GameObject notif = Instantiate(notifPrefab, feed);
        Destroy(notif, notifLifetime);
        notif.GetComponentsInChildren<Image>()[1].sprite = icon;
        notif.GetComponentInChildren<TMP_Text>().text = text;
    }
}
