using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool hitFlag = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null && hitFlag)
        {
            //hit
            hitFlag = false;
        }
    }
}
