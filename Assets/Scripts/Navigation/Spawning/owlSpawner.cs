using UnityEngine;

public class owlSpawner : MonoBehaviour
{
    public DayNightCycle cycle;
    public GameObject Owl;
    [Range(0f, 1f)] public float spawnChance = 0.5f;
    private bool hasSpawned = false;
    private GameObject spawnedOwl;

    private void Start()
    {
        if (cycle == null)
        {
            cycle = FindFirstObjectByType<DayNightCycle>();
        }
    }

    private void Update()
    {
        if (cycle == null) return;
        if (IsNight())
        {
            spawnRoll();
        }else
        {
            Despawn();
        }
    }

    bool IsNight()
    {
        float time = cycle._time;
        float nightStarted = 1080f;
        float nightEnd = 360f;
        return (time >= nightStarted ||  time <= nightEnd);
    }

    void spawnRoll()
    {
        if (hasSpawned) return;
        if (Random.value <= spawnChance)
        {
            spawnedOwl = Instantiate(spawnedOwl, transform.position, transform.rotation);
            Debug.Log("Owl spawned");
        }
        hasSpawned = true;
    }

    void Despawn()
    {
        hasSpawned = false;
        if (spawnedOwl != null)
        {
            Destroy(spawnedOwl);
            spawnedOwl = null;
            Debug.Log("killedowl lol");
        }
        }
}
