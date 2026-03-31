using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SkyboxController : MonoBehaviour
{
    public List<SkyboxColours> _skyColours = new List<SkyboxColours>();
    public Material _skyboxMaterial;
    public Material _cloudMaterial;

    public float _secsPerDay = 1440;


    private void Awake()
    {
        GetTimes();
    }
    void OnValidate()
    {
        GetTimes();
    }
    public void GetTimes()
    {
        foreach (SkyboxColours colours in _skyColours)
        {
            colours.StartTimeSeconds = StringToTime(colours._startTime);
        }
        _skyColours.Sort((a, b) => a.StartTimeSeconds.CompareTo(b.StartTimeSeconds));
    }

    public void UpdateTime(float time)
    {
        if (_skyColours.Count == 0 || _skyboxMaterial == null) return;

        time = Mathf.Repeat(time, _secsPerDay);

        int prevIndex = -1;
        int nextIndex = -1;

        for (int i = 0; i < _skyColours.Count; i++)
        {
            float t = _skyColours[i].StartTimeSeconds;

            if (t <= time)
                prevIndex = i;

            if (t > time)
            {
                nextIndex = i;
                break;
            }
        }

        //wrap around
        if (prevIndex == -1)
            prevIndex = _skyColours.Count - 1;

        if (nextIndex == -1)
            nextIndex = 0;

        var prev = _skyColours[prevIndex];
        var next = _skyColours[nextIndex];

        float startTime = prev.StartTimeSeconds;
        float endTime = next.StartTimeSeconds;

        float blend = GetBlend(time, startTime, endTime, _secsPerDay);

        ChangeColours(prev, next, blend, time);
    }

    int GetFirstIndex()
    {
        int index = 0;
        float min = float.MaxValue;

        for (int i = 0; i < _skyColours.Count; i++)
        {
            if (_skyColours[i].StartTimeSeconds < min)
            {
                min = _skyColours[i].StartTimeSeconds;
                index = i;
            }
        }

        return index;
    }

    int GetLastIndex()
    {
        int index = 0;
        float max = float.MinValue;

        for (int i = 0; i < _skyColours.Count; i++)
        {
            if (_skyColours[i].StartTimeSeconds > max)
            {
                max = _skyColours[i].StartTimeSeconds;
                index = i;
            }
        }

        return index;
    }

    float GetBlend(float time, float start, float end, float dayLength)
    {
        if (Mathf.Approximately(start, end))
            return 0f;

        if (end > start)
        {
            return Mathf.InverseLerp(start, end, time);
        }
        else
        {
            float duration = (dayLength - start) + end;

            float t = (time >= start) ? time - start : (dayLength - start) + time;

            return t / duration;
        }
    }

    float StringToTime(string timeString)
    {
        return (float)TimeSpan.ParseExact(timeString, @"hh\:mm", null).TotalMinutes;
    }

    void ChangeColours(SkyboxColours current, SkyboxColours next, float timeBlend, float time)
    {
        _skyboxMaterial.SetColor("_SkyTopColour", current._skyTop);
        _skyboxMaterial.SetColor("_SkyColour", current._sky);
        _skyboxMaterial.SetColor("_HorizonColour", current._horizon);
        _skyboxMaterial.SetColor("_SunriseColour", current._sunrise);
        _skyboxMaterial.SetFloat("_HorizonSunriseRadius", current._sunriseRadius);
        _skyboxMaterial.SetFloat("_SkyBlendHeight", current._skyBlendHeight);
        _skyboxMaterial.SetFloat("_StarIntensity", current._starIntensity);
        _cloudMaterial.SetColor("_CloudColour", current._cloudColour);
        _cloudMaterial.SetColor("_CloudMidColour", current._cloudMidColour);
        _cloudMaterial.SetColor("_CloudEdgeColour", current._cloudEdgeColour);
        _cloudMaterial.SetFloat("_MidColourSize", current._cloudMidSize);
        _cloudMaterial.SetFloat("_EdgeColourSize", current._cloudEdgeSize);

        _skyboxMaterial.SetColor("_SkyTopColour2", next._skyTop);
        _skyboxMaterial.SetColor("_SkyColour2", next._sky);
        _skyboxMaterial.SetColor("_HorizonColour2", next._horizon);
        _skyboxMaterial.SetColor("_SunriseColour2", next._sunrise);
        _skyboxMaterial.SetFloat("_HorizonSunriseRadius2", next._sunriseRadius);
        _skyboxMaterial.SetFloat("_SkyBlendHeight2", next._skyBlendHeight);
        _skyboxMaterial.SetFloat("_StarIntensity2", next._starIntensity);
        _cloudMaterial.SetColor("_CloudColour2", next._cloudColour);
        _cloudMaterial.SetColor("_CloudMidColour2", next._cloudMidColour);
        _cloudMaterial.SetColor("_CloudEdgeColour2", next._cloudEdgeColour);
        _cloudMaterial.SetFloat("_MidColourSize2", next._cloudMidSize);
        _cloudMaterial.SetFloat("_EdgeColourSize2", next._cloudEdgeSize);

        float startTime = time < 720 ? time + 1440 : time;
        _skyboxMaterial.SetFloat("_StarTime", time);

        _skyboxMaterial.SetFloat("_TimeBlend", Mathf.Clamp01(timeBlend));
        _cloudMaterial.SetFloat("_TimeBlend", Mathf.Clamp01(timeBlend));
    }
}
