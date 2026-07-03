using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Tiles[] tiles;
}

[Serializable]
public class Tiles : MonoBehaviour
{
    [SerializeField] Tile tile;
}
