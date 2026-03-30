using System.Collections;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public bool _isMoving;
    
    int _walkState = 0;
    //0-walk
    //1-sprint
    //2-slow walk

    public float[] _stepInterval = { 0.8f, 0.6f, 1.2f };

    public AudioClip[] _stepSound;
    public AudioSource _audioSource;

    PlayerController _player;

    Coroutine _steppingInterval;

    float _timeSinceStep = 5f;

    void Update()
    {
        _timeSinceStep += Time.deltaTime;
    }

    public void UpdateWalkState(int walkstate)
    {
        if (walkstate == _walkState) return;

        _walkState = walkstate;

        if (_isMoving)
        {
            StopCoroutine(_steppingInterval);
            _steppingInterval = StartCoroutine(StepTimer(_stepInterval[_walkState] - _timeSinceStep));
        }
    }

    public void UpdateMoving(bool moving)
    {
        if (moving && !_isMoving)
        {
            _steppingInterval = StartCoroutine(StepTimer(_stepInterval[_walkState] - _timeSinceStep));
        }
        else if (!moving && _isMoving)
        {
            StopCoroutine(_steppingInterval);
        }
        _isMoving = moving;
    }

    IEnumerator StepTimer(float stepTime)
    {
        yield return new WaitForSeconds(stepTime);
        PlayStepSound();
        _timeSinceStep = 0;
        if (_isMoving)
        {
            _steppingInterval = StartCoroutine(StepTimer(_stepInterval[_walkState]));
        }
    }

    void PlayStepSound()
    {
        //randomize sound
        _audioSource.PlayOneShot(_stepSound[_walkState]);
    }
}
