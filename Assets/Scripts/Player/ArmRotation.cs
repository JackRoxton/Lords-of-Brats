using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public Vector3 mousePos;
    public bool faceR = true;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(player.transform.position).x, Camera.main.WorldToScreenPoint(player.transform.position).y, 0);
        Vector3 dir = new Vector3(mousePos.x - player.transform.position.x, mousePos.y - player.transform.position.y, player.transform.position.z).normalized;
        transform.right = dir;

        if (mousePos.x < player.transform.position.x && faceR)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
            faceR = false;
        }
        else if (mousePos.x >= player.transform.position.x && !faceR)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, -this.transform.localScale.z);
            faceR = true;
        }
    }
}
