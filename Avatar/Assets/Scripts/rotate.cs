using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    private bool currentRotation;//true=Right false = Left
    void Start()
    {
        currentRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Left")&& currentRotation)
        {
            RotateLeft();
            currentRotation = false;
        }
        if (Input.GetButtonDown("Right")&& !currentRotation)
        {
            RotateRight();
            currentRotation = true;
        }
    }
    void RotateLeft()
    {
        Debug.Log("Left Rotation Triggered");

        transform.Rotate(Vector3.up*180);
    }
    void RotateRight()
    {
        Debug.Log("Right Rotation Triggered");
        transform.Rotate(Vector3.down * 180);
    }
}
