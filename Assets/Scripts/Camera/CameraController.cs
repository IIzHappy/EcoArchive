using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera VFCam;

    [SerializeField] GameObject UI;
    [SerializeField] GameObject camUI;

    [SerializeField] VolumeProfile volumeProfile;
    DepthOfField dof;

    [SerializeField] RenderTexture viewFinder;

    [SerializeField] GameObject flashLight;

    public List<Texture2D> photos = new List<Texture2D>();
    public List<bool> save = new List<bool>();

    private bool camUsable = true;
    public bool advancedCam = false;

    LayerMask ignore;
    int animal;

    public float adjVal;

    public int photoNum = 0;
    private string filename;

    int resWidth, resHeight;

    public bool VF = false;
    public bool flash = false;

    private void Start()
    {
        ignore = ~LayerMask.GetMask("Lines", "Boxes");
        animal = LayerMask.NameToLayer("Animal");
        cam.usePhysicalProperties = true;
        DepthOfField test;

        resHeight = cam.pixelHeight; 
        resWidth = cam.pixelWidth;

        if (volumeProfile.TryGet<DepthOfField>(out test))
        {
            dof = test;
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Player Images/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Player Images/");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && camUsable)
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
                //if (Input.GetKey("c"))
                //{
                //    //Aperture
                //    dof.aperture.value += adjVal;
                //    if (dof.aperture.value > 32)
                //    {
                //        dof.aperture.value = 32;
                //    }
                //    else if (dof.aperture.value < 0.95f)
                //    {
                //        dof.aperture.value = 0.95f;
                //    }
                //}

                //else if (Input.GetKey("x"))
                //{
                //    //Shutter Speed
                //    cam.shutterSpeed += adjVal;
                //    if (cam.shutterSpeed > 0.1f)
                //    {
                //        cam.shutterSpeed = 0.1f;
                //    }
                //    VFCam.shutterSpeed = cam.shutterSpeed;
                //}

                //if (Input.GetKey("f"))
                //{
                //    //focal length
                //    dof.focalLength.value += adjVal;
                //    if (dof.focalLength.value > 70f)
                //    {
                //        dof.focalLength.value = 70;
                //    }
                //    else if (dof.focalLength.value < 5f)
                //    {
                //        dof.focalLength.value = 5;
                //    }
                //}

                if (Input.GetKey("f"))
                {
                    //focal distance
                    dof.focusDistance.value += adjVal / 5f;
                    if (dof.focusDistance.value > 1.8f)
                    {
                        dof.focusDistance.value = 1.8f;
                    }
                    else if (dof.focusDistance.value < 1.4f)
                    {
                        dof.focusDistance.value = 1.4f;
                    }
                }

                else if (Input.GetKeyDown("z"))
                {
                    flash = !flash;
                }

                else if (Input.GetKeyDown("x"))
                {
                    for (int i = 0; i < 240; i++)
                    {
                        for (int j = 0; j < 135; j++)
                        {
                            RaycastHit check;
                            Ray temp = cam.ScreenPointToRay(new Vector3(0 + (i * 8), 0 + (j * 8), 0));
                            Physics.Raycast(temp, out check, Mathf.Infinity, ignore);
                            if (check.collider != null)
                            {
                                if (check.collider.gameObject.layer == animal)
                                {
                                    dof.focusDistance.value = check.distance * 20f;
                                }
                            }
                        }
                    }
                }

                else
                {
                    cam.focalLength += adjVal * 10f;
                    if (cam.focalLength > 100f)
                    {
                        cam.focalLength = 100f;
                    }
                    else if (cam.focalLength < 20f)
                    {
                        cam.focalLength = 20;
                    }

                    VFCam.focalLength = cam.focalLength;
                }
            }
        }
        adjVal = 0;
    }

    private IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        VFCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        if (flash) flashLight.SetActive(true);
        VFCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        VFCam.targetTexture = viewFinder;
        Destroy(rt);
        yield return new WaitForEndOfFrame();
        //byte[] bytes = screenShot.EncodeToPNG();
        filename = string.Format(Application.persistentDataPath + "/Player Images/" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "." + photoNum + ".png");
        screenShot.name = filename;
        yield return new WaitForEndOfFrame();
        //System.IO.File.WriteAllBytes(filename, bytes);
        screenShot.Apply();
        photos.Add(screenShot);
        save.Add(true);
        Photo photo = ScriptableObject.CreateInstance<Photo>();
        photo._photo = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f), 100f);
        StartCoroutine(ScorePhoto(photo));
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        yield return new WaitForSeconds(0.5f);
        flashLight.SetActive(false);
        yield return null;
    }

    private IEnumerator ScorePhoto(Photo photo)
    {
        GameObject[] subjects = new GameObject[10];
        int subjectsNum = 0;
        float score = 0;
        float percent = 0;
        float act = 1;
        float frameMult = 1;
        float focus = 0.15f;
        LayerMask boxes = ~LayerMask.GetMask("Lines");
        LayerMask lines = ~LayerMask.GetMask("Boxes");

        for (int i = 0; i < 240; i++)
        {
            for (int j = 0; j < 135; j++)
            {
                RaycastHit check;
                Ray temp = cam.ScreenPointToRay(new Vector3(0 + (i*8), 0 + (j*8), 0));
                Physics.Raycast(temp, out check, Mathf.Infinity, ignore);
                //Debug.DrawRay(temp.origin, temp.direction, Color.red, 5f);
                if (check.collider != null)
                {
                    if (check.collider.gameObject.layer == animal)
                    {
                        percent++;
                        if (!subjects.Contains(check.collider.gameObject) && subjects.Contains(null))
                        {
                            subjects[subjectsNum] = check.collider.gameObject;
                            subjectsNum++;
                        }
                    }
                }
            }
            if (i%80 == 0)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        foreach(GameObject cur in subjects)
        {
            if (cur != null)
            {
                float tempAct = 1f;
                AnimalNavBase navBase;
                cur.TryGetComponent<AnimalNavBase>(out navBase);
                //cur.GetComponent(typeof(AnimalNavBase)) as AnimalNavBase;
                if (navBase != null)
                {
                    switch (navBase.CurrentState)
                    {
                        case AnimalNavBase.AnimalState.Idle:
                            tempAct = 1;
                            break;

                        case AnimalNavBase.AnimalState.Roaming:
                            tempAct = .95f;
                            break;

                        case AnimalNavBase.AnimalState.Chasing:
                            tempAct = .75f;
                            break;

                        case AnimalNavBase.AnimalState.Fleeing:
                            tempAct = .75f;
                            break;

                        case AnimalNavBase.AnimalState.Unique:
                            tempAct = .5f;
                            break;

                        case AnimalNavBase.AnimalState.Resting:
                            tempAct = .6f;
                            break;

                        default:
                            tempAct = 1f;
                            break;
                    }
                    if (tempAct < act)
                    {
                        act = tempAct;
                    }

                    RaycastHit hitChest;
                    RaycastHit hitHind;
                    RaycastHit hitHeadL;
                    RaycastHit hitHeadB;
                    Ray frameChest = new Ray(cur.transform.Find("Chest").transform.position, (cam.transform.position - cur.transform.Find("Chest").transform.position));
                    Ray frameHind = new Ray(cur.transform.Find("Hind").transform.position, (cam.transform.position - cur.transform.Find("Hind").transform.position).normalized);
                    Ray frameHead = new Ray(cur.transform.Find("Head").transform.position, (cam.transform.position - cur.transform.Find("Head").transform.position).normalized);
                    GameObject thirdChest = null;
                    GameObject thirdHind = null;
                    GameObject thirdHeadL = null;
                    GameObject thirdHeadB = null;
                    if (Physics.Raycast(frameChest, out hitChest, Mathf.Infinity, lines))
                        thirdChest = hitChest.collider.gameObject;
                    if (Physics.Raycast(frameHind, out hitHind, Mathf.Infinity, lines))
                        thirdHind = hitHind.collider.gameObject;
                    if (Physics.Raycast(frameHead, out hitHeadL, Mathf.Infinity, lines))
                        thirdHeadL = hitHeadL.collider.gameObject;
                    if (Physics.Raycast(frameHead, out hitHeadB, Mathf.Infinity, boxes))
                        thirdHeadB = hitHeadB.collider.gameObject;
                    if (thirdChest != null && thirdHind != null && thirdHeadL != null && thirdHeadB != null)
                    {
                        if (thirdChest.name == "R 3 Line" || thirdChest.name == "L 3 Line")
                        {
                            if (thirdHeadB.name == "C 3 Line")
                            {
                                if (frameMult < 2.5f) frameMult = 2.5f;
                            }
                        }

                        if (thirdChest.name == "R 3 Line")
                        {
                            if (thirdHind.name == "L 3 Line")
                            {
                                if (frameMult < 2f) frameMult = 2f;
                            }
                        }
                        else if (thirdChest.name == "L 3 Line")
                        {
                            if (thirdHind.name == "R 3 Line")
                            {
                                if (frameMult < 2f) frameMult = 2f;
                            }
                        }

                        if (thirdChest.name == "R 3 Line" && thirdHind.name == "R 3 Line" && thirdHeadL.name == "R 3 Line")
                        {
                            if (frameMult < 3f) frameMult = 3f;
                        }
                        else if (thirdChest.name == "R 3 Line" && thirdHind.name == "R 3 Line" && thirdHeadL.name == "R 3 Line")
                        {
                            if (frameMult < 3f) frameMult = 3f;
                        }
                    }

                    Debug.Log(hitChest.collider.gameObject.name);
                    Debug.Log(hitChest.distance + " + " + dof.focusDistance);
                    if (hitChest.distance <= 20f * dof.focusDistance.GetValue<float>() && focus != 1 && (hitChest.collider.gameObject.layer == LayerMask.NameToLayer("Lines") || hitChest.collider.gameObject.layer == LayerMask.NameToLayer("Boxes")))
                    {
                        focus = 1;
                        Debug.Log(navBase.AnimalID);
                        Collection.Instance.FoundAnimal(navBase.AnimalID);
                    }
                }
            }
        }

        float diff;
        percent = percent / 324;
        if (Mathf.Abs(percent - 70) < Mathf.Abs(percent - 40))
        {
            diff = Mathf.Abs(percent - 70f);
            Debug.Log(diff + " Closer 70");
        }
        else
        {
            diff = Mathf.Abs(percent - 40);
            Debug.Log(diff + " Closer 40");
        }

        //Final score application
        Debug.Log("(1000 * " + frameMult + " * " + focus + ") - (" + diff + " * 25 * " + act + " * " + frameMult + " * " + focus + ")");
        score = ((1000 * frameMult * focus) - (diff * 25 * act * frameMult * focus));
        photo._score = score; 
        photo.name = (photo._score.ToString() + " pts.");
        LoadablePhoto loadablePhoto = new LoadablePhoto(photo.name, filename, photo._score);
        Collection.Instance.AddLoadablePhoto(loadablePhoto);
        Collection.Instance.AddPhoto(photo);
        Debug.Log(photo.name);
    }

    public void SavePhotos()
    {
        for (int i = 0; i < photos.Count(); i++)
        {
            if (save[i])
            {
                byte[] bytes = photos[i].EncodeToPNG();
                System.IO.File.WriteAllBytes(photos[i].name, bytes);
            }
        }
        photos.Clear();
        save.Clear();
    }

    public void SetCamUseable(bool allow)
    {
        camUsable = allow;
    }
}
