using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMotor : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    [SerializeField] private Grid aStarGrid;

    Vector2 lastTargetPosition;
    private AStar aStar;

    private Node[] path;

    Cooldown movementCooldown;

    private void Awake()
    {
        movementCooldown = new Cooldown(1);
    }

    private void Update()
    {
        if ((Vector2)target.position != lastTargetPosition)
        {
            print("Finding new path");
            lastTargetPosition = target.position;

            path = FindNewPath();
        }

        if (!movementCooldown.IsCoolingDown && (path != null || path.Count() > 0))
        {
            print("First move is " + path[0].Position.x + ", " + path[0].Position.y);
            print("Moving to target at " + path.Last().Position.x + ", " + path.Last().Position.y);
            MoveToTarget();
        }
    }


    private Node[] FindNewPath()
    {
        return GetPath();
    }

    private void MoveToTarget()
    {
        float positionOffset = 0.5f;

        Vector2Int tilePosition = aStarGrid.ConvertNodeIndexToWorldPosition(path[0]);
        transform.position = new Vector3(tilePosition.x + positionOffset, tilePosition.y + positionOffset, 0);

        path = path.Skip(1).ToArray();
        movementCooldown.StartCooldown();
    }

    private Node[] GetPath()
    {
        Vector3Int startCellPosition = groundTilemap.WorldToCell(transform.position);
        Vector3Int goalCellPosition = groundTilemap.WorldToCell(target.position);

        Vector2Int start = aStarGrid.ConvertWorldToGrid(new Vector2Int(startCellPosition.x, startCellPosition.y));
        Vector2Int goal = aStarGrid.ConvertWorldToGrid(new Vector2Int(goalCellPosition.x, goalCellPosition.y));

        aStar = new AStar(aStarGrid.map, new Node(null, (Vector2Int)start, true), new Node(null, (Vector2Int)goal, true));
        return aStar.Path;
    }
}
