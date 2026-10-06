using UnityEngine;

public class Dog : MonoBehaviour
{
    // Default facing +x axis 

    public float distanceBehindPlayer;
    public float rotationSpeed;
    public float speed;
    void Update()
    {
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
}
