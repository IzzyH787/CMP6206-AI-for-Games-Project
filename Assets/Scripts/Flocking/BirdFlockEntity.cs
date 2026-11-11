using UnityEngine;

public class BirdFlockEntity : MonoBehaviour
{
    [Header("Personal Values")]
    private float _speed;
    public float speed {
        get { return _speed; }
        set
        {
            _speed = Mathf.Clamp(value, minSpeed, maxSpeed);
        }
    }
    public float rotatonSpeed = 30f;
    bool turning = true;

    [Header("Set Values")]
    float neighbourDistance = 10f;
    float distanceAwayWithoutSeparation = 6f;

    public float minSpeed = 3f;
    public float maxSpeed = 10f;
    public float speedVariation = 0.5f;

    private void Start()
    {
        Init();

    }
    public void Init()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    private void Update()
    {
        // Perform 20% of the time
        if (Utilities.GetSuccessInPercentChance(20))
        {


            GameObject[] birdsInScene = BirdGlobalFlock.birds;

            Vector3 vCentre = gameObject.transform.position; // Cohesion
            Vector3 vAvoid = Player.Instance.transform.position; // Separation

            float groupSpeed = 0f;
            Vector3 goalPosition = BirdGlobalFlock.goalPosition;

            float distance;
            int groupSize = 0;

            // Iterate through all birds in scene
            foreach (GameObject _bird in birdsInScene)
            {
                // Ignore itself in array
                if (_bird == gameObject)
                    continue;

                // Get dist between this brid and other bird
                distance = Vector3.Distance(_bird.transform.position, gameObject.transform.position);

                //Check if other _bird is within neighbouring range of this brid
                if (distance <= neighbourDistance)
                {
                    vCentre += _bird.transform.position; // Add positions together to get centroid position between all birds
                    groupSize++;

                    // If close enough that needs to be separated
                    if (distance < distanceAwayWithoutSeparation)
                    {
                        vAvoid += (gameObject.transform.position - _bird.transform.position); // Separation
                    }
                    // Get ref to other bird's entity script
                    BirdFlockEntity otherEntity = _bird.GetComponent<BirdFlockEntity>();
                    groupSpeed += otherEntity.speed;
                }
            }

            // Group size has got how many other birds within neighbouring radius of this bird
            if (groupSize > 0)
            {
                vCentre = vCentre / groupSize + (goalPosition - gameObject.transform.position); // Cohesion force being applied
                speed = (groupSpeed/groupSize) + Random.Range(-speedVariation, speedVariation);

                Vector3 direction = (vCentre + vAvoid) - gameObject.transform.position; // Alignment
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotatonSpeed * Time.deltaTime);
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    public bool ShouldTurn()
    {
        // Turning true if bird too far from player OR too low in sky
        turning = (Vector3.Distance(transform.position, Player.Instance.transform.position) >= BirdGlobalFlock.airSize ||
            transform.position.y <= BirdGlobalFlock.minHeight);

        return turning;
    }
    public void Turn()
    {
        if (ShouldTurn())
        {
            Vector3 direction = (Player.Instance.transform.position + new Vector3(0, Random.Range(BirdGlobalFlock.minHeight, BirdGlobalFlock.airSize))) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotatonSpeed * Time.deltaTime);
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
