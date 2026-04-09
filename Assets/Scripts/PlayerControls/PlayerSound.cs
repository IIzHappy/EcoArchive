using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public List<GameObject> _walkRangeAnimals = new List<GameObject>();
    public List<GameObject> _sprintRangeAnimals = new List<GameObject>();
    public List<GameObject> _slowRangeAnimals = new List<GameObject>();

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
        AlertAnimals(_walkState);
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

    void AlertAnimals(int state)
    {
        switch (state)
        {
            case 0:
                foreach (GameObject animal in _walkRangeAnimals)
                {
                    animal.GetComponent<AnimalNavBase>().playerFlee(this.transform);
                }
                break;
            case 1:
                foreach (GameObject animal in _sprintRangeAnimals)
                {
                    animal.GetComponent<AnimalNavBase>().playerFlee(this.transform);
                }
                break;
            case 2:
                foreach (GameObject animal in _slowRangeAnimals)
                {
                    animal.GetComponent<AnimalNavBase>().playerFlee(this.transform);
                }
                break;

            default:
                Debug.Log("Ruh Roh");
                break;
        }
    }

    void AddWalkAnimal(GameObject animal)
    {
        _walkRangeAnimals.Add(animal);
    }
    void AddSprintAnimal(GameObject animal)
    {
        _sprintRangeAnimals.Add(animal);
    }
    void AddSlowAnimal(GameObject animal)
    {
        _slowRangeAnimals.Add(animal);
    }

    void RemoveWalkAnimal(GameObject animal)
    {
        _walkRangeAnimals.Remove(animal);
    }
    void RemoveSprintAnimal(GameObject animal)
    {
        _sprintRangeAnimals.Remove(animal);
    }
    void RemoveSlowAnimal(GameObject animal)
    {
        _slowRangeAnimals.Remove(animal);
    }
}