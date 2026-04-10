using UnityEngine;

public class MothSpawner : MonoBehaviour
{
    public GameObject moth1;
    public GameObject butterfly1;
    public GameObject butterfly2;
    public GameObject butterfly3;
    GameObject spawnedBug;

    [SerializeField] float thingToSpawn = 0f;

    private void Start()
    {
        if (thingToSpawn == 0f)
        {
            spawnedBug = moth1;
        }
        else if (thingToSpawn == 1f)
        {
            spawnedBug = butterfly1;
        }
        else if (thingToSpawn == 2f)
        {
            spawnedBug = butterfly2;
        }
        else
        {
            spawnedBug = butterfly3;
        }
        if (spawnedBug == null)
        {
            Debug.Log("Getfuckedloser");
        }
        else
        {
            Instantiate(spawnedBug, transform.position, transform.rotation);
            Debug.Log("bug spawned");
        }
    }
}
