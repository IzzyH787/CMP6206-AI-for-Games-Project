using UnityEngine;

public class SheepFlockManager : MonoBehaviour
{
    [Header("Set Values")]

    static int numSheep = 700;
    public Transform minBounds;
    public Transform maxBounds;

    [Header("Scene Values")]
    public static GameObject[] sheep;

    [Header("Object References")]
    public GameObject sheepPrefab;


    void Start()
    {
        SpawnSheep();
    }

    public void SpawnSheep()
    {
        sheep = new GameObject[numSheep];

        for (int i = 0; i < numSheep; i++)
        {

            // Spawn bird
            sheep[i] = (GameObject)Instantiate(
                sheepPrefab,
                RandomPositionInField(),
                Quaternion.LookRotation(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))));
        }
    }

    public Vector3 RandomPositionInField()
    {
        return new Vector3(
            Random.Range(minBounds.position.x, maxBounds.position.x),
            Random.Range(minBounds.position.y, maxBounds.position.y),
            Random.Range(minBounds.position.z, maxBounds.position.z)
            );
    }
}
