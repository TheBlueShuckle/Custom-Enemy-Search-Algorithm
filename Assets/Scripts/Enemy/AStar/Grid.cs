using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public Vector2Int size;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    public Node[,] map;

    private int xOffset;
    private int yOffset;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, (Vector2)size);
    }

    private void Awake()
    {
        
    }

    private Node[,] ConvertTilesToNodes()
    {
        Node[,] nodes = new Node[size.x, size.y];
        xOffset = groundTilemap.cellBounds.xMin;
        yOffset = groundTilemap.cellBounds.yMin;


        for (int y = 0; y < groundTilemap.cellBounds.y - yOffset; y++)
        {
            for (int x = 0; x < groundTilemap.cellBounds.x - xOffset; x++)
            {
                nodes[x, y] = new Node(null, new Vector2Int(x, y), true);
            }
        }
    }

    private Vector2Int ConvertNodeIndexToVector2Int(Node node)
    {
        return new Vector2Int(node.Position.x + xOffset, node.Position.y + yOffset);
    }
}
