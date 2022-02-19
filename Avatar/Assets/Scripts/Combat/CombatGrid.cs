using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGrid
{
    // number of tiles per side
    public const int Length = 9;

    // references
    public RectTransform Panel;
    public GameObject ButtonPrefab;
    public GameObject[,] grid;

    // list for making tiles diapear and such
    public List<GameObject> marked;
    public List<GameObject> TestMatched;

    // constructor
    public CombatGrid(RectTransform panel, GameObject prefab)
    {
        grid = new GameObject[Length, Length];
        marked = new List<GameObject>();
        TestMatched = new List<GameObject>();
        Panel = panel;
        ButtonPrefab = prefab;
    }

    /// <summary>
    /// Makes a new grid with random tiles in them
    /// </summary>
    public void SpawnGrid()
    {
        // loop for rows
        for (int i = 0; i < Length; i++)
        {
            // loop for columns
            for (int j = 0; j < Length; j++)
            {
                // spawns button and updates the prefab script's data
                CElementButton c = SpawnButton(i, j);
                c.xIndex = i;
                c.yIndex = j;
                c.grid = this;
            }
        }
    }

    /// <summary>
    /// Spawns a singular button at the specified (x,y) coordinates and returns the button component
    /// </summary>
    /// <param name="x">The grid-space x position of the button</param>
    /// <param name="y">The grid-space y position of the button</param>
    /// <returns>CElementButton attached to the prefab that was spawned</returns>
    private CElementButton SpawnButton(int x, int y)
    {
        grid[x, y] = GameObject.Instantiate(ButtonPrefab, Panel, false);
        RectTransform rt = grid[x, y].GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(((rt.sizeDelta.x + 5) * x), -(rt.sizeDelta.y + 5) * y);
        return grid[x, y].GetComponent<CElementButton>();
    }

    /// <summary>
    /// Updates the current grid of tiles to move
    /// them after the player clicks on them
    /// </summary>
    public void UpdateGrid()
    {
        // loop for rows
        for (int i = 0; i < Length; i++)
        {
            // loop for colums
            for (int j = Length - 2; j >= 0; j--)
            {
                // checks if the current tile exists
                if (grid[i, j] != null && !grid[i, j].GetComponent<CElementButton>().IsMoving)
                {
                    // checks if the tile below it exists
                    if (grid[i, j + 1] == null)
                    {
                        // if doesn't exist

                        // makes the tile at i,j fall to i,j+1
                        GameObject g = grid[i, j];

                        //g.GetComponent<CElementButton>().yIndex++;
                        RectTransform rt = g.GetComponent<RectTransform>();
                        CElementButton c = g.GetComponent<CElementButton>();

                        c.target = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - (rt.sizeDelta.y + 5));
                        c.IsMoving = true;
                        c.DoneMoving = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Method that moves the tiles down
    /// </summary>
    public void MoveTiles()
    {
        // loop for rows
        for (int i = 0; i < Length; i++)
        {
            // loop for colums
            for (int j = Length - 2; j >= 0; j--)
            {
                // checks if the current tile exists
                if (grid[i, j] != null)
                {
                    // makes the tile at i,j fall to i,j+1
                    GameObject g = grid[i, j];
                    RectTransform rt = g.GetComponent<RectTransform>();
                    CElementButton c = g.GetComponent<CElementButton>();

                    // checks if tile is moving
                    if (c.IsMoving)
                    {
                        // sets the new transform
                        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - (400f * Time.deltaTime));
                        
                        // if the transform is at the target
                        // stops the moving
                        if (c.target.y >= rt.anchoredPosition.y)
                        {
                            // sets the tile to not moving
                            c.DoneMoving = true;
                            c.IsMoving = false;
                            rt.anchoredPosition = c.target;

                            // checks if top tile
                            if (!c.IsTop)
                            {
                                // sets the original reference to null for next iteration
                                grid[i, j + 1] = g;
                                grid[i, j] = null;
                                c.yIndex++;
                            }
                            else
                            {
                                // sets the top to false
                                c.IsTop = false;
                            }
                        }
                    }
                }
            }
        }

        // checks for new fire matches
        NewMatches();
    }


    /// <summary>
    /// Spawns new tiles at the top of every colum where one is null
    /// </summary>
    public void SpawnTiles()
    {
        // loop that makes the new tiles at the top of
        // each column if neccessary
        for (int i = 0; i < Length; i++)
        {
            // makes sure there is no top tile
            if (grid[i, 0] == null)
            {
                // If it doesn't

                // Make a new tile at the top
                CElementButton c = SpawnButton(i, 0);
                RectTransform rt = c.GetComponent<RectTransform>();

                // gets the script
                c.target = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y /*- (rt.sizeDelta.y + 5)*/);

                // updates its data
                c.xIndex = i;
                c.yIndex = 0;
                c.grid = this;
                c.IsMoving = true;
                c.IsTop = true;
            }
        }
    }


    /// <summary>
    /// Method that checks the falling tiles if they
    /// make a match. CAN CAUSE A DISCO BOARD IF NOT
    /// USED ONLY ON FIRE TILES
    /// </summary>
    public void NewMatches()
    {
        // temp var
        bool matched = false;

        // checks if anything is marked
        if (marked.Count == 0)
        {
            // loops for rows
            for (int i = 0; i < Length; i++)
            {
                // loop for colums
                for (int j = 0; j < Length; j++)
                {
                    // checks if the grid element is null
                    if (grid[i, j] != null)
                    {
                        // gets the script
                        CElementButton c = grid[i, j].GetComponent<CElementButton>();

                        // checks if the tile fell and if it was fire to be checking for matches
                        if (c.DoneMoving && c.type == TileType.Fire)
                        {
                            c.CheckMatches();
                        }

                        // checks if the fires have 5 tiles to match
                        if (TestMatched.Count > 4)
                        {
                            // loop that adds all of the tiles to the marked list
                            for (int k = 0; k < TestMatched.Count; k++)
                            {
                                marked.Add(TestMatched[k]);
                            }
                            TestMatched.Clear();
                            matched = true;
                            break;
                        }
                        else
                        {
                            // loop that clears the unmatched tiles and sets their marked to false
                            for (int k = 0; k < TestMatched.Count; k++)
                            {
                                TestMatched[k].GetComponent<CElementButton>().Marked = false;
                            }
                            TestMatched.Clear();
                        }
                    }
                }

                // exits the loop if matches are made
                if (matched)
                {
                    break;
                }
            }
        }

        // updates the grid if matches are made
        if (matched)
        {
            UpdateGrid();
        }
    }
}
