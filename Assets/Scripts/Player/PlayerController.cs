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
    float speed = 7.5f;
    float throwStrength = 10f;
    int hp = 3;
    Vector2 MousePos;

    bool faceR = false;

    Animator animator;
    [NonSerialized] public GameObject SavedWeapon;
    public GameObject Sprite;
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
        animator = Sprite.GetComponent<Animator>();
    }

    void Update()
    {
        if(State == GameState.Pause) return;
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
            Sprite.GetComponent<SpriteRenderer>().flipX = true;
            faceR = true;
            if(SavedWeapon != null)
                SavedWeapon.transform.localScale = new Vector3(-1,-1,1);
        }
        else if(!ArmRotation.GetComponent<ArmRotation>().faceR && faceR)
        {
            Sprite.GetComponent<SpriteRenderer>().flipX = false;
            faceR = false;
            if (SavedWeapon != null)
                SavedWeapon.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (SavedWeapon != null)
        {
            SavedWeapon.transform.position = Arm.transform.position;
            SavedWeapon.transform.rotation = Arm.transform.rotation;
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

        this.GetComponent<Rigidbody2D>().velocity *= 0.9f;
        this.transform.position += Movement * speed * Time.deltaTime;

        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void GetHit(int dmg, Vector2 kb)
    {
        this.GetComponent<Rigidbody2D>().velocity = kb;
        animator.Play("PlayerHurt");
        hp -= dmg;
        if(hp <= 0)
        {
            Debug.Log("ono am ded");
        }
    }

    IEnumerator Attack()
    {
        SavedWeapon.GetComponent<Weapon>().hitFlag = true;
        Arm.GetComponent<Animator>().Play("WeaponAttack");
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        SavedWeapon.GetComponent<Weapon>().hitFlag = false;
        isAttacking = false;
    }

    IEnumerator Headbutt()
    {
        Head.GetComponent<PlayerHead>().hitFlag = true;
        if(faceR)
            animator.Play("HeadButtR");
        else
            animator.Play("HeadButtL");
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
        SavedWeapon.GetComponent <Weapon>().hitFlag = true;
        SavedWeapon.GetComponent<Weapon>().Throw();
        SavedWeapon.GetComponent<Rigidbody2D>().velocity = new Vector2(MousePos.x - this.transform.position.x,MousePos.y - this.transform.position.y).normalized * throwStrength;
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
