using System.Collections;
using UnityEngine;

public enum DotColors
{
    Red,
    Green,
    Blue,
    Pink,
    Orange,
    Yellow
}

public class Dot : MonoBehaviour
{
    [Header("Board Variables")] public DotColors DotColor;

    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;

    public int targetX;
    public int targetY;
    public bool isMatched;
    public float swipeAngle;
    public float swipeResist = 1f;

    private Board _board;
    private Vector2 finalTouchPosition;


    private Vector2 firstTouchPosition;
    private GameObject otherDot;
    private Vector2 tempPosition;

    private void Start()
    {
        _board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        previousRow = row;
        previousColumn = column;
    }

    private void Update()
    {
        FindMatches();
        if (isMatched)
        {
            var mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
            Debug.Log(DotColor);
        }

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            // Move Toward The Target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            // Directly Set The Position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            _board.allDots[column, row] = gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move Toward The Target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            // Directly Set The Position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            _board.allDots[column, row] = gameObject;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
        MovePieces();
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            }
            else
            {
                _board.DestroyMatches();
            }
 
            otherDot = null;
    }

    private void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist
            || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
            finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        }
    }

    private void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < _board.width - 1)
        {
            // Right Swap
            otherDot = _board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > -45 && swipeAngle <= 135 && row < _board.height - 1)
        {
            // Up Swap
            otherDot = _board.allDots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if (swipeAngle > 135 || (swipeAngle <= -135 && column > 0))
        {
            // Left Swap
            otherDot = _board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down Swap
            otherDot = _board.allDots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }

    private void FindMatches()
    {
        if (column > 0 && column < _board.width - 1)
        {
            var leftDotGo1 = _board.allDots[column - 1, row];
            var rightDotGo1 = _board.allDots[column + 1, row];
            
            leftDotGo1.TryGetComponent(out Dot leftDot);
            rightDotGo1.TryGetComponent(out Dot rightDot);
            
            if (leftDotGo1 != null && rightDotGo1 != null)
            {
                if (leftDot.DotColor == DotColor && rightDot.DotColor == DotColor)
                {
                    leftDot.isMatched = true;
                    rightDot.isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < _board.height - 1)
        {
            var upDotGo1 = _board.allDots[column, row + 1];
            var downDotGo1 = _board.allDots[column, row - 1];

            upDotGo1.TryGetComponent(out Dot upDot);
            downDotGo1.TryGetComponent(out Dot downDot);
            
            if (upDotGo1 != null && downDotGo1 != null)
            {
                if (upDot.DotColor == DotColor && downDot.DotColor == DotColor)
                {
                    upDot.isMatched = true;
                    downDot.isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}