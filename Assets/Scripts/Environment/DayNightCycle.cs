using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public float _time;
    public float _fullDayTime = 1440;

    public TMP_Text _timeText;
    public Image _timeIcon;

    public Transform _sunTransform;
    public Light _sun;
    public int _day;
    public float _intensity;
    public Color _fogDay;
    public Color _fogNight;

    void Update()
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        _time += Time.deltaTime;
        _timeText.text = GetTime();
        if (_time > _fullDayTime)
        {
            Debug.Log("next day");
            _day++;
            _time = 0;
        }
        _sunTransform.rotation = Quaternion.Euler(new Vector3((_time-_fullDayTime/4)/_fullDayTime*360, 0, 0));
        _timeIcon.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -(_time - _fullDayTime / 4) / _fullDayTime * 360));

        if (_time < _fullDayTime / 2)
        {
            _intensity = 1 - (_fullDayTime / 2 - _time) / (_fullDayTime / 2);
        }
        else
        {
            _intensity = 1 - (_fullDayTime / 2 - _time) / (-_fullDayTime / 2);
        }
    }

    public string GetTime()
    {
        //irl sec = in game min
        return(TimeSpan.FromSeconds(_time).ToString(@"mm\:ss"));
    }
}
