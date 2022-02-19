using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public MovePlayer player;
    public EnemyBending bending;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy Touching" + other.name);
        player = other.GetComponent<MovePlayer>();
        if (player != null && !bending.stunned)
        {
            CManager.Instance.StartCombat();
        }
    }
}
