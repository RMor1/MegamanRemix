using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlV2 : MonoBehaviour
{
    public Animator anima;
    float xmov;
    public Rigidbody2D rdb;
    public ParticleSystem fire;
    [Range(0, 20)] public float MoveSpeed = 20;
    [Range(1f, 1.2f)] public float JumpForce = 1;
    private float LastTime;
    public float ShootCooldown;
    [Range(0, 5)] public int vida;
    private float timer;
    public GameObject playerprefab;
    private GameObject respawn;

    [Range(0, 50), SerializeField] private float JumpSpeed;
    private float JumpTimeCounter;
    [SerializeField] public float jumpTime;
    private RaycastHit2D[] GroundCheck;
    private bool isJumping;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    bool lastisGrounded;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    }
    void Start()
    {
        respawn = GameObject.Find("Respawn");
    }
    void Update()
    {
        if (vida <= 0)
        {
            vida = 5;
            Instantiate(playerprefab, respawn.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        xmov = Input.GetAxis("Horizontal");
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
        if(Mathf.Round(rdb.velocity.y)<0)
        {
            anima.SetBool("Falling",true);
        }
        else
        {
            anima.SetBool("Falling",false);
        }
        anima.SetBool("Fire", false);
        LastTime += Time.fixedDeltaTime;
        if (Input.GetButtonDown("Fire1") && LastTime > ShootCooldown)
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
        rdb.AddForce(new Vector2(xmov * MoveSpeed / (rdb.velocity.magnitude + 1), 0));
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
        if ((Time.time - timer) > 2)
        {
            timer = Time.time;
            vida--;
        }
    }
}
