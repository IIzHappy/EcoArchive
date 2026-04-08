using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioMixerGroup ambientSource;
    [SerializeField] PlayerRotateCam playerController;
    [SerializeField] CameraController cameraController;
    [SerializeField] TMP_Text text;
    [SerializeField] Slider[] sliders;

    public void SetSliders(float[] settings)
    {
        if (sliders != null)
        {
            sliders[0].value = settings[0];
            sliders[1].value = settings[1];
            sliders[2].value = settings[2];
            sliders[3].value = settings[3] / 150;
        }
    }

    public void setMasterVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }

    public void SetMusicVolume(Slider slider)
    {
        musicSource.volume = slider.value;
    }

    public void SetAmbientVolume(Slider slider)
    {
        ambientSource.audioMixer.SetFloat("Master", Mathf.Log10(slider.value) * 20);
    }

    public void SetSensitivity(Slider slider)
    {
        playerController._mouseSensX = slider.value*150;
        playerController._mouseSensY = slider.value*150;
    }

    public void ToggleAdvanced()
    {
        cameraController.advancedCam = !cameraController.advancedCam;
        if (cameraController.advancedCam )
        {
            text.text = "Active";
        }
        else
        {
            text.text = "Inactive";
        }
    }

    public float[] GetSettings()
    {
        float amb;
        ambientSource.audioMixer.GetFloat("Master", out amb);
        float[] settings = {AudioListener.volume, musicSource.volume, amb, playerController._mouseSensX};
        return settings;
    }
}
