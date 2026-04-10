using UnityEngine;
using UnityEngine.SceneManagement;

public class toGame : instruction
{
    [SerializeField] DayNightCycle _cycle;
    [SerializeField] GameObject _player;
    public override void Update()
    {
        if (_cycle._time > 700) SceneManager.LoadScene("Prototype scene");
        Destroy(_player);
    }
}
