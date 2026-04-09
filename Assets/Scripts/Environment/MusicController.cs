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
        //if (player.time )
        //{

        //}
    }
}
