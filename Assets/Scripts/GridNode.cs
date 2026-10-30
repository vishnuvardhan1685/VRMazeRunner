
using UnityEngine;

public class GridNode
{
    public int x;
    public int z;

    public int gCost;
    public int hCost;

    public GridNode parent;

    public int FCost => gCost + hCost;

    public GridNode(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
