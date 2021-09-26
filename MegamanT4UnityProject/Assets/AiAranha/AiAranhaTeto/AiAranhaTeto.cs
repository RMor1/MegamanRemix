using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAranhaTeto : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public int vida;
    [SerializeField] private float moveSpeed;
    public bool playerDetected;
    private Vector2 aranhaSizes;
    private bool Attacking;
    [SerializeField] private bool triggered;


    private GameObject teia;
    private Vector2 teiaSizes;
    private float minHeight, maxHeight;
    private bool returning;
    [SerializeField] private bool bossFight;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        teia = transform.parent.GetChild(2).gameObject;
        aranhaSizes = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        teiaSizes = new Vector2(teia.GetComponent<SpriteRenderer>().bounds.size.x, teia.GetComponent<SpriteRenderer>().bounds.size.y);
        animator = GetComponent<Animator>();
        if (bossFight)
        {
            teia.transform.position = teia.transform.position - new Vector3(0, teiaSizes.y / 2, 0);
        }
        transform.position = teia.transform.position + new Vector3(0,
            teiaSizes.y / 2 - aranhaSizes.y / 2
            , 0);
        minHeight = transform.position.y;
        maxHeight = teia.transform.position.y - teiaSizes.y / 2 + aranhaSizes.y / 2;
    }
    void FixedUpdate()
    {
        if (playerDetected && returning==false)
        {
            Attacking = true;
        }
        if (Attacking)
        {
            animator.SetBool("Moving", true);
            if (Mathf.Abs(maxHeight - transform.position.y) <= 0.05f || triggered==true)
            {
                animator.SetTrigger("Attacking");
                returning = true;
                Attacking = false;
            }
            rb.MovePosition(transform.position - new Vector3(0, 1, 0) * moveSpeed * Time.fixedDeltaTime);
        }
        else if (returning)
        {
            animator.SetBool("Moving", true);
            rb.MovePosition(transform.position + new Vector3(0, 1, 0) * moveSpeed/2 * Time.fixedDeltaTime);
            if (Mathf.Abs(minHeight - transform.position.y) <= 0.05f)
            {
                returning = false;
            }
        }
        else animator.SetBool("Moving", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        { 
            triggered = true;
            collision.GetComponent<ControlV2>().Damage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) triggered = false;
    }
    private void OnParticleTrigger()
    {
        vida--;
        if (vida <= 0) Destroy(gameObject);
    }
}
