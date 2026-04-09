using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    public bool _running = false;
    public bool _sleeping = false;

    [SerializeField] public float _time;
    [SerializeField] float _startTime;
    public float _secsPerDay = 1440;

    public TMP_Text _timeText;
    public Image _timeIcon;

    public Transform _sunTransform;
    public Light _sun;
    public int _day;
    public float _intensity;
    public Color _fogDay;
    public Color _fogNight;

    public SkyboxController _skyboxController;

    private void Start()
    {
        _time = _startTime;
        _skyboxController = GetComponent<SkyboxController>();
        _skyboxController._secsPerDay = _secsPerDay;
        _running = true;
    }
    void Update()
    {
        UpdateTime();

        if (_skyboxController != null)
        {
            _skyboxController.UpdateTime(_time);
        }
        else
        {
            _skyboxController = GetComponent<SkyboxController>();
        }
    }

    public void UpdateTime()
    {
        if (_running)
        {
            _time += Time.deltaTime;
            if (_sleeping)
            {
                _time += Time.deltaTime * 24;
            }
        }
        //update time and loop
        _time = Mathf.Repeat(_time, _secsPerDay);

        //update day after reset
        if (_time < Time.deltaTime)
        {
            _day++;
            Debug.Log("next day");
        }

        _timeText.text = GetTime();

        float normalizedTime = _time / _secsPerDay;

        //sun rotation
        float sunAngle = (normalizedTime - 0.25f) * 360f;
        _sunTransform.rotation = Quaternion.Euler(sunAngle, 0, 0);

        //UI icon rotation
        _timeIcon.rectTransform.rotation =
            Quaternion.Euler(0, 0, normalizedTime * 360f);

        //light intensity
        _intensity = Mathf.Clamp01(Mathf.Cos(normalizedTime * Mathf.PI * 2) * -0.5f + 0.5f);
        _sun.intensity = _intensity;

        if (_sleeping && _time > 360 &&  _time < 1260)
        {
            _sleeping = false;
        }
    }

    public string GetTime()
    {
        //irl sec = in game min

        return(TimeSpan.FromSeconds((_time/_secsPerDay)*1440).ToString(@"mm\:ss"));
    }

    public void Sleep()
    {
        _sleeping = true;
    }
}
