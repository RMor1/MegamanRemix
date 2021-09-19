using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneNPCScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    private float lenght, height;
    bool jump;[SerializeField] float jumptime; float fixedJumpTime;
    Animator animator;
    void Start()
    {
        
        fixedJumpTime = jumptime;
        rb = GetComponent<Rigidbody2D>();
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        animator = GetComponent<Animator>();
        animator.SetTrigger("TriggerCutscene");
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x - lenght / 2, transform.position.y+height/2), new Vector2(transform.position.x - lenght*3, transform.position.y+ height/2));
    }
    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MineradorAndando")) rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        RaycastHit2D[] frontHit = Physics2D.LinecastAll(new Vector2(transform.position.x - lenght / 2, transform.position.y + height / 2), new Vector2(transform.position.x - lenght * 3, transform.position.y + height / 2));
        foreach (RaycastHit2D go in frontHit)
        {
            if (go.collider.name == "estalactiteWall2")
            {
                animator.SetTrigger("TriggerIdle");
            }
            else if (jump == false & go.collider.name== "colide")
            {
                jump = true;
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
    void AnimationFunction(string function)
    {
        if(function=="EndIdle")
        {
            animator.ResetTrigger("TriggerIdle");
        }
    }
}
