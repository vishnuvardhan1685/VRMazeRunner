
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    public MazeGrid mazeGrid;

    public List<Vector3> FindPath(Vector3 startWorld, Vector3 goalWorld)
    {
        List<Vector3> path = new List<Vector3>();

        if (mazeGrid == null || mazeGrid.walkable == null)
        {
            Debug.LogError("AStarPathfinder: MazeGrid is not ready.");
            return path;
        }

        GridNode[,] nodes = new GridNode[mazeGrid.width, mazeGrid.height];

        for (int x = 0; x < mazeGrid.width; x++)
        {
            for (int z = 0; z < mazeGrid.height; z++)
            {
                nodes[x, z] = new GridNode(x, z);
            }
        }

        Vector3 startOffset = startWorld - mazeGrid.origin;
        Vector3 goalOffset = goalWorld - mazeGrid.origin;

        int startX = Mathf.FloorToInt(startOffset.x / mazeGrid.cellSize);
        int startZ = Mathf.FloorToInt(startOffset.z / mazeGrid.cellSize);
        int goalX = Mathf.FloorToInt(goalOffset.x / mazeGrid.cellSize);
        int goalZ = Mathf.FloorToInt(goalOffset.z / mazeGrid.cellSize);

        if (!mazeGrid.IsWalkable(startX, startZ) ||
            !mazeGrid.IsWalkable(goalX, goalZ))
        {
            Debug.LogWarning("Start or goal is outside the grid or blocked.");
            return path;
        }

        GridNode startNode = nodes[startX, startZ];
        GridNode goalNode = nodes[goalX, goalZ];

        List<GridNode> openList = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            GridNode current = openList[0];

            foreach (GridNode node in openList)
            {
                if (node.FCost < current.FCost ||
                    (node.FCost == current.FCost &&
                     node.hCost < current.hCost))
                {
                    current = node;
                }
            }

            openList.Remove(current);
            closedSet.Add(current);

            if (current == goalNode)
            {
                GridNode trace = goalNode;

                while (trace != null)
                {
                    path.Add(mazeGrid.CellToWorld(trace.x, trace.z));
                    trace = trace.parent;
                }

                path.Reverse();
                return path;
            }

            foreach (Vector2Int direction in GetDirections())
            {
                int nextX = current.x + direction.x;
                int nextZ = current.z + direction.y;

                if (!mazeGrid.IsWalkable(nextX, nextZ))
                    continue;

                GridNode neighbor = nodes[nextX, nextZ];

                if (closedSet.Contains(neighbor))
                    continue;

                int tentativeG = current.gCost + 10;

                if (tentativeG < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost =
                        (Mathf.Abs(goalX - nextX) +
                         Mathf.Abs(goalZ - nextZ)) * 10;

                    neighbor.parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        Debug.LogWarning("AStarPathfinder: No path found.");
        return path;
    }

    private Vector2Int[] GetDirections()
    {
        return new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };
    }
}
