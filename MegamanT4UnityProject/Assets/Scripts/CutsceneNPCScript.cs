using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneNPCScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    private float lenght;
    bool jump;[SerializeField] float jumptime; float fixedJumpTime;
    void Start()
    {
        fixedJumpTime = jumptime;
        rb = GetComponent<Rigidbody2D>();
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x - lenght / 2, transform.position.y), new Vector2(transform.position.x - lenght*3, transform.position.y));
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        RaycastHit2D[] frontHit = Physics2D.LinecastAll(new Vector2(transform.position.x - lenght / 2, transform.position.y), new Vector2(transform.position.x - lenght * 3, transform.position.y));
        foreach (RaycastHit2D go in frontHit)
        {
            if (go.collider.name == "EstalactiteParede")
            {

            }
            else if (jump == false & go.collider.name== "colide")
            {
                jump = true;
                Debug.Log(go.collider.name);
            }
        }
        if (jump)
        {
            if (jumptime > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, Vector2.up.y * 3);
                jumptime -= Time.fixedDeltaTime;
            }
            else
            {
                jumptime = fixedJumpTime;
                jump = false;
            }
        }
    }

}
