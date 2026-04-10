using UnityEngine;

public class AnimalNoiseMaker : MonoBehaviour
{
    [SerializeField] AudioSource player;
    [SerializeField] AudioClip[] noises;

    private void Start()
    {
        player.maxDistance = 30;
    }

    private void Update()
    {
        if (Random.Range(0, 100000) == 555)
        {
            player.PlayOneShot(noises[Random.Range(0, noises.Length)]);
        }
    }
}
