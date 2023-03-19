using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public float spaces;
    public GameObject tilePrefab;
    public GameObject[] dots;
    private BackgroundTile[,] _allTiles;
    public GameObject[,] allDots;

    
    void Start()
    {
        _allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i * spaces, j * spaces);
                GameObject backgroundTile = Instantiate(tilePrefab,tempPosition,quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";
                
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, quaternion.identity);
                dot.transform.parent = this.transform;
                dot.transform.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;

            }
        }
    }
}
