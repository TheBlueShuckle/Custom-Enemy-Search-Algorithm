using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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

    private void Start()
    {
        map = ConvertTilesToNodes();

        foreach (Node node in map)
        {
            print(node.Position.x + ", " + node.Position.y + ". " + node.IsTraversable);
        }
    }

    private Node[,] ConvertTilesToNodes()
    {
        xOffset = groundTilemap.cellBounds.xMin;
        yOffset = groundTilemap.cellBounds.yMin;

        Node[,] nodes = new Node[groundTilemap.cellBounds.xMax - xOffset, groundTilemap.cellBounds.yMax - yOffset];

        for (int y = 0; y < groundTilemap.cellBounds.yMax - yOffset; y++)
        {
            for (int x = 0; x < groundTilemap.cellBounds.xMax - xOffset; x++)
            {
                bool isTraversable = !collisionTilemap.HasTile(new Vector3Int(groundTilemap.cellBounds.x, groundTilemap.cellBounds.x, 0)) || !groundTilemap.HasTile(new Vector3Int(groundTilemap.cellBounds.x, groundTilemap.cellBounds.x, 0));
                nodes[x, y] = new Node(null, new Vector2Int(x, y), isTraversable);
            }
        }

        return nodes;
    }

    public Vector2Int ConvertWorldToGrid(Vector2Int position)
    {
        Vector2Int newPosition = Vector2Int.zero;

        xOffset = groundTilemap.cellBounds.xMin;
        yOffset = groundTilemap.cellBounds.yMin;

        newPosition.x = position.x - xOffset;
        newPosition.y = position.y - yOffset;

        return newPosition;
    }

    public Vector2Int ConvertNodeIndexToWorldPosition(Node node)
    {
        return new Vector2Int(node.Position.x + xOffset, node.Position.y + yOffset);
    }
}
