using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileType
{
    Water,
    Earth,
    Fire,
    Air
}


public class CElementButton : MonoBehaviour
{
    // Indecies for the grid array
    public int xIndex;
    public int yIndex;

    // element type
    public TileType type;

    // type images
    public Sprite fire;
    public Sprite earth;
    public Sprite air;
    public Sprite water;

    // grid its stored in
    public CombatGrid grid;

    // going to be used for the moving tiles
    public Vector2 target;
    public bool IsMoving;
    public bool DoneMoving;
    public bool IsTop;

    // if marked or not to be deleted
    public bool Marked;

    // Start is called before the first frame update
    void Start()
    {
        // makes it a random element
        switch (Random.Range(0, 4))
        {
            // water element
            case 0:
                type = TileType.Water;
                gameObject.GetComponent<Image>().sprite = water;
                break;

                // Earth element
            case 1:
                type = TileType.Earth;
                gameObject.GetComponent<Image>().sprite = earth;
                break;

                // fire element
            case 2:
                type = TileType.Fire;
                gameObject.GetComponent<Image>().sprite = fire;
                
                // chance that you can't interact with it
                if (Random.Range(0, 3) == 1)
                    gameObject.GetComponent<Button>().interactable = false;
                break;

                // air element
            case 3:
                type = TileType.Air;
                gameObject.GetComponent<Image>().sprite = air;
                break;
        }
    }

    /// <summary>
    /// Recursive method that checks and marks the tiles
    /// that need to be for deletion after being clicked on
    /// </summary>
    public void Clicked()
    {
        if (!CManager.Instance.Ended)
        {
            // sets this game object to be marked
            grid.marked.Add(this.gameObject);
            Marked = true;

            // Series of checks for the tiles around
            // Left Tile
            if (xIndex != 0)
            {
                // first checks if null
                if (grid.grid[xIndex - 1, yIndex] != null)
                {
                    // gets the left script
                    CElementButton left = grid.grid[xIndex - 1, yIndex].GetComponent<CElementButton>();

                    // checks if it's been checked
                    if (left.type == type && !left.Marked && !left.IsMoving)
                    {
                        // checks it
                        left.Clicked();
                    }
                }
            }

            // Top Tile
            if (yIndex != 0)
            {
                // first checks if null
                if (grid.grid[xIndex, yIndex - 1] != null)
                {
                    // top script
                    CElementButton top = grid.grid[xIndex, yIndex - 1].GetComponent<CElementButton>();

                    // checks mark
                    if (top.type == type && !top.Marked && !top.IsMoving)
                    {
                        // checks tile
                        top.Clicked();
                    }
                }
            }

            // Right Tile
            if (xIndex != 8)
            {
                // first checks if null
                if (grid.grid[xIndex + 1, yIndex] != null)
                {
                    // right script
                    CElementButton right = grid.grid[xIndex + 1, yIndex].GetComponent<CElementButton>();

                    // checks marked
                    if (right.type == type && !right.Marked && !right.IsMoving)
                    {
                        // checks tile
                        right.Clicked();
                    }
                }
            }

            // Bottom Tile
            if (yIndex != 8)
            {
                // first checks if null
                if (grid.grid[xIndex, yIndex + 1] != null)
                {
                    // bottom script
                    CElementButton bottom = grid.grid[xIndex, yIndex + 1].GetComponent<CElementButton>();

                    // checks marked
                    if (bottom.type == type && !bottom.Marked && !bottom.IsMoving)
                    {
                        // checks tile
                        bottom.Clicked();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Works similar to the Clicked method, but is
    /// meant to be used when the tiles fall. Should
    /// only be checking the fire tiles
    /// </summary>
    public void CheckMatches()
    {
        // adds to matched
        grid.TestMatched.Add(this.gameObject);
        Marked = true;
        
        //Seires of checks
        // Left Tile
        if (xIndex != 0)
        {
            // first checks if null
            if (grid.grid[xIndex - 1, yIndex] != null)
            {
                // script
                CElementButton left = grid.grid[xIndex - 1, yIndex].GetComponent<CElementButton>();

                // checks if should check
                if (left.type == type && !left.Marked && !left.IsMoving)
                {
                    // checks
                    left.CheckMatches();
                }
            }
        }

        // Top Tile
        if (yIndex != 0)
        {
            // first checks if null
            if (grid.grid[xIndex, yIndex - 1] != null)
            {
                // script
                CElementButton top = grid.grid[xIndex, yIndex - 1].GetComponent<CElementButton>();

                // checks params
                if (top.type == type && !top.Marked && !top.IsMoving)
                {
                    // checks tile
                    top.CheckMatches();
                }
            }
        }

        // Right Tile
        if (xIndex != 8)
        {
            // first checks if null
            if (grid.grid[xIndex + 1, yIndex] != null)
            {
                // get script
                CElementButton right = grid.grid[xIndex + 1, yIndex].GetComponent<CElementButton>();

                // check params
                if (right.type == type && !right.Marked && !right.IsMoving)
                {
                    // check tiles
                    right.CheckMatches();
                }
            }
        }

        // Bottom Tile
        if (yIndex != 8)
        {
            // first checks if null
            if (grid.grid[xIndex, yIndex + 1] != null)
            {
                // get script
                CElementButton bottom = grid.grid[xIndex, yIndex + 1].GetComponent<CElementButton>();

                // check params
                if (bottom.type == type && !bottom.Marked && !bottom.IsMoving)
                {
                    // check tile
                    bottom.CheckMatches();
                }
            }
        }
    }
}
