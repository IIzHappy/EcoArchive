using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.HighDefinition;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Camera playerCam;

    [SerializeField] GameObject UI;
    [SerializeField] GameObject camUI;

    [SerializeField] VolumeProfile volumeProfile;
    DepthOfField dof;

    public float adjVal;

    int resWidth, resHeight;

    private bool VF = false;

    private void Start()
    {
        cam.usePhysicalProperties = true;
        DepthOfField test;

        resHeight = cam.pixelHeight; 
        resWidth = cam.pixelWidth;

        if (volumeProfile.TryGet<DepthOfField>(out test))
        {
            dof = test;
        }
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            adjVal++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            adjVal--;
        }

        if (VF)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Take photo
                Debug.Log("Snap");

                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                cam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                cam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                cam.targetTexture = null;
                RenderTexture.active = null; // JC: added to avoid errors
                Destroy(rt);
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = string.Format(Application.dataPath + "/Player Images/" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("Took screenshot to: {0}", filename));
            }

            if (Input.anyKey)
            {
                if (Input.GetKey("g"))
                {
                    //Aperture
                    dof.aperture.value += adjVal;
                    if (dof.aperture.value > 32)
                    {
                        dof.aperture.value = 32;
                    }
                    else if (dof.aperture.value < 0.95f)
                    {
                        dof.aperture.value = 0.95f;
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
                    dof.focalLength.value += adjVal;
                }

                else if (Input.GetKey("r"))
                {
                    //focal distance
                    dof.focusDistance.value += (adjVal * 10);
                    if (dof.focusDistance.value > 135f)
                    {
                        dof.focusDistance.value = 135;
                    }
                    else if (dof.focusDistance.value < 35f)
                    {
                        dof.focusDistance.value = 35;
                    }
                }

                else if (Input.GetKey("t"))
                {
                    //iso
                    cam.iso += (int) adjVal;
                }
            }
        }
        adjVal = 0;
    }
}
