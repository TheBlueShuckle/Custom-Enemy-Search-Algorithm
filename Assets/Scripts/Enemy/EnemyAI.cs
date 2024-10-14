using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private Tilemap _groundTilemap;
    [SerializeField]
    private Tilemap _collisionTilemap;

    private void Start()
    {
        
    }

    private void FindOptimalPath(Tile start, Tile goal)
    {
        Tile[] _closed;
        Tile[] _open;

    }
}
