using UnityEngine;
using UnityEngine.Splines;

public class Grid : MonoBehaviour
{
    public Transform Player;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius) + (Vector3.forward * (y * nodeDiameter + nodeRadius)));
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint);
            }
    }

    public Node NodeFromWorldPoint(Vector3 _pos)
    {
        // Clamped between 0 and 1 in case object is eve=r outside of grid then invalid node not generated
        float percentX = Mathf.Clamp01((_pos.x + gridWorldSize.x / 2) / gridWorldSize.x); // Half width added as grid is centrred on (0,0) and this will then map left side of grid to be 0
        float percentY = Mathf.Clamp01((_pos.z + gridWorldSize.y / 2) / gridWorldSize.y); // Half height added as grid is centrred on (0,0) and this will then map bottom side of grid to be 0

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
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

                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f)); // Draws cubes for each node in grid (-.1 to give small gap in gizmos drawn
            }
        }
    }
}
