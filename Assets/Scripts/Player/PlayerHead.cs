using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [NonSerialized] public bool hitFlag = false;
    int dmg = 1;
    float strength = 50f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hitFlag) return;
        if (collision == null) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetHit(dmg, new Vector2(this.transform.position.x - enemy.transform.position.x, this.transform.position.y - enemy.transform.position.y).normalized * strength);
        }
    }
}
