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
        playerController._mouseSensX = slider.value*250;
        playerController._mouseSensY = slider.value*250;
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
}
