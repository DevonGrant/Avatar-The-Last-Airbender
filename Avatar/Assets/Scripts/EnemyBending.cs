using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBending : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Transform eBendPoint;
    public GameObject eBendingPrefab;
    private float timer = 5f;
    public bool stunned;
    public Enemy enemy;
    void Start()
    {
        CManager.Instance.hasPlayerWon = false;
        stunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CManager.Instance.Running)
            Attack();
    }
    private void Attack()
    {
        if (!stunned)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 5;
                StartCoroutine(LetFireAnimationPlay());
            }
        }
    }

    private IEnumerator LetFireAnimationPlay()
    {
        animator.SetInteger("state", 1);
        yield return new WaitForSeconds(0.2f);

        GameObject spawned = Instantiate(eBendingPrefab, eBendPoint.position, eBendPoint.rotation);
        EnemyProjectile p = spawned.GetComponent<EnemyProjectile>();
        p.enemy = enemy;
        p.SetVelocityOnInstantiation(eBendPoint.right);

        yield return new WaitForSeconds(0.8f);
        animator.SetInteger("state", 0);
    }
    /*private IEnumerator Stun()
    {
        hasPlayerWon = true;
        yield return new WaitForSeconds(80.0f);
        hasPlayerWon = false;
    }*/
    private IEnumerator Stun()
    {
        /*Debug.Log(CManager.Instance.stunTimer);
        CManager.Instance.stunTimer -= Time.deltaTime;
        if (CManager.Instance.stunTimer <= 0)
        {
            stunned = false;
            CManager.Instance.hasPlayerWon = false;
            CManager.Instance.stunTimer = 10;
        }*/
        enemy.player = null;
        Debug.Log("Stunned");
        stunned = true;
        animator.SetInteger("state", 2);
        yield return new WaitForSeconds(10.0f);
        animator.SetInteger("state", 0);
        stunned = false;
    }
    public void Stunned()
    {
        StartCoroutine(Stun());
    }
}
