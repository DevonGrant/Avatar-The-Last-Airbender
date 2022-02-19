using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bending : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform bendPoint;
    public GameObject bendingPrefab;
    public MovePlayer player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!CManager.Instance.Running && Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    void Fire()
    {
        GameObject spawned = Instantiate(bendingPrefab, bendPoint.position, bendPoint.rotation);
        Projectile p = spawned.GetComponent<Projectile>();
        p.player = player;
        p.SetVelocityOnInstantiation(bendPoint.right);
    }
}
