using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRange : MonoBehaviour
{
    public PlayerController Player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.tag == "Pickable" && Player.pickingUp)
        {
            if (Player.SavedWeapon != null) return;
            Player.SavedWeapon = collision.gameObject;
            Player.SavedWeapon.tag = "Weapon";
            Player.SavedWeapon.GetComponent<Weapon>().ChangeSprite(Player.gameObject);
            if (Player.faceR)
            {
                Player.SavedWeapon.transform.localScale = new Vector3(-1, -1, 1);
            }
            else if (!Player.faceR)
            {
                Player.SavedWeapon.transform.localScale = new Vector3(-1, 1, 1);
            }
            SoundManager.Instance.Play("Pick");
        }
    }
}
