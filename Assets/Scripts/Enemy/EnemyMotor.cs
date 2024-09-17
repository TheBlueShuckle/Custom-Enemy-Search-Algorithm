using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMotor : MonoBehaviour
{
    [SerializeField] 
    private Transform target;

    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    Vector2 lastTargetPosition;

    private void Update()
    {
        if ((Vector2)target.position != lastTargetPosition)
        {
            lastTargetPosition = target.position;
        }
    }

    private int GetDistanceToTarget(Vector2 target)
    {
        return (int)Mathf.Round(Vector2.Distance(transform.position, target));
    }
}
