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

    public Vector3 CentreOfFlock()
    {
        Vector3 centre = Vector3.zero;
        int groupSize = 0;
        foreach (GameObject _sheep in sheepInScene)
        {
            centre += _sheep.transform.position;
            groupSize++;
        }
        if (groupSize == 0)
            return Vector3.zero;

        centre /= groupSize;
        return centre;
    }
}
