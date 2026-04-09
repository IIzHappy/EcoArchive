using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Terrain))]
public class DetailCarve : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    private void Reset()
    {
        terrain = GetComponent<Terrain>();

        Extract();
    }

    [ContextMenu("Extract")]
    public void Extract()
    {
        Bounds test;
        Collider[] colliders = terrain.GetComponentsInChildren<Collider>();

        for (int i = 1; i < colliders.Length; i++)
        {
            DestroyImmediate(colliders[i].gameObject);
        }

        for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
        {
            TreePrototype tree = terrain.terrainData.treePrototypes[i];

            TreeInstance[] instances = terrain.terrainData.treeInstances.Where(x => x.prototypeIndex == i).ToArray();

            for (int j = 0; j < instances.Length; j++)
            {
                instances[j].position = Vector3.Scale(instances[j].position, terrain.terrainData.size);
                instances[j].position += terrain.GetPosition();

                CapsuleCollider prefabCollider = tree.prefab.GetComponent<CapsuleCollider>();
                if (!prefabCollider) continue;

                GameObject obj = new GameObject();
                obj.name = tree.prefab.name + j;

                CapsuleCollider objCollider = obj.AddComponent<CapsuleCollider>();

                objCollider.center = prefabCollider.center;
                objCollider.height = prefabCollider.height;
                objCollider.radius = prefabCollider.radius;

                obj.layer = terrain.gameObject.layer;

                obj.transform.position = instances[j].position;
                obj.transform.parent = terrain.transform;
            }
        }

        for (int l = 0; l < terrain.terrainData.detailPrototypes.Length; l++)
        {
            DetailPrototype detail = terrain.terrainData.detailPrototypes[l];

            for (int j = 0; j < terrain.terrainData.detailPatchCount; j++)
            {
                for (int k = 0; k < terrain.terrainData.detailPatchCount; k++)
                {
                    DetailInstanceTransform[] instances = terrain.terrainData.ComputeDetailInstanceTransforms(j, k, l, 1, out test);
                    for (int i = 0; i < instances.Length; i++)
                    {
                        Vector3 position = new Vector3(instances[i].posX, instances[i].posY, instances[i].posZ);
                        position += terrain.GetPosition();

                        CapsuleCollider prefabCollider = detail.prototype.GetComponent<CapsuleCollider>();
                        if (!prefabCollider) continue;

                        GameObject obj = new GameObject();
                        obj.name = detail.prototype.name + j + k;
                        obj.transform.rotation = Quaternion.Euler(0, instances[i].rotationY * Mathf.Rad2Deg, 0);
                        Vector3 scale = new Vector3(instances[i].scaleXZ, instances[i].scaleY, instances[i].scaleXZ);
                        obj.transform.localScale = scale;

                        CapsuleCollider objCollider = obj.AddComponent<CapsuleCollider>();

                        objCollider.center = prefabCollider.center;
                        objCollider.height = prefabCollider.height;
                        objCollider.radius = prefabCollider.radius;
                        objCollider.direction = prefabCollider.direction;

                        NavMeshObstacle obstacle = obj.AddComponent<NavMeshObstacle>();
                        obstacle.shape = NavMeshObstacleShape.Box;
                        obstacle.center = prefabCollider.center;
                        switch (objCollider.direction)
                        {
                            case 0:
                                obstacle.size = new Vector3(prefabCollider.height, prefabCollider.radius * 2, prefabCollider.radius * 2);
                                break;
                            case 1:
                                obstacle.size = new Vector3(prefabCollider.radius * 2, prefabCollider.height, prefabCollider.radius * 2);
                                break;
                            case 2:
                                obstacle.size = new Vector3(prefabCollider.radius * 2, prefabCollider.radius * 2, prefabCollider.height);
                                break;
                            default:
                                Debug.Log("No Direction");
                                break;
                        }
                        obstacle.carving = true;

                        obj.layer = terrain.gameObject.layer;

                        obj.transform.position = position;
                        obj.transform.parent = terrain.transform;
                    }
                }
            }
        }
    }
}