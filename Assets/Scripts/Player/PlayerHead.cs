using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [NonSerialized] public bool hitFlag = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hitFlag) return;
        if (collision == null) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetHit(1);
        }
    }
}
