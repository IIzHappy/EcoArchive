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
        float nightStarted = 1200f;
        float nightEnd = 420f;
        return (time >= nightStarted ||  time <= nightEnd);
    }

    void spawnRoll()
    {
        if (hasSpawned) return;
        if (Random.value <= spawnChance)
        {
            if (Owl == null)
            {
                Debug.Log("aint no owls here fuclker");
            }
            spawnedOwl = Instantiate(Owl, transform.position, transform.rotation);
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
