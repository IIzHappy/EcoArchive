using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera VFCam;

    [SerializeField] GameObject UI;
    [SerializeField] GameObject camUI;

    [SerializeField] VolumeProfile volumeProfile;
    DepthOfField dof;

    public float adjVal;

    public int photoNum = 0;

    int resWidth, resHeight;

    public bool VF = false;

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
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            adjVal = Input.GetAxis("Mouse ScrollWheel") * 10f;
        }
        else
        {
            adjVal = Input.GetAxis("Mouse ScrollWheel");
        }

        if (VF)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Take photo
                photoNum++;
                StartCoroutine(TakePhoto());
            }

            if (Input.anyKey)
            {
                if (Input.GetKey("c"))
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

                else if (Input.GetKey("f"))
                {
                    //focal length
                    dof.focalLength.value += adjVal;
                    if (dof.focalLength.value > 135f)
                    {
                        dof.focalLength.value = 135;
                    }
                    else if (dof.focalLength.value < 35f)
                    {
                        dof.focalLength.value = 35;
                    }
                }

                else if (Input.GetKey("r"))
                {
                    //focal distance
                    dof.focusDistance.value += adjVal;
                }

                else
                {
                    cam.focalLength +=  adjVal * 10f;
                    VFCam.focalLength +=  adjVal * 10f;
                    if (cam.focalLength > 100f)
                    {
                        cam.focalLength = 100f;
                        VFCam.focalLength = 100f;
                    }
                    else if (cam.focalLength < 20f)
                    {
                        cam.focalLength = 20;
                        VFCam.focalLength = 20;
                    }
                }
            }
        }
        adjVal = 0;
    }

    private IEnumerator TakePhoto()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = string.Format(Application.dataPath + "/Player Images/" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "." + photoNum + ".png");
        System.IO.File.WriteAllBytes(filename, bytes);
        screenShot.Apply();
        Photo photo = ScriptableObject.CreateInstance<Photo>();
        photo._photo = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f), 100f);
        photo.name = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        //add score to the photo
        Collection.Instance.AddPhoto(photo);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        yield return null;
    }
}
