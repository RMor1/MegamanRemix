using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAndaRewritten : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private bool verRange = false;
    [SerializeField] private float SeguirRange;
    [SerializeField] private float AtaqueRange;
    private int Action;
    private GameObject Player;
    private int MovingDirection=1;
    private float lenght, height;
    private GameObject Aim;
    private Vector2 LastKnowPosition;
    private int vidas=3;
    private enum AiTypeList
    {
        melee, ranged
    }
    [SerializeField] private AiTypeList TipoDeAI;
    void Start()
    {
        LastKnowPosition = new Vector2(0, 0);
        Aim = GameObject.Find("Aim");
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnDrawGizmos()
    {
        if (verRange == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SeguirRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AtaqueRange);
            Gizmos.color = Color.blue;
            if (MovingDirection==1) Gizmos.DrawWireCube(new Vector3(transform.position.x + lenght, transform.position.y), new Vector2(lenght, height + 0.5f));
            else Gizmos.DrawWireCube(new Vector3(transform.position.x - lenght, transform.position.y), new Vector2(lenght, height + 0.5f));
        }
    }
    void Update()
    {
        float DistanceToPlayer;
        DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (DistanceToPlayer < AtaqueRange)
        {
            GetComponent<Animator>().SetBool("ISATTACKING", true);
            GetComponent<Animator>().SetBool("ISWALKING", false);
            Action = 2;
        }
        else if (DistanceToPlayer < SeguirRange)
        {
            GetComponent<Animator>().SetBool("ISWALKING", true);
            Action = 1;
        }
        else
        {
            GetComponent<Animator>().SetBool("ISWALKING", true);
            Action = 0;
        }
    }
    private void FixedUpdate()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (vidas<=0)
        {
            Destroy(gameObject);
        }
        if (TipoDeAI == AiTypeList.ranged)
        {
            //Angulo entre player e Inimigo
            RaycastHit2D LineOfSight = Physics2D.Linecast(transform.position, Player.transform.position);
            if (LineOfSight.collider.CompareTag("Player"))
            {
                Vector2 lookdir = new Vector2(Player.transform.position.x, Player.transform.position.y) - rb.position;
                float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
                Aim.GetComponent<Rigidbody2D>().rotation = angle;
                LastKnowPosition = Player.transform.position;
            }
            else
            {
                Vector2 lookdir = LastKnowPosition - rb.position;
                float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
                Aim.GetComponent<Rigidbody2D>().rotation = angle;
            }
            Aim.transform.position = transform.position;
        }
        rb.velocity = new Vector3(0, 0, 0);
        switch (Action)
        {
            case 2:
                break;
            case 1:
                if (Player.transform.position.x - transform.position.x > 0) MovingDirection = 1;
                else if(Player.transform.position.x - transform.position.x < 0) MovingDirection = 0;
                Move();
                break;
            case 0:
                #region
                bool isThereGround = false;
                bool isThereWall = false;
                RaycastHit2D[] CheckList;
                if (MovingDirection==1) CheckList = Physics2D.BoxCastAll(new Vector3(transform.position.x + lenght, transform.position.y), new Vector2(lenght, height + 0.5f), 0f, new Vector2(0, 0));
                else CheckList = Physics2D.BoxCastAll(new Vector3(transform.position.x - lenght, transform.position.y), new Vector2(lenght, height + 0.5f), 0f, new Vector2(0, 0));
                foreach (RaycastHit2D ObjHit in CheckList)
                {
                    if (ObjHit.collider.CompareTag("Ground")) isThereGround = true;
                    else if (ObjHit.collider.CompareTag("Wall")) isThereWall = true;
                }
                if(isThereGround==true & isThereWall == false)
                {
                    Move();
                }
                else
                {
                    Vira();
                }
                break;
                #endregion
        }
        AnimationSideCorrect();
    }
    void Vira()
    {
        if (MovingDirection == 1) MovingDirection = 0;
        else if (MovingDirection == 0) MovingDirection = 1;
        AnimationSideCorrect();
        Move();
    }
    void AnimationSideCorrect()
    {
        if (MovingDirection == 1) gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (MovingDirection == 0) gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }
    void Move()
    {
        if (MovingDirection==1) rb.MovePosition(transform.position + new Vector3(1, 0, 0) * MoveSpeed * Time.fixedDeltaTime);
        else rb.MovePosition(transform.position + new Vector3(-1, 0, 0) * MoveSpeed * Time.fixedDeltaTime);
    }
    void AlertOnAnimator(string AnimationName)
    {
        if (AnimationName.Equals("AttackStart"))
        {
            Aim.GetComponent<BoxCollider2D>().enabled = true;
            //hitbox turn on
        }
        else if (AnimationName.Equals("AttackEnd"))
        {
            //hitbox turn off 
            Aim.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetBool("ISATTACKING", false);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        vidas--;
    }
}
