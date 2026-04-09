using System.Linq;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] dayMusic;
    [SerializeField] AudioClip[] nightMusic;
    [SerializeField] DayNightCycle day;
    [SerializeField] AudioSource player;

    private void Start()
    {
        player.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying)
        {
            if (day._time >= 1200 || day._time <= 420)
            {
                player.clip = nightMusic[Random.Range(0, nightMusic.Count())];
            }
            else
            {
                player.clip = dayMusic[Random.Range(0, dayMusic.Count())];
            }
        }
        player.Play();
    }
}
