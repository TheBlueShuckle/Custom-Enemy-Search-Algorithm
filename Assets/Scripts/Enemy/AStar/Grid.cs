using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
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
        xOffset = groundTilemap.cellBounds.xMin;
        yOffset = groundTilemap.cellBounds.yMin;

        map = ConvertTilesToNodes();
    }

    private Node[,] ConvertTilesToNodes()
    {
        Node[,] nodes = new Node[groundTilemap.cellBounds.xMax - xOffset, groundTilemap.cellBounds.yMax - yOffset];

        for (int y = 0; y < groundTilemap.cellBounds.yMax - yOffset; y++)
        {
            for (int x = 0; x < groundTilemap.cellBounds.xMax - xOffset; x++)
            {
                bool isTraversable = groundTilemap.HasTile(new Vector3Int(x + xOffset, y + yOffset, 0)) && !collisionTilemap.HasTile(new Vector3Int(x + xOffset, y + yOffset, 0));
                nodes[x, y] = new Node(null, new Vector2Int(x, y), isTraversable);
            }
        }

        #region Test
        string message = "";

        for (int y = 0; y < nodes.GetLength(1); y++)
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                message += (nodes[x, y].IsTraversable ? 'O' : '\u25A0') + " ";
            }

            message += '\n';
        }

        Debug.Log(message);
        #endregion

        return nodes;
    }

    public Vector2Int ConvertWorldToGrid(Vector2Int position)
    {
        Vector2Int newPosition = Vector2Int.zero;

        newPosition.x = position.x - xOffset;
        newPosition.y = position.y - yOffset;

        return newPosition;
    }

    public Vector2Int ConvertNodeIndexToWorldPosition(Node node)
    {
        return new Vector2Int(node.Position.x + xOffset, node.Position.y + yOffset);
    }
}
