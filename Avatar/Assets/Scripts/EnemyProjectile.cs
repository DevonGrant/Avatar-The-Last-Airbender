using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 20f;
    private Rigidbody2D rb;
    private float distanceTraveled = 0;
    public Enemy enemy;

    public void SetVelocityOnInstantiation(Vector3 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir.normalized * speed;
    }

    void Update()
    {
        Physics2D.IgnoreLayerCollision(11, 13);//enemy proj. dont hit each other
        Physics2D.IgnoreLayerCollision(14, 13);//enemy proj. dont hit the door
        distanceTraveled += rb.velocity.x * Time.deltaTime;
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Input code to trigger approprate scene  SceneManager.LoadScene(#);
        //Debug.Log(collision.name);


        Debug.Log(collision.name + "Hit!");
        MovePlayer player = collision.GetComponent<MovePlayer>();

        if (player != null)
        {
            enemy.player = player;
            CManager.Instance.StartCombat();

        }
        Destroy(gameObject);
    }
}
