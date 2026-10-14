using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public Node parent;

    public int gridX, gridY;

    int heapIndex;

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

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(Node _nodeToCompare)
    {
        int compare = fCost.CompareTo(_nodeToCompare.fCost);
        if (compare == 0) // f costs are the same so h costs are the deciders
        {
            compare = hCost.CompareTo(_nodeToCompare.hCost);
        }

        // CompareTo eturns 1 if current node has higher cost than compared to node
        // For pathfinding we want lower values to be higher priority so invert the result
        return -compare; 
    }

    public Node(bool _isWalkable, Vector3 _pos, int _gridX, int _gridY)
    {
        isWalkable = _isWalkable;
        worldPosition = _pos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
