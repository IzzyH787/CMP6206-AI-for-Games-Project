using UnityEngine;
using System.Collections.Generic;
public class Grid : MonoBehaviour
{
    public static Grid Instance; 
    public Transform Player;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    public List<Node> walkableNodes {get; private set;}

    public float nodeDiameter { get; private set; }
    int gridSizeX, gridSizeY;

    public List<Node> path;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    public void CreateGrid()
    {
        //Create list if null
        if (walkableNodes == null)
        {
            walkableNodes = new List<Node>();
        }

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius) + (Vector3.forward * (y * nodeDiameter + nodeRadius)));
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

                // Create list of walkable nodes
                if (walkable)
                    walkableNodes.Add(new Node(walkable, worldPoint, x, y));
            }
    }

    public Node NodeFromWorldPoint(Vector3 _pos)
    {
        // Clamped between 0 and 1 in case object is ever outside of grid then invalid node not generated
        float percentX = Mathf.Clamp01((_pos.x + gridWorldSize.x / 2) / gridWorldSize.x); // Half width added as grid is centrred on (0,0) and this will then map left side of grid to be 0
        float percentY = Mathf.Clamp01((_pos.z + gridWorldSize.y / 2) / gridWorldSize.y); // Half height added as grid is centrred on (0,0) and this will then map bottom side of grid to be 0

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Node NodeFromGameObject(GameObject _obj)
    {
        return NodeFromWorldPoint(_obj.transform.position);
    }

    public List<Node> GetNeighbours(Node _node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue; // Skip as this is the node we are looking for neighbours for

                // Get coords of node to check for if neighbour
                int checkX = _node.gridX + x;
                int checkY = _node.gridY + y;

                if (checkX >= 0 && checkX <  gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }

        return neighbours;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(Player.position);
            foreach (Node node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.white : Color.red;

                // Special color for node with player
                if (playerNode == node)
                    Gizmos.color = Color.cyan;

                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.4f)); // Draws cubes for each node in grid (-.4 to give larger gap in gizmos drawn)
            }
        }
    }
}
