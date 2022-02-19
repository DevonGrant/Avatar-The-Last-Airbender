using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Enemy[] enemies;
    private Enemy stunnedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        stunnedEnemy = null;

    }

    // Update is called once per frame
    void Update()
    {
        stunnedEnemy = EnemyStunned();
    }
    public Enemy EnemyStunned()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.player != null)
            {
                stunnedEnemy = enemy.GetComponent<Enemy>();
            }
        }
        return stunnedEnemy;
    }
}
