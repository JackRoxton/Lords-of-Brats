using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 Movement;
    float speed = 10f;
    float throwStrength = 10f;
    int hp = 3;
    Vector2 MousePos;

    bool faceR = false;

    Animator animator;
    [NonSerialized] public GameObject SavedWeapon;
    public GameObject Arm;
    public GameObject ArmRotation;
    public GameObject PickupCollision;
    public GameObject Head;
    [NonSerialized] public bool pickingUp;
    bool isAttacking = false;
    bool isThrowing = false;

    public enum GameState
    {
        Play,
        Pause
    }
    public GameState State = GameState.Play;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (State = GameState.Pause) return;
        if(Input.GetMouseButtonDown(0))
        {
            if(isAttacking) return;
            if(SavedWeapon != null)
            {
                StartCoroutine(Attack());
            }
            else
            {
                StartCoroutine(Headbutt());
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(isThrowing) return;
            if(SavedWeapon != null)
            {
                StartCoroutine(Throw());
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Pickup());
        }

        if(ArmRotation.GetComponent<ArmRotation>().faceR && !faceR)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            faceR = true;
        }
        else if(!ArmRotation.GetComponent<ArmRotation>().faceR && faceR)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            faceR = false;
        }

        if (SavedWeapon != null)
        {
            SavedWeapon.transform.position = Arm.transform.position;
            SavedWeapon.transform.rotation = Arm.transform.rotation;
            SavedWeapon.transform.localScale = ArmRotation.transform.localScale;
        }

    }

    private void FixedUpdate()
    {
        Movement.x = Input.GetAxis("Horizontal");
        Movement.y = Input.GetAxis("Vertical");

        if (Movement == Vector3.zero)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);

        Movement *= 0.9f;
        this.transform.position += Movement * speed * Time.deltaTime;

        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void GetHit(int dmg)
    {
        //knockback
        hp -= dmg;
        if(hp <= 0)
        {
            Debug.Log("ono am ded");
        }
    }

    IEnumerator Attack()
    {
        Arm.GetComponent<Animator>().Play("WeaponAttack");
        SavedWeapon.GetComponent<Weapon>().hitFlag = true;
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        SavedWeapon.GetComponent<Weapon>().hitFlag = false;
        isAttacking = false;
    }

    IEnumerator Headbutt()
    {
        animator.Play("HeadButt");
        Head.GetComponent<PlayerHead>().hitFlag = true;
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        Head.GetComponent<PlayerHead>().hitFlag = false;
        isAttacking = false;
    }

    IEnumerator Throw()
    {
        Arm.GetComponent<Animator>().Play("ThrowWeapon");
        isThrowing = true;
        yield return new WaitForSeconds(0.1f);
        SavedWeapon.GetComponent<Rigidbody2D>().velocity = new Vector2(MousePos.x - this.transform.position.x,MousePos.y - this.transform.position.y).normalized * throwStrength;
        SavedWeapon.GetComponent<Weapon>().hitFlag = true;
        SavedWeapon.GetComponent<Weapon>().isThrown = true;
        SavedWeapon = null;
        isThrowing = false;
    }

    IEnumerator Pickup()
    {
        pickingUp = true;
        yield return new WaitForSeconds(1);
        pickingUp = false;
    }

}
