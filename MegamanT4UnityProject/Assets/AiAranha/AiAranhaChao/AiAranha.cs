using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAranha : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 sizesAranha;
    private SpriteRenderer spriteRenderer;
    [Header("Atributos")]
    [SerializeField] private int vida;
    [Header("Movimentação")]
    [SerializeField] private float MoveSpeed;
    private bool attacking;
    private int direcao = -1;

    private bool canShoot = true;
    [Header("Ataque")]
    [SerializeField] private bool verRange;
    [SerializeField] private float AtaqueRange;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float projectileForce;
    private GameObject Player;

    private float DistanceToPlayer = 1000f;
    private bool CheckPlayerCooldown;

    [SerializeField] private GameObject teiaPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        sizesAranha = GetComponent<SpriteRenderer>().bounds.size;
    }
    private void OnDrawGizmos()
    {
        if (verRange == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AtaqueRange);
        }
    }
    void FixedUpdate()
    {
        bool isThereGround = false;
        bool isThereWall = false;
        RaycastHit2D[] CheckList;
        rb.velocity = new Vector2(0, 0);
        if (CheckPlayerCooldown) DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if(attacking==false)
        {
            if (DistanceToPlayer < AtaqueRange)
            {
                if (canShoot)
                {
                    if (Mathf.RoundToInt((Player.transform.position.x - transform.position.x) / Mathf.Abs(Player.transform.position.x - transform.position.x)) == direcao)
                    {
                        GetComponent<Animator>().SetTrigger("Fire");
                    }
                    else
                    {
                        GetComponent<Animator>().SetTrigger("Fire2");
                    }
                }
                else
                {
                    Move();
                }
            }
            else
            {
                if (direcao == 1) CheckList = Physics2D.BoxCastAll(new Vector3(transform.position.x + sizesAranha.x, transform.position.y), new Vector2(sizesAranha.x, sizesAranha.y + 0.5f), 0f, new Vector2(0, 0));
                else CheckList = Physics2D.BoxCastAll(new Vector3(transform.position.x - sizesAranha.x, transform.position.y), new Vector2(sizesAranha.x, sizesAranha.y + 0.5f), 0f, new Vector2(0, 0));
                foreach (RaycastHit2D ObjHit in CheckList)
                {
                    if (ObjHit.collider.CompareTag("Ground")) isThereGround = true;
                    else if (ObjHit.collider.CompareTag("Wall")) isThereWall = true;
                }
                if (isThereGround == true & isThereWall == false)
                {
                    Move();
                }
                else Vira();
            }
        }
        StartCoroutine(CheckPlayer());
    }
    IEnumerator ShootcooldownCounter()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
    IEnumerator CheckPlayer()
    {
        CheckPlayerCooldown = false;
        yield return new WaitForSeconds(0.1f);
        CheckPlayerCooldown = true;
    }
    public void AnimationEvents(string tipo)
    {
        if (tipo == "AnimationStart")
        {
            attacking = true;
        }
        else if (tipo == "AttackEnd")
        {
            Attack();
            Vira();
        }
        else if (tipo == "Attack2Start") attacking = true;
        else if (tipo == "Attack2End")
        {
            Attack();
        }

    }
    private void Attack()
    {
        int playerSide = Mathf.RoundToInt((Player.transform.position.x - transform.position.x) / Mathf.Abs(Player.transform.position.x - transform.position.x));
        Vector3 spawnPosition = transform.position + new Vector3(playerSide * sizesAranha.x / 2, sizesAranha.y/2, 0);
        GameObject teia = Instantiate(teiaPrefab, spawnPosition, Quaternion.Euler(0, 0, 0));
        if (playerSide == -1) teia.GetComponent<SpriteRenderer>().flipX = true;
        teia.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerSide, 0) * projectileForce);
        attacking = false;
        StartCoroutine(ShootcooldownCounter());
    }
    private void Move()
    {
        rb.MovePosition(new Vector2(transform.position.x + MoveSpeed * Time.fixedDeltaTime * direcao, transform.position.y));
    }
    private void Vira()
    {
        if (direcao == 1)
        {
            spriteRenderer.flipX = false;
            direcao = -1;
        }
        else
        {
            spriteRenderer.flipX = true;
            direcao = 1;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        vida--;
        if (vida <= 0) Destroy(gameObject);
    }
}
