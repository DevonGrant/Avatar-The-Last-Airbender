using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CManager : MonoBehaviour
{
    public static CManager Instance { get; private set; }
    const float MaxCanvasRotation = 7;

    // necessary references and prefabs
    public Animator EnemyAnimator;
    public Animator PlayerAnimator;
    public RectTransform Panel;
    public RectTransform CanvasContainer;
    public GameObject ButtonPrefab;
    public Slider EnemySlider;
    public Slider TimerSlider;
    public TMP_Text stateText;
    public TMP_Text enemyText;
    public TMP_Text timerText;
    public float CanvasRotationFactor;
    public Camera uiCamera;
    public Button endButton;

    // play variables
    public float timer;
    public float maxTimer;
    public float enemyHealth;
    public float maxEnemyHealth;
    public bool ended;
    private bool running;
    public bool Running { get { return running; } }
    public bool Ended { get { return ended; } }

    public bool hasPlayerWon;

    //public Enemy enemy;
    public Enemies enemies;
    private EnemyBending stunnedEnemy = null;

    // grid
    public CombatGrid grid;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        ended = true;
        running = false;
    }

    private void Initialize()
    {
        stunnedEnemy = enemies.EnemyStunned().GetComponent<EnemyBending>();
        hasPlayerWon = false;
        EnemyAnimator.SetInteger("state", -1);
        PlayerAnimator.SetInteger("state", -1);
        // sets all the references it needs
        timer = maxTimer;
        enemyHealth = maxEnemyHealth;
        EnemySlider.maxValue = maxEnemyHealth;
        EnemySlider.value = enemyHealth;
        TimerSlider.maxValue = maxTimer;
        TimerSlider.value = timer;
        grid = new CombatGrid(Panel, ButtonPrefab);
        ended = false;
        running = true;
        uiCamera.enabled = true;
        endButton.gameObject.SetActive(false);
        endButton.onClick.AddListener(() =>
        {
            End();
            //stunnedEnemy = null;
        });
        stateText.text = "Find matching tiles! Avoid matching fire!";
        PlatformerUI.Instance.ToggleWindowHUD(false);

        // spawns the random grid
        grid.SpawnGrid();
    }

    public void StartCombat()
    {
        
        if (!running)
            Initialize();
    }

    private void End()
    {
        foreach (GameObject g in grid.grid)
            Destroy(g);
        uiCamera.enabled = false;
        running = false;
        if (WinstateManager.Instance.LossState >= 3)
            PlatformerUI.Instance.ToggleWindowPause(true);
        else
            PlatformerUI.Instance.ToggleWindowHUD(true);

        if (hasPlayerWon)
        {
            stunnedEnemy.Stunned();
            hasPlayerWon = false;
        }
    }

    void Start()
    {
        uiCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            // updates the unity fields and timer
            if (!ended)
                timer -= Time.deltaTime;
            TimerSlider.value = Mathf.Lerp(TimerSlider.value, timer, Time.deltaTime);
            EnemySlider.value = Mathf.Lerp(EnemySlider.value, enemyHealth, Time.deltaTime);
            timerText.text = string.Format("Time Left: {0:F2}", timer);
            enemyText.text = string.Format("Enemy Health: {0}", enemyHealth);

            // rotates the canvas based on who's in "favor" of the ratio of sliders
            float timerRatio = TimerSlider.value / TimerSlider.maxValue;
            float enemyRatio = EnemySlider.value / EnemySlider.maxValue;
            CanvasContainer.rotation = Quaternion.Slerp(
                CanvasContainer.rotation,
                Quaternion.Euler(0, Mathf.Clamp(
                    CanvasRotationFactor * (enemyRatio - timerRatio),
                    -MaxCanvasRotation,
                    MaxCanvasRotation), 0),
                Time.deltaTime);

            // moves tiles down
            grid.MoveTiles();

            // updates and spawns the grid
            grid.UpdateGrid();

            // spawns new tiles
            grid.SpawnTiles();

            // checks if player lost
            if (timer <= 0 && enemyHealth > 0 && !ended)
            {
                ended = true;
                stateText.text = "You lost :(";
                WinstateManager.Instance.LossState++;
                endButton.gameObject.SetActive(true);
                hasPlayerWon = false;
            }

            // checks if the enemy has too much health
            if (enemyHealth > maxEnemyHealth)
            {
                enemyHealth = maxEnemyHealth;
            }
            // checks if the player won
            else if (enemyHealth <= 0f && timer > 0 && !ended)
            {
                ended = true;
                stateText.text = "You win :)";
                WinstateManager.Instance.KeyState++;
                endButton.gameObject.SetActive(true);
                hasPlayerWon = true;
            }

            // checks if there are marked tiles to be deleted
            if (grid.marked.Count > 2)
            {
                // for debugging
                //Debug.Log(string.Format("Destroying {0} tiles", grid.marked.Count));

                // loop that goes through the marked tiles
                int count = grid.marked.Count;
                for (int i = 0; i < count; i++)
                {
                    // gets the data it needs
                    GameObject g = grid.marked[0];
                    grid.marked.Remove(g);
                    CElementButton c = g.GetComponent<CElementButton>();

                    // checks to see how the score needs to be adjusted
                    // values subject to change
                    if (c.type == TileType.Fire)
                    {
                        enemyHealth += 10 * grid.marked.Count;
                    }
                    else
                    {
                        enemyHealth -= 5 * grid.marked.Count;
                    }

                    // resets the reference of the tiles
                    grid.grid[c.xIndex, c.yIndex] = null;
                    Destroy(g);
                }

                // resets the marked list
                grid.marked.Clear();
            }
            else // if there wasn't enough to match
            {
                // removes all of the marked elements and clears the list
                for (int i = 0; i < grid.marked.Count; i++)
                {
                    grid.marked[i].GetComponent<CElementButton>().Marked = false;
                }
                grid.marked.Clear();
            }
        }
    }
}
