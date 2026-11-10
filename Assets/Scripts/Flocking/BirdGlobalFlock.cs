using UnityEngine;

public class BirdGlobalFlock : MonoBehaviour
{
    [Header("Set Values")]
    public static int airSize = 30;
    public static float minHeight = 10f;
    static int numBirds = 700;

    [Header("Scene Values")]
    public static Vector3 goalPosition = new Vector3(0, 0, 0);
    public static GameObject[] birds = new GameObject[numBirds];

    [Header("Object References")]
    public GameObject birdPrefab;
    GameObject player;
    Vector3 playerPosition;

    void Start()
    {
        Init();
        SpawnBirds();
    }

    public void Init()
    {
        player = Player.Instance.gameObject;
        playerPosition = player.transform.position;
        goalPosition = RandomPositionInAir();
    }
    public void SpawnBirds()
    {
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
        return new Vector3(
            Random.Range(playerPosition.x - airSize, playerPosition.x + airSize),
            Random.Range(playerPosition.y + minHeight, playerPosition.y + airSize),
            Random.Range(playerPosition.x - airSize, playerPosition.z + airSize)
            );
    }
}
