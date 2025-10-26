using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Camera playerCam;

    [SerializeField] GameObject UI;
    [SerializeField] GameObject camUI;

    public float adjVal;

    private bool VF = false;

    private void Start()
    {
        cam.usePhysicalProperties = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            VF = true;
            UI.SetActive(false);
            camUI.SetActive(true);
            cam.enabled = true;
            playerCam.enabled = false;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            VF = false;
            UI.SetActive(true);
            camUI.SetActive(false);
            cam.enabled = false;
            playerCam.enabled = true;
        }
        
        adjVal = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.UpArrow))
        {
            adjVal++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            adjVal--;
        }

        if (VF)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Take photo
                Debug.Log("Snap");
            }

            if (Input.anyKey)
            {
                if (Input.GetKey("g"))
                {
                    //Aperture
                    cam.aperture += adjVal;
                    if (cam.aperture > 32)
                    {
                        cam.aperture = 32;
                    }
                    else if (cam.aperture < 0.95f)
                    {
                        cam.aperture = 0.95f;
                    }
                }

                else if (Input.GetKey("x"))
                {
                    //Shutter Speed
                    cam.shutterSpeed += adjVal;
                    if (cam.shutterSpeed > 0.1f)
                    {
                        cam.shutterSpeed = 0.1f;
                    }
                }

                else if (Input.GetKey("c"))
                {
                    //Barrel Clipping
                    cam.barrelClipping += adjVal;

                }

                else if (Input.GetKey("f"))
                {
                    //focal length

                }

                else if (Input.GetKey("r"))
                {
                    //focal distancesss

                }

                else if (Input.GetKey("t"))
                {
                    //iso

                }
            }
        }
        adjVal = 0;
    }
}
