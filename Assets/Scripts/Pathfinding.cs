using UnityEngine;
using System.Collections.Generic;
using System.Transactions;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }
    public void FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(_startPosition);
        Node targetNode = grid.NodeFromWorldPoint(_targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0]; // Current node is first in open set

            // Get lowest cost node in open set (reachable nodes to check)
            for (int i = 1; i < openSet.Count; i++)  // Start at because current node is 1
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            // Remove node from open set and add to closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return; // Exit out as found path to node
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    continue; // Skip node if not waklable or already been checked

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                // Checks if this path has found a better route to neighbour than found previously or neighbour not been found yet
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) // TO DO: Avoid repeated check
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode; // start at end as retracing path

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse(); // Sorts list to be start -> end
        grid.path = path;
    }
    public int GetDistance(Node _nodeA, Node _nodeB)
    {
        int distanceX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int distanceY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        if (distanceX > distanceY)
            // Will need to move diagnonally upwards then straight horizontally
            return (14*distanceY) + (10 * (distanceX - distanceY)); // 14 used to mimic sqrt2 being diagonal dist, 10 used to mimic 1 being horizontal/vertical dist

        // Will move digagonally sideways then straight vertically
        return (14 * distanceX) + (10 * (distanceY -  distanceX));
    }
}
