using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ControlRigV2 : MonoBehaviour
{
    private GameObject Player;
    private GameObject[] ChildList;
    [SerializeField] private GameObject rigParent;
    private GameObject[] rigList;
    private int ChildCount = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool SpawnSequence;
    private int SpawnAction;
    [SerializeField] private bool verRange;
    [SerializeField] private float AtaqueRange;
    [SerializeField] private float detectRange;
    private bool detected = false;
    private Vector3 Center;
    private GameObject bodyBase;
    private float weight;
    private bool move = false;
    private Vector3 DeltaPosition;
    private float delay;
    [SerializeField] private GameObject projectilePrefab;
    private Transform firePoint;
    [SerializeField] private float shootCooldown;
    public int vidas = 3;
    void Start()
    {
        if (GetComponentInChildren<Transform>().GetChild(0).transform.gameObject.transform.childCount > 0)
        {
            int ListTamanho = 0;
            bool repeat = true;
            GameObject GObj = GetComponentInChildren<Transform>().GetChild(0).transform.gameObject;
            while (repeat == true)
            {
                if (GObj.transform.childCount > 0)
                {
                    if (GObj.transform.GetChild(0).GetComponent<Rigidbody2D>() != null)
                    {
                        ListTamanho++;
                        GObj = GObj.GetComponentInChildren<Transform>().GetChild(0).transform.gameObject;
                    }
                    else repeat = false;
                }
                else
                {
                    repeat = false;
                }
            }
            repeat = true;
            ChildList = new GameObject[ListTamanho + 1];//Limite de Objetos filhos+ a ele mesmo 
            ChildList[0] = GetComponentInChildren<Transform>().GetChild(0).transform.gameObject;
            while (repeat == true)
            {
                if (ChildList[ChildCount].transform.childCount > 0)
                {
                    if (ChildList[ChildCount].GetComponentInChildren<Transform>().GetChild(0).GetComponent<Rigidbody2D>() != null)
                    {
                        ChildCount++;
                        ChildList[ChildCount] = ChildList[ChildCount - 1].GetComponentInChildren<Transform>().GetChild(0).transform.gameObject;
                    }
                    else repeat = false;
                }
                else
                {
                    repeat = false;
                }
            }
        }
        rigList = new GameObject[rigParent.transform.childCount - 1];
        for (int i = 0; i < rigParent.transform.childCount - 1; i++)
        {
            rigList[i] = rigParent.transform.GetChild(i).transform.gameObject;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        bodyBase = ChildList[0];
        Center = ChildList[0].transform.position;
        weight = rigList[0].GetComponent<DampedTransform>().weight;
        if (SpawnSequence)
        {
            foreach (GameObject go in ChildList)
            {
                go.GetComponent<SpriteRenderer>().enabled = false;
                go.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        firePoint = ChildList[ChildCount].GetComponentInChildren<Transform>();
    }
    private void OnDrawGizmos()
    {
        if (verRange == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Center, AtaqueRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Center, AtaqueRange * 1.2f);
            for (int i = 1; i <= ChildCount; i++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(Center, i * (AtaqueRange / ChildCount));
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Center, detectRange);
        }
    }
    void FixedUpdate()
    {
        if(vidas <=0)
        {
            Destroy(GameObject.Find("AiLacraia"));
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        float DistanceToPlayer;
        DistanceToPlayer = Vector2.Distance(Center, Player.transform.position);
        if (SpawnSequence == false)
        {
            int OutputBodyAmount = -1;
            if (DistanceToPlayer < AtaqueRange * 1.2f)
            {
                ChildList[ChildCount].GetComponent<Animator>().SetBool("Attacking", true);
                for (int i = 0; i <= ChildCount; i++)
                {
                    if (DistanceToPlayer < (1 + i) * (AtaqueRange / ChildCount))
                    {
                        OutputBodyAmount = ChildCount - i;
                        break;
                    }
                }
                if (bodyBase != ChildList[OutputBodyAmount])
                {
                    bodyBase = ChildList[OutputBodyAmount];
                    move = true;
                }
                if (move)
                {
                    if (Vector2.Distance(bodyBase.transform.position, Center) > 0.05)
                    {
                        for (int i = 0; i < ChildCount; i++)
                        {
                            ChildList[i].GetComponent<Animator>().SetBool("Moving", true);
                        }
                        bodyBase.GetComponent<Rigidbody2D>().MovePosition(bodyBase.transform.position + (Center - bodyBase.transform.position) * moveSpeed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        for (int i = 0; i < ChildCount; i++)
                        {
                            ChildList[i].GetComponent<Animator>().SetBool("Moving", false);
                        }
                        move = false;
                    } 
                }
                for (int i = 0; i < OutputBodyAmount; i++)
                {
                    rigList[i].GetComponent<DampedTransform>().weight = 0;
                }
                for (int i = rigParent.transform.childCount - 2; i >= OutputBodyAmount; i--)
                {
                    rigList[i].GetComponent<DampedTransform>().weight = weight;
                }
                for (int i = 0; i < OutputBodyAmount; i++)
                {
                    ChildList[i].GetComponent<BoxCollider2D>().enabled = false;
                    ChildList[i].GetComponent<SpriteRenderer>().enabled = false;
                }
                for (int i = ChildCount; i >= OutputBodyAmount; i--)
                {
                    ChildList[i].GetComponent<BoxCollider2D>().enabled = true;
                    ChildList[i].GetComponent<SpriteRenderer>().enabled = true;
                }
                delay += Time.fixedDeltaTime;
                if (delay> shootCooldown)
                {
                    GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(firePoint.right*-6, ForceMode2D.Impulse);
                    delay = 0;
                }
            }
            else ChildList[ChildCount].GetComponent<Animator>().SetBool("Attacking", false);
        }
        else if (SpawnSequence == true)
        {
            if (DistanceToPlayer < detectRange)
            {
                detected = true;
            }
            if (detected == true)
            {
                switch (SpawnAction)
                {
                    case 0:
                        if (Vector2.Distance(bodyBase.transform.position, Center) < bodyBase.GetComponent<SpriteRenderer>().bounds.size.x * ChildCount & SpawnAction == 0)
                        {
                            bodyBase.GetComponent<Rigidbody2D>().MovePosition(bodyBase.transform.position - (Player.transform.position - bodyBase.transform.position) * 10 * Time.fixedDeltaTime);
                        }
                        else
                        {
                            SpawnAction = 1;
                        }
                        break;
                    case 1:
                        delay += Time.fixedDeltaTime;
                        if (Vector2.Distance(bodyBase.transform.position, Center)>0.05)
                        {
                            DeltaPosition = Center - bodyBase.transform.position;
                            CorrecaoDeDirecao();
                            bodyBase.GetComponent<Rigidbody2D>().MovePosition(bodyBase.transform.position + DeltaPosition * moveSpeed * Time.fixedDeltaTime);
                        }
                        else SpawnSequence = false;
                        if(delay>1)
                        {
                            foreach (GameObject go in ChildList)
                            {
                                if (go.transform.position.x-Center.x< 1)
                                {
                                    go.GetComponent<SpriteRenderer>().enabled = true;
                                    go.GetComponent<BoxCollider2D>().enabled = true;
                                }
                            }
                        }
                        break;
                }
            }
        }
        Vector2 lookdir = new Vector2(Player.transform.position.x, Player.transform.position.y) - bodyBase.GetComponent<Rigidbody2D>().position;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 180;
        if(bodyBase.GetComponent<Rigidbody2D>().rotation != angle)
        {
            for(int i=0;i<ChildCount;i++)
            {
                ChildList[i].GetComponent<Animator>().SetBool("Moving", true);
            }
            bodyBase.GetComponent<Rigidbody2D>().rotation = angle;
        }
        else
        {
            for (int i = 0; i < ChildCount; i++)
            {
                ChildList[i].GetComponent<Animator>().SetBool("Moving", false);
            }
        }
    }
    void CorrecaoDeDirecao()
    {
        bool IsXNegative = false;
        bool IsYNegative = false;
        if (DeltaPosition.x < 0) IsXNegative = true;
        if (DeltaPosition.y < 0) IsYNegative = true;
        DeltaPosition.x = Mathf.Abs(DeltaPosition.x);
        DeltaPosition.y = Mathf.Abs(DeltaPosition.y);
        float XDecimal = DeltaPosition.x / (DeltaPosition.x + DeltaPosition.y);
        float YDecimal = DeltaPosition.y / (DeltaPosition.x + DeltaPosition.y);
        if (IsXNegative == true)
        {
            XDecimal = (XDecimal) * -1;
            IsXNegative = false;
        }
        if (IsYNegative == true)
        {
            YDecimal = (YDecimal) * -1;
            IsYNegative = false;
        }
        DeltaPosition.x = XDecimal;
        DeltaPosition.y = YDecimal;
    }
}
