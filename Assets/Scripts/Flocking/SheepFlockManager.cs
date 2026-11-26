using UnityEngine;

public class SheepFlockManager : MonoBehaviour
{
    public static SheepFlockManager Instance; 

    [Header("Set Values")]

    static int numSheep = 46;
    public Transform minBounds;
    public Transform maxBounds;

    [Header("Scene Values")]
    public GameObject[] sheepInScene;

    [Header("Object References")]
    public GameObject sheepPrefab;
    public GameObject predator; // TO DO: Add support for multiple predators in scene

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SpawnSheep();
    }

    public void SpawnSheep()
    {
        sheepInScene = new GameObject[numSheep];

        for (int i = 0; i < numSheep; i++)
        {

            // Spawn sheep
            sheepInScene[i] = (GameObject)Instantiate(
                sheepPrefab,
                RandomPositionInField(),
                Quaternion.LookRotation(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))));
        }
    }

    public Vector3 RandomPositionInField()
    {
        return new Vector3(
            Random.Range(minBounds.position.x, maxBounds.position.x),
            0,
            Random.Range(minBounds.position.z, maxBounds.position.z)
            );
    }
}
