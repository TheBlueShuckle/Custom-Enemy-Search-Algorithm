using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMotor : MonoBehaviour
{
    [SerializeField] 
    private Transform target;

    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    Vector2 lastTargetPosition;
    private AStar aStar;

    private Node[] path;
    private Node[,] map;

    private void Start()
    {
        map = ConvertTilemapToAstarMap(groundTilemap, collisionTilemap);
    }

    private void Update()
    {
        if ((Vector2)target.position != lastTargetPosition)
        {
            lastTargetPosition = target.position;

            path = GetPath();
        }
    }

    private Node[,] ConvertTilemapToAstarMap(Tilemap ground, Tilemap obstacles)
    {

    }

    private Node[] GetPath()
    {
        Vector3Int start = groundTilemap.WorldToCell(transform.position);
        Vector3Int goal = groundTilemap.WorldToCell(target.position);

        aStar = new AStar(map, new Node(null, start.x, start.y, true), new Node(null, goal.x, goal.y, true));
        return aStar.Path;
    }
}
