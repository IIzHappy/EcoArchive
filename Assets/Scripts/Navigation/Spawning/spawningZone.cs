using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spawningZone : MonoBehaviour
{
    [System.Serializable]
    public class SpawnType
    {
        public GameObject prefab;
        public int maxNum;
        public float spawnRadius;

    }

    public List<SpawnType> animalTypes;
    public float spawnInterval = 2f;

    public bool spawnZoneActive = false;
    public bool startZone = false;
    private Dictionary<SpawnType, List<GameObject>> activeAnimals = new Dictionary<SpawnType, List<GameObject>>();

    private void Start()
    {
        foreach (var species in animalTypes)
        {
            activeAnimals[species] = new List<GameObject>();
        }

        if (startZone)
        {
            spawnZoneActive = true;
            StartCoroutine(spawning());
        }

    }

   

    private void OnTriggerEnter(Collider entity)
    {
        if (entity.CompareTag("Player"))
        {
            Debug.Log("spawning" + gameObject.name);
            spawnZoneActive = true;
            StartCoroutine(spawning());
        }
    }

    private void OnTriggerExit(Collider thingy)
    {
        if (thingy.CompareTag("Player"))
        {
            Debug.Log("Despawning" + gameObject.name);
            spawnZoneActive = false;
             DespawnAnimals();
        }
    }

    IEnumerator spawning()
    {
        while (spawnZoneActive)
        {
            foreach (var species in animalTypes)
            {
                attemptSpawn(species);
            }
            yield return new WaitForSeconds(spawnInterval);
        }


    }

    void attemptSpawn(SpawnType species)
    {
        var newList = activeAnimals[species];
        if (newList.Count >= species.maxNum) return;
        Vector3 spawnPosition;
        if (getMeshPoint(species.spawnRadius, out spawnPosition)){
            GameObject animal = Instantiate(species.prefab, spawnPosition, Quaternion.identity, transform);
            newList.Add(animal);
        }
    }

    bool getMeshPoint (float radius, out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randPoint = transform.position + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randPoint, out hit, 5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    void DespawnAnimals()
    {
        foreach (var list in activeAnimals.Values)
        {
            foreach (var animal in list)
            {
                if (animal != null)
                {
                    Destroy(animal);
                }
                ;
            }
            list.Clear();
        }
    }
}

