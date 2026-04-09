using UnityEngine;

public class AnimalNoiseMaker : MonoBehaviour
{
    [SerializeField] AudioSource player;
    [SerializeField] AudioClip[] noises;

    private void Update()
    {
        if (Random.Range(0, 1000) == 555)
        {
            player.PlayOneShot(noises[Random.Range(0, noises.Length)]);
        }
    }
}
