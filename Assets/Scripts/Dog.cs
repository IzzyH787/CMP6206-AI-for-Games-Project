using UnityEngine;

public class Dog : MonoBehaviour
{
    // Default facing +x axis 
    private DogState state;

    public float distanceBehindPlayer;
    public float rotationSpeed;
    public float speed;

    private float lastCommandTime = 0.0f;
    private float idleTimeUntilSleep = 1000.0f;

    Vector3 wanderDestination;
    public float distanceToDestinationThreshold;


    // Used for pathfinding when following player

    /// <summary> Stores when player was last in line of sight </summary>
    private float lastTimePlayerSeen;

    /// <summary> Stores node player was in when last in line of sight </summary>
    private Node lastNodePlayerSeen;

    /// <summary> Stores time player last performed here command </summary>
    private float lastHereCommandTime;

    /// <summary> Stores node player was in when last in line of sight </summary>
    private Node lastHereCallNode;
    private void Awake()
    {
        UpdateState(DogState.WANDERING);
    }
    void Update()
    {
        CheckForCommands();
        UpdateIfCanSeePlayer();

        switch (state)
        {
            case DogState.WANDERING:
                //CheckIfShouldSleep();
                Wander();
                break;
            case DogState.FOLLOWING:
                //CheckIfShouldSleep();
                FollowPlayer();
                break;
            case DogState.SLEEPING:
                Sleep();
                break;
            case DogState.SIT:
                CheckIfShouldSleep();
                Sit();
                break;
            case DogState.WALK_UP:
                WalkUp();
                break;
            case DogState.COME_BY:
                ComeBy();
                break;
            case DogState.AWAY:
                Away();
                break;
            default:
                break;
        }

    }

    private void UpdateIfCanSeePlayer()
    {
        if (!CanSeePlayer())
            return;

        lastTimePlayerSeen = Time.time;
        lastNodePlayerSeen = Grid.Instance.NodeFromGameObject(Player.Instance.gameObject);
    }
    private bool CanSeePlayer()
    {
        return gameObject.GetComponent<AISensor>().IsInSight(Player.Instance.gameObject);
    }

    public void UpdateState(DogState _state)
    {
        Debug.Log($"State changes from {state} to {_state}");
        state = _state;
    }
    public void FollowPlayer()
    {
        Debug.Log("Following player");
        // Check if player is currently in line of sight
        if (gameObject.GetComponent<AISensor>().IsInSight(Player.Instance.gameObject, false))
        {
            Debug.Log("Can see player");
        }
        else
            Debug.Log("Cant see player");


        /*// Move towards player if far enough away
        if (Utilities.CalculateDistanceToObject(gameObject, Player.Instance.transform.position) > distanceBehindPlayer)
        {
            // Rotate towards player

            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
                Utilities.CalculateAngleToObject(gameObject, Player.Instance.transform.position),
                rotationSpeed * Time.deltaTime);

            // Move towards player
            gameObject.transform.position += transform.forward * speed * Time.deltaTime;
        }*/
    }

    public void Follow(Vector3 _position)
    {
        // Rotate towards position
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
            Quaternion.LookRotation(_position - transform.position),
            rotationSpeed * Time.deltaTime);

        // Move towards player
        gameObject.transform.position += transform.forward * speed * Time.deltaTime;
    }
    public void Wander()
    {
        // Check if new point need to be picked
        if (wanderDestination == null || Utilities.CalculateDistanceToObject(gameObject, wanderDestination) < distanceToDestinationThreshold)
        {
            wanderDestination = PickRandomPoint();
        }
        Follow(wanderDestination);
    }
    public Vector3 PickRandomPoint()
    {
        Vector3 minBound = WorldBounds.Instance.MinBound.position;
        Vector3 maxBound = WorldBounds.Instance.MaxBound.position;

        return new Vector3(
            Random.Range(minBound.x, maxBound.x),
            Random.Range(minBound.y, maxBound.y),
            Random.Range(minBound.z, maxBound.z));
    }

    public void Sleep()
    {
        // TO DO: Implement sleep behaviour
    }
    public void Sit()
    {
        // TO DO: Implement sleep behaviour
    }
    public void WalkUp()
    {
        // TO DO: Implement walk up behaviour
    }
    public void ComeBy()
    {
        // TO DO: Implement come by behaviour
    }
    public void Away()
    {
        // TO DO: Implement away behaviour
    }
    public void UpdateCommandTime()
    {
        lastCommandTime = Time.time;
    }
    public void CheckIfShouldSleep()
    {
        if (Time.time - lastCommandTime > idleTimeUntilSleep)
        {
            UpdateState(DogState.SLEEPING);
        }
    }
    public void CheckForCommands()
    {
        //Debug.Log($"Sit: {PlayerCommands.SitCommandGiven}, Walk: {PlayerCommands.WalkUpCommandGiven}, Come By: {PlayerCommands.ComeByCommandGiven}, Away: {PlayerCommands.AwayCommandGiven}");
        if (PlayerCommands.SitCommandGiven)
            OnSit();
        else if (PlayerCommands.WalkUpCommandGiven)
            OnWalkUp();
        else if (PlayerCommands.ComeByCommandGiven)
            OnComeBy();
        else if (PlayerCommands.AwayCommandGiven)
            OnAway();
        else if (PlayerCommands.HereCommandGiven)
            OnHere();
    }
    public void OnComeBy()
    {
        UpdateCommandTime();
        UpdateState(DogState.COME_BY);
    }
    public void OnAway()
    {
        UpdateCommandTime();
        UpdateState(DogState.AWAY);
    }
    public void OnWalkUp()
    {
        UpdateCommandTime();
        UpdateState(DogState.WALK_UP);
    }
    public void OnSit()
    {
        UpdateCommandTime();
        UpdateState(DogState.SIT);
    }
    public void OnHere()
    {
        lastHereCommandTime = Time.time;
        lastHereCallNode = Grid.Instance.NodeFromGameObject(Player.Instance.gameObject);

        UpdateCommandTime();
        UpdateState(DogState.FOLLOWING);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wanderDestination, 0.7f);
    }
}

public enum DogState
{
    WANDERING,
    FOLLOWING,
    SLEEPING,
    SIT,
    WALK_UP,
    COME_BY,
    AWAY,
    NUM_OF_STATES
}
