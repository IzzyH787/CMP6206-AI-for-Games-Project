using UnityEngine;

public class BirdGlobalFlock : MonoBehaviour
{
    [Header("Set Values")]
    public static int airSize = 30;
    public static float minHeight = 10f;
    static int numBirds = 700;

    [Header("Scene Values")]
    public static Vector3 goalPosition = Vector3.zero;
    public static GameObject[] birds;

    [Header("Object References")]
    public GameObject birdPrefab;


    void Start()
    {
        Init();
        SpawnBirds();
    }

    public void Init()
    {
        goalPosition = RandomPositionInAir();
    }
    public void SpawnBirds()
    {
        birds = new GameObject[numBirds];

        for (int i = 0; i < numBirds; i++)
        {

            // Spawn bird
            birds[i] = (GameObject)Instantiate(
                birdPrefab,
                RandomPositionInAir(),
                Quaternion.LookRotation(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))));
        }
    }

    private void Update()
    {
        //  1% chance to be performed
        if(Utilities.GetSuccessInPercentChance(1))
        {
            goalPosition = RandomPositionInAir();
        }
    }

    public Vector3 RandomPositionInAir()
    {
        Vector3 playerPosition = Player.Instance.gameObject.transform.position;
        return new Vector3(
            Random.Range(playerPosition.x - airSize, playerPosition.x + airSize),
            Random.Range(playerPosition.y + minHeight, playerPosition.y + airSize),
            Random.Range(playerPosition.z - airSize, playerPosition.z + airSize)
            );
    }
}
