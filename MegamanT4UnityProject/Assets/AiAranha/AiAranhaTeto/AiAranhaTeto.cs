using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAranhaTeto : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    public bool playerDetected;
    private Vector2 aranhaSizes;
    private bool Attacking;
    [SerializeField] private bool triggered;


    private GameObject teia;
    private Vector2 teiaSizes;
    private float minHeight, maxHeight;
    private bool returning;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        teia = transform.parent.GetChild(2).gameObject;
        aranhaSizes = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        teiaSizes = new Vector2(teia.GetComponent<SpriteRenderer>().bounds.size.x, teia.GetComponent<SpriteRenderer>().bounds.size.y);
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
            if (Mathf.Abs(maxHeight - transform.position.y) <= 0.05f || triggered==true)
            {
                if (triggered == true) Debug.Log(triggered);
                else Debug.Log(Mathf.Abs(maxHeight - transform.position.y));
                returning = true;
                Attacking = false;
                Debug.Log(Attacking);
            }
            rb.MovePosition(transform.position - new Vector3(0, 1, 0) * moveSpeed * Time.fixedDeltaTime);
        }
        else if (returning)
        {
            rb.MovePosition(transform.position + new Vector3(0, 1, 0) * moveSpeed/2 * Time.fixedDeltaTime);
            if (Mathf.Abs(minHeight - transform.position.y) <= 0.05f)
            {
                returning = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) triggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) triggered = false;
    }
}
