
using UnityEngine;

public class MazeGrid : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public float cellSize = 1f;

    public Vector3 origin = new Vector3(-10f, 0f, -10f);
    public LayerMask obstacleMask;

    public bool[,] walkable;

    // BuildGrid() is intentionally NOT called here.
    // MazeGenerator.Awake() calls it after all walls have been instantiated,
    // ensuring Physics.CheckBox finds the correct obstacles.
    void Awake() { }

    public void BuildGrid()
    {
        walkable = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 cellCenter = CellToWorld(x, z)
                                   + Vector3.up * 1f;

                bool blocked = Physics.CheckBox(
                    cellCenter,
                    new Vector3(
                        cellSize * 0.48f,
                        1.0f,
                        cellSize * 0.48f
                    ),
                    Quaternion.identity,
                    obstacleMask
                );

                walkable[x, z] = !blocked;
            }
        }

        Debug.Log(
            $"Maze grid initialized: {width} x {height}"
        );
    }

    public Vector3 CellToWorld(int x, int z)
    {
        return origin + new Vector3(
            (x + 0.5f) * cellSize,
            0f,
            (z + 0.5f) * cellSize
        );
    }

    public bool IsInside(int x, int z)
    {
        return x >= 0 && x < width &&
               z >= 0 && z < height;
    }

    public bool IsWalkable(int x, int z)
    {
        return IsInside(x, z) &&
               walkable != null &&
               walkable[x, z];
    }

    void OnDrawGizmos()
    {
        if (width <= 0 || height <= 0 || cellSize <= 0f)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 center = CellToWorld(x, z);

                bool isWalkable =
                    walkable == null ||
                    x >= walkable.GetLength(0) ||
                    z >= walkable.GetLength(1) ||
                    walkable[x, z];

                Gizmos.color = isWalkable
                    ? new Color(0f, 1f, 0f, 0.25f)
                    : new Color(1f, 0f, 0f, 0.6f);

                Gizmos.DrawCube(
                    center + Vector3.up * 0.03f,
                    new Vector3(
                        cellSize * 0.95f,
                        0.03f,
                        cellSize * 0.95f
                    )
                );

                Gizmos.color = Color.white;

                Gizmos.DrawWireCube(
                    center + Vector3.up * 0.05f,
                    new Vector3(cellSize, 0.02f, cellSize)
                );
            }
        }
    }
}
