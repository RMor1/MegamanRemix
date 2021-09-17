using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleNPC : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private bool verGizmos = false;
    private int MovingDirection = 1;
    private float lenght, height;

    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        if (verGizmos == true)
        {
            Gizmos.color = Color.blue;
            if (MovingDirection == 1) Gizmos.DrawWireCube(new Vector3(transform.position.x + lenght, transform.position.y), new Vector2(lenght, height + 0.5f));
            else Gizmos.DrawWireCube(new Vector3(transform.position.x - lenght, transform.position.y), new Vector2(lenght, height + 0.5f));
        }
    }
    void Start()
    {
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region
        bool isThereGround = false;
        bool isThereWall = false;
        RaycastHit2D[] CheckList;
        if (MovingDirection == 1) CheckList = Physics2D.BoxCastAll(new Vector3(transform.position.x + lenght, transform.position.y), new Vector2(lenght, height + 0.5f), 0f, new Vector2(0, 0));
        else CheckList = Physics2D.BoxCastAll(new Vector3(transform.position.x - lenght, transform.position.y), new Vector2(lenght, height + 0.5f), 0f, new Vector2(0, 0));
        foreach (RaycastHit2D ObjHit in CheckList)
        {
            if (ObjHit.collider.CompareTag("Ground")) isThereGround = true;
            else if (ObjHit.collider.CompareTag("Wall")) isThereWall = true;
        }
        if (isThereGround == true & isThereWall == false)
        {
            Move();
        }
        else
        {
            Vira();
        }
        #endregion

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
        if (MovingDirection == 1) rb.MovePosition(transform.position + new Vector3(1, 0, 0) * MoveSpeed * Time.fixedDeltaTime);
        else rb.MovePosition(transform.position + new Vector3(-1, 0, 0) * MoveSpeed * Time.fixedDeltaTime);
    }
}
