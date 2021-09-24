﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlV2 : MonoBehaviour
{
    [Header("Segmentos")]
    public Animator anima;
    float xmov;
    public Rigidbody2D rdb;
    public ParticleSystem fire;
    public GameObject playerprefab;


    [Header("Movimentação e Atributos")]
    [Range(0, 20)] public float MoveSpeed = 20;
    [Range(1f, 1.2f)] public float JumpForce = 1;
    private float LastTime;
    public float ShootCooldown;
    [Range(0, 5)] public int vida;
    private float timer;
    [Range(0, 50), SerializeField] private float JumpSpeed;
    [SerializeField] public float jumpTime;
    private float JumpTimeCounter;
    private RaycastHit2D[] GroundCheck;
    private bool isJumping;

    [Header("Jump Configs")]
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    bool lastisGrounded;

    [Header("Cutscenes")]
    [SerializeField] bool onCutscene;

    [Header("Debuffs")]
    [SerializeField] bool isSlowed;
    [SerializeField] private float slowAmountPercent;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    }
    void Start()
    {
    }
    void Update()
    {
        if (vida <= 0)
        {
            vida = 5;
            GameObject respawn = GameObject.Find("Respawn");
            Instantiate(playerprefab, respawn.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (onCutscene == false)
        {
            xmov = Input.GetAxis("Horizontal");
        }
        else
            xmov = 0;
        bool isGrounded = false;
        GroundCheck = Physics2D.CircleCastAll(feetPos.position, checkRadius, new Vector2(0, 0));
        foreach (RaycastHit2D go in GroundCheck)
        {
            if (go.collider.CompareTag("Ground")) isGrounded = true;
        }
        if (isGrounded == true & lastisGrounded == false)
        {
            anima.SetTrigger("HitGround");
        }
        else if (isGrounded == true & lastisGrounded == true)
        {
            anima.SetTrigger("HitGround");
        }
        else
        {
            anima.ResetTrigger("HitGround");
        }
        lastisGrounded = isGrounded;
        if(onCutscene==false)
        {
            if (isGrounded == true && Input.GetKeyDown(KeyCode.Space) && Mathf.Round(rdb.velocity.y) == 0)
            {
                anima.SetTrigger("Jump");
                isJumping = true;
                JumpTimeCounter = jumpTime;
                rdb.velocity = new Vector2(rdb.velocity.x, Vector2.up.y * JumpSpeed);
            }
            if (Input.GetKey(KeyCode.Space) && isJumping == true)
            {
                if (JumpTimeCounter > 0)
                {
                    rdb.velocity = new Vector2(rdb.velocity.x, Vector2.up.y * JumpSpeed);
                    JumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }
        if (Mathf.Round(rdb.velocity.y) < 0)
        {
            anima.SetBool("Falling", true);
        }
        else
        {
            anima.SetBool("Falling", false);
        }
        anima.SetBool("Fire", false);
        LastTime += Time.fixedDeltaTime;
        if (Input.GetButtonDown("Fire1") && LastTime > ShootCooldown && onCutscene==false)
        {
            fire.Emit(1);
            anima.SetBool("Fire", true);
            LastTime = 0;
        }

    }

    void FixedUpdate()
    {
        Reverser();
        anima.SetFloat("Velocity", Mathf.Abs(xmov));
        float playerVelocity = xmov * MoveSpeed / (rdb.velocity.magnitude + 1);
        if(!isSlowed) rdb.AddForce(new Vector2(playerVelocity, 0));
        else rdb.AddForce(new Vector2(playerVelocity- playerVelocity*slowAmountPercent/100, 0));
        if (anima.GetCurrentAnimatorStateInfo(0).IsName("JumpFly"))
        {
            float ZRotation;
            ZRotation = Mathf.Abs(rdb.velocity.y) / (Mathf.Abs(rdb.velocity.x * 4) + Mathf.Abs(rdb.velocity.y)) * -90;
            if (-90 < ZRotation & ZRotation <= 0)
            {
                ZRotation = ZRotation - anima.gameObject.transform.eulerAngles.z;
                if (ZRotation == float.NaN) ZRotation = 0;
                anima.gameObject.transform.Rotate(0, 0, ZRotation);
            }
        }
        else
        {
            anima.gameObject.transform.Rotate(-anima.gameObject.transform.eulerAngles.x, 0, -anima.gameObject.transform.eulerAngles.z, Space.Self);
        }
    }
    void Reverser()
    {
        if (xmov > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (xmov < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }
    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        if (rdb.velocity.x < 0.1f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Explosion")) Damage();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitBox")) Damage();
    }
    public void Damage()
    {
        if ((Time.time - timer) > 2 && onCutscene==false)
        {
            timer = Time.time;
            vida--;
        }
    }
}
