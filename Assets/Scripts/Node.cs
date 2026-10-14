using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector3 worldPosition;

    public Node(bool _isWalkable, Vector3 _pos)
    {
        isWalkable = _isWalkable;
        worldPosition = _pos;
    }
}
