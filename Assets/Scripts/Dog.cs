using UnityEngine;

public class Dog : MonoBehaviour
{
    // Default facing +x axis 
    private DogState state;

    public float distanceBehindPlayer;
    public float rotationSpeed;
    public float speed;
    void Update()
    {
        switch (state)
        {
            case DogState.WANDERING:
                Wander();
                break;
            case DogState.FOLLOWING:
                FollowPlayer();
                break;
            case DogState.SLEEPING:
                Sleep();
                break;
            case DogState.SIT:
                Sit();
                break;
            case DogState.WALK_UP:
                break;
            case DogState.COME_BY:
                break;
            case DogState.AWAY:
                break;
            case DogState.NUM_OF_STATES:
                break;
            default:
                break;
        }
        FollowPlayer();
    }

    public Quaternion CalculateAngleToPlayer()
    {
        //return Vector3.SignedAngle(gameObject.transform.forward, CalculateDirectionToPlayer(), Vector3.up);
        return Quaternion.LookRotation(CalculateDirectionToPlayer());
    }
    public float CalculateDistanceToPlayer()
    {
        return Vector3.Distance(gameObject.transform.position, Player.Instance.gameObject.transform.position);
    }
    public Vector3 CalculateDirectionToPlayer()
    {
        return (Player.Instance.gameObject.transform.position - gameObject.transform.position).normalized;
    }

    public void FollowPlayer()
    { 
        // Move towards player if far enough away
        if (CalculateDistanceToPlayer() > distanceBehindPlayer)
        {
            // Rotate towards player

            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
                CalculateAngleToPlayer(),
                rotationSpeed * Time.deltaTime);

            // Move towards player
            gameObject.transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    public void Wander()
    {
        // TO DO: Implement wandering behaviour: Pick random point move to it & repeat
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
