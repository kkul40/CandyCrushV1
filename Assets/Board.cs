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


    private void Start()
    {
        _allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
            var tempPosition = new Vector2(i * spaces, j * spaces);
            var backgroundTile = Instantiate(tilePrefab, tempPosition, quaternion.identity);
            backgroundTile.transform.parent = transform;
            backgroundTile.name = "( " + i + ", " + j + " )";
            var dotToUse = Random.Range(0, dots.Length);


            int maxIteration = 0;
            while (MatchesAt(i,j, dots[dotToUse]) && maxIteration< 100)
            {
                dotToUse = Random.Range(0, dots.Length);
                maxIteration++;
            }
            maxIteration = 0;
            

            var dot = Instantiate(dots[dotToUse], tempPosition, quaternion.identity);
            dot.transform.parent = transform;
            dot.transform.name = "( " + i + ", " + j + " )";
            allDots[i, j] = dot;
        }
    }

    private bool MatchesAt(int column, int row, GameObject peace)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor
                || allDots[column -2, row].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor)
            {
                return true;
            }
            if (allDots[column, row - 1].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor
                || allDots[column, row - 2].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor
                    && allDots[column, row - 2].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column-1, row].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor
                    && allDots[column-2, row].GetComponent<Dot>().DotColor == peace.GetComponent<Dot>().DotColor)
                {
                    return true;
                }
            }
        }
        
        
        return false;
    }


    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i,j);
                }
            }
        }
    }
}