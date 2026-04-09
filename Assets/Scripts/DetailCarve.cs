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
        Debug.Log(terrain.terrainData.detailPatchCount);
        Bounds test;
        Collider[] colliders = terrain.GetComponentsInChildren<Collider>();

        //Skip the first, since its the Terrain Collider
        for (int i = 1; i < colliders.Length; i++)
        {
            //Delete all previously created colliders first
            DestroyImmediate(colliders[i].gameObject);
        }

        //for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
        //{
            DetailPrototype detail = terrain.terrainData.detailPrototypes[2];

            //Get all instances matching the prefab index
            //DetailInstanceTransform[] instances = terrain.terrainData.ComputeDetailInstanceTransforms();
            //treeInstances.Where(x => x.prototypeIndex == i).ToArray();

            for (int j = 0; j < 32; j++)
            {
            for (int k = 0; k < 32; k++)
            {

                DetailInstanceTransform[] instances = terrain.terrainData.ComputeDetailInstanceTransforms(j, k, 2, 1, out test);
                //Un-normalize positions so they're in world-space
                for (int i = 0; i < instances.Length; i++)
                {
                    Vector3 position = new Vector3(instances[i].posX, instances[i].posY, instances[i].posZ);
                    //position = Vector3.Scale(position, terrain.terrainData.size);
                    position += terrain.GetPosition();

                    //Fetch the collider from the prefab object parent
                    CapsuleCollider prefabCollider = detail.prototype.GetComponent<CapsuleCollider>();

                    if (!prefabCollider) continue;

                    GameObject obj = new GameObject();
                    obj.name = detail.prototype.name + j + k;
                    obj.transform.rotation = Quaternion.Euler(0, instances[i].rotationY * Mathf.Rad2Deg, 0);
                    Vector3 scale = new Vector3(instances[i].scaleXZ, instances[i].scaleY, instances[i].scaleXZ);
                    obj.transform.localScale = scale;
                    //obj.transform.rotation.SetEulerAngles(0, instances[i].rotationY, 0);
                    //obj.transform.rotation.SetAxisAngle(Vector3.up, instances[i].rotationY * Mathf.Rad2Deg);

                    CapsuleCollider objCollider = obj.AddComponent<CapsuleCollider>();

                    objCollider.center = prefabCollider.center;
                    objCollider.height = prefabCollider.height;
                    objCollider.radius = prefabCollider.radius;
                    objCollider.direction = prefabCollider.direction;

                    NavMeshObstacle obstacle = obj.AddComponent<NavMeshObstacle>();
                    obstacle.shape = NavMeshObstacleShape.Box;
                    obstacle.center = prefabCollider.center;
                    obstacle.size = new Vector3 (prefabCollider.radius * 2, prefabCollider.radius * 2 , prefabCollider.height);
                    obstacle.carving = true;


                    //if (terrain.preserveTreePrototypeLayers) obj.layer = detail.prototype.layer;
                    obj.layer = terrain.gameObject.layer;

                    obj.transform.position = position;
                    obj.transform.parent = terrain.transform;
                }
            }
            }
        //}
    }
}