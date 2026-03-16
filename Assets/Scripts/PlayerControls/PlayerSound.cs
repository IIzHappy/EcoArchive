using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    int walkState = 0;
    //0-walk
    //1-sprint
    //2-slow walk

    public float[] _stepInterval = { 0.8f, 0.6f, 1.2f };

    public AudioClip[] _stepSound;

    PlayerController _player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWalkState()
    {

    }

    public void StartMoving()
    {

    }

    void PlayStepSound()
    {

    }
}
