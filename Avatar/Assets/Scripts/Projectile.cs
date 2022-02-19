using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 20f;
    private Rigidbody2D rb;
    private float distanceTraveled = 0;
    public MovePlayer player;

    public void SetVelocityOnInstantiation(Vector3 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir.normalized * speed;
    }

    void Update()
    {
        Physics2D.IgnoreLayerCollision(9,10);//player will never shoot himself but just in case
        distanceTraveled += rb.velocity.x * Time.deltaTime;
        Destroy(gameObject, 3);
        //Debug.Log(distanceTraveled);
        /*if (distanceTraveled >= 40f)
        {
            Debug.Log("Max distance reached");
            Destroy(gameObject);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Input code to trigger approprate scene  SceneManager.LoadScene(#);
        //Debug.Log(collision.name);
        

        Debug.Log(collision.name + "Hit!");
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.player = player;
            CManager.Instance.StartCombat();
        }
        Destroy(gameObject);
    }
}
