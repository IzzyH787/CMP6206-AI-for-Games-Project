using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public Node parent;

    public int gridX, gridY;

    /// <summary>
    /// Cost to get from start position to this node
    /// </summary>
    public int gCost;
    /// <summary>
    /// Distance from this node to target node
    /// </summary>
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _isWalkable, Vector3 _pos, int _gridX, int _gridY)
    {
        isWalkable = _isWalkable;
        worldPosition = _pos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
