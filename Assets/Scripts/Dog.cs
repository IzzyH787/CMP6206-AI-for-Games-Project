using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Dog : MonoBehaviour
{
    // Default facing +x axis 
    private DogState state;
    Pathfinding pathfindingComponent;

    [SerializeField] private float distanceBehindPlayer = 2.0f;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float speed = 3.0f;

    private float lastCommandTime = 0.0f;
    [SerializeField] private float commandDuration = 30.0f;
    [SerializeField] private float idleTimeUntilSleep = 1000.0f;

    Vector3 wanderDestination;
    [SerializeField] public float distanceToDestinationThreshold = 10.0f;


    // Used for pathfinding when following player

    /// <summary> Stores when player was last in line of sight </summary>
    private float lastTimePlayerSeen;

    /// <summary> Stores node player was in when last in line of sight </summary>
    private Node lastNodePlayerSeen;

    /// <summary> Stores time player last performed here command </summary>
    private float lastHereCommandTime;

    /// <summary> Stores node player was in when last in line of sight </summary>
    private Node lastHereCallNode;

    bool isConfused;
    [SerializeField] float timeConfusedUntilWander = 3.0f;
    float timeGotConfused;

    private void Awake()
    {
        pathfindingComponent = GetComponent<Pathfinding>();
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
                CheckIfShouldIdle();
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
        return gameObject.GetComponent<AISensor>().IsInSight(Player.Instance.gameObject, false);
    }

    public void UpdateState(DogState _state)
    {
        Debug.Log($"State changes from {state} to {_state}");
        state = _state;
    }
    public void FollowPlayer()
    {
        // Check if dog is already close enough to player
        if (Utilities.CalculateDistanceToObject(gameObject, Player.Instance.gameObject) < distanceBehindPlayer)
            return;

        // Check if player is currently in line of sight
        if (gameObject.GetComponent<AISensor>().IsInSight(Player.Instance.gameObject, false))
        {
            Debug.Log("Can see player");
            pathfindingComponent.FindPathFromSelfToObject(Player.Instance.gameObject);
        }
        else
        {
            Debug.Log("Cant see player");
            // Check if dog has seen player since command was given
            if (lastHereCommandTime > lastTimePlayerSeen)
            {
                // If most recent was command, follow sound (location of player when command given)
                pathfindingComponent.FindPathFromSelfToPosition(lastHereCallNode.worldPosition);
            }
            // If most recent was seeing player, follow last seen player location
            else
            {
                // If already at last time seeing player, enter conufsed state
                if (Grid.Instance.NodeFromGameObject(gameObject) == lastNodePlayerSeen && !isConfused)
                {
                    isConfused = true;
                    timeGotConfused = Time.time;
                }

                // If has been consufed for long enough return to wander state
                if(Time.time - timeGotConfused >= timeConfusedUntilWander)
                    UpdateState(DogState.WANDERING);

                pathfindingComponent.FindPathFromSelfToPosition(lastNodePlayerSeen.worldPosition);
            }
        }

        Pathfind();
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
        if (wanderDestination == null || Utilities.CalculateDistanceToPosition(gameObject, wanderDestination) < distanceToDestinationThreshold)
        {
            wanderDestination = Utilities.RandomWalkablePosition();
        }

        // A* to position
        pathfindingComponent.FindPathFromSelfToPosition(wanderDestination);
        Pathfind();
    }

    public void Pathfind()
    {
        // Check path has been found
        if (pathfindingComponent.currentPathToFollow.Count <= 0)
            return; // No path found so skip tracking this frame

        // Check if dog is currently on next node in path if so remove node
        if (Grid.Instance.NodeFromGameObject(gameObject) == pathfindingComponent.currentPathToFollow[0])
        {
            pathfindingComponent.currentPathToFollow.RemoveAt(0);
        }

        // Navigate towrds next node
        Follow(pathfindingComponent.currentPathToFollow[0].worldPosition);
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

    public void CheckIfShouldIdle() 
    { 
        if (Time.time - lastCommandTime > commandDuration)
        {
            UpdateState(DogState.WANDERING);
        }
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

        if (Grid.Instance == null || pathfindingComponent.currentPathToFollow == null || pathfindingComponent.currentPathToFollow.Count <= 0)
            return;

        foreach (Node node in pathfindingComponent.currentPathToFollow)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(node.worldPosition, Vector3.one * (Grid.Instance.nodeDiameter - 0.1f)); // Draws cubes for each node in grid (-.1 to give small gap in gizmos drawn
        }
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
