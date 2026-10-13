using UnityEngine;

public class Dog : MonoBehaviour
{
    // Default facing +x axis 
    private DogState state;

    public float distanceBehindPlayer;
    public float rotationSpeed;
    public float speed;

    private float lastCommandTime = 0.0f;
    private float idleTimeUntilSleep = 10.0f;
    private void Awake()
    {
        UpdateState(DogState.FOLLOWING);
    }
    void Update()
    {
        CheckForCommands();

        switch (state)
        {
            case DogState.WANDERING:
                CheckIfShouldSleep();
                Wander();
                break;
            case DogState.FOLLOWING:
                CheckIfShouldSleep();
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
    public void UpdateState(DogState _state)
    {
        Debug.Log($"State changes from {state} to {_state}");
        state = _state;
    }
    public void FollowPlayer()
    { 
        // Move towards player if far enough away
        if (Utilities.CalculateDistanceToObject(gameObject, Player.Instance.transform.position) > distanceBehindPlayer)
        {
            // Rotate towards player

            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
                Utilities.CalculateAngleToObject(gameObject, Player.Instance.transform.position),
                rotationSpeed * Time.deltaTime);

            // Move towards player
            gameObject.transform.position += transform.forward * speed * Time.deltaTime;
        }
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
