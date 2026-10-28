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
}
