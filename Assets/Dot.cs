using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DotColors
{
    Red,
    Green,
    Blue,
    Pink,
    Orange,
    Yellow,
}

public class Dot : MonoBehaviour
{
    public DotColors DotColor;
    
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    
    private Board _board;
    private GameObject otherDot;
                    
    
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0f;

    private void Start()
    {
        _board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x; 
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;

    }

    private void Update()
    {
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
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
            _board.allDots[column, row] = this.gameObject;
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
            _board.allDots[column, row] = this.gameObject;
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

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
            finalTouchPosition.x - firstTouchPosition.x) * 180/Mathf.PI;
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < _board.width)
        {
            // Right Swap
            otherDot = _board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > -45 && swipeAngle <= 135 && row < _board.height)
        {
            // Up Swap
            otherDot = _board.allDots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if (swipeAngle > 135 || swipeAngle <= -135 && column > 0)
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
    }

    void FindMatches()
    {
        if (column > 0 && column < _board.width - 1)
        {
            GameObject leftDotGo1 = _board.allDots[column - 1, row];
            GameObject rightDotGo1 = _board.allDots[column + 1, row];

            Dot leftDot = leftDotGo1.GetComponent<Dot>();
            Dot rightDot = rightDotGo1.GetComponent<Dot>();
            
            if(leftDot.DotColor == DotColor && rightDot.DotColor == DotColor)
            {
                leftDot.isMatched = true;
                rightDot.isMatched = true;
                isMatched = true;
            }
        }
        
        if (row > 0 && row < _board.height -1)
        {
            GameObject upDotGo1 = _board.allDots[column, row + 1];
            GameObject downDotGo1 = _board.allDots[column, row - 1];
            
            Dot upDot = upDotGo1.GetComponent<Dot>();
            Dot downDot = downDotGo1.GetComponent<Dot>();
            
            if (upDot.DotColor == DotColor && downDot.DotColor == DotColor)
            {
                upDot.isMatched = true;
                downDot.isMatched = true;
                isMatched = true;
            }
        }
    }
    
}


