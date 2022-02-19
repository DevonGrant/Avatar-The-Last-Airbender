using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MovePlayer>() != null)
            WinstateManager.Instance.TriggerWin();
    }
}
