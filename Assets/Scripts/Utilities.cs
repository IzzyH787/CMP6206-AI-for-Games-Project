using UnityEngine;

public class Utilities: MonoBehaviour
{
    public static Quaternion CalculateAngleToObject(GameObject _self, Vector3 _pos)
    {
        //return Vector3.SignedAngle(gameObject.transform.forward, CalculateDirectionToPlayer(), Vector3.up);
        return Quaternion.LookRotation(CalculateDirectionToObject(_self, _pos));
    }
    public static float CalculateDistanceToPosition(GameObject _self, Vector3 _pos)
    {
        return Vector3.Distance(_self.transform.position, _pos);
    }
    public static float CalculateDistanceToObject(GameObject _self, GameObject _obj)
    {
        return CalculateDistanceToPosition(_self, _obj.transform.position);
    }
    public static Vector3 CalculateDirectionToObject(GameObject _self, Vector3 _pos)
    {
        return (_pos - _self.transform.position).normalized;
    }
    public static Vector3 RandomPosition()
    {
        Vector3 minBound = WorldBounds.Instance.MinBound.position;
        Vector3 maxBound = WorldBounds.Instance.MaxBound.position;

        return new Vector3(
            Random.Range(minBound.x, maxBound.x),
            Random.Range(minBound.y, maxBound.y),
            Random.Range(minBound.z, maxBound.z));
    }
    public static Node RandomWalkableNode()
    {
        if (Grid.Instance.walkableNodes == null || Grid.Instance.walkableNodes.Count == 0) 
            return null;

        return Grid.Instance.walkableNodes[Random.Range(0, Grid.Instance.walkableNodes.Count)];
    }

    public static Vector3 RandomWalkablePosition()
    {
        Node randomNode = RandomWalkableNode(); 
        if (randomNode == null)
            return Vector3.zero;

        return randomNode.worldPosition;
    }
}
