using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

public class BossScript : MonoBehaviour
{
    private GameObject Player;
    private GameObject[] ChildList;
    private GameObject rigParent;
    private GameObject[] rigList;
    private int ChildCount = 0;
    [Header("Movimentação")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool leaveHole;
    private bool SpawnSequence = true;
    private int SpawnAction;
    [Header("Ataque")]
    [SerializeField] private bool verRange;
    [SerializeField] private float AtaqueRange;
    private Vector3 Center;
    private GameObject bodyBase;
    private float weight;
    private bool move = false;
    private Vector3 DeltaPosition;
    private float delay;
    [SerializeField] private GameObject projectilePrefab;
    private Transform firePoint;
    [SerializeField] private float shootCooldown;
    public int vida;
    private int maxHp;
    private bool ExitSequence = false;
    private Vector2 sizes;
    [Header("Buracos")]
    [SerializeField] private Transform[] holesTransform = new Transform[4];
    private int holePos;
    [Header("Inimigos Prefabs")]
    [SerializeField] private GameObject rato;
    [SerializeField] private GameObject morcegoLeft2Right;
    [SerializeField] private GameObject morcegoRight2Left;
    [SerializeField] private GameObject aranhaTeto;
    [SerializeField] private GameObject aranhaChao;
    [Header("SpawnInimigos")]
    [SerializeField] private Transform[] portasTransform = new Transform[2];
    [SerializeField] private Transform[] TetoTransform = new Transform[2];
    [SerializeField] private Transform[] morcegoTransform = new Transform[2];

    private GameObject[] spawnedEnemies = new GameObject[4];
    private bool isEnemiesSpawned;
    private bool changeHole = true;

    public bool cutscene;
    void Start()
    {
        maxHp = vida;
        rigParent = transform.parent.GetChild(1).gameObject;
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
        Player = GameObject.Find("Player");
        bodyBase = ChildList[0];
        ChildList[0].transform.position = holesTransform[0].position;
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
        sizes = new Vector2(bodyBase.GetComponent<SpriteRenderer>().bounds.size.x, bodyBase.GetComponent<SpriteRenderer>().bounds.size.y);
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
        }
    }
    void FixedUpdate()
    {
        switch (holePos)
        {
            case 0:
            case 1:
                if (vida <= 0)
                {
                    ExitSequence = true;
                }
                break;
            case 2:
            case 3:
                bool isThereIsGameObject = false;
                foreach (GameObject go in spawnedEnemies)
                {
                    if (go != null) isThereIsGameObject = true;
                }
                if (vida <= 0)
                {
                    ExitSequence = true;
                }
                if (!isThereIsGameObject && vida <= 0)
                {
                    changeHole = true;
                }
                break;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        float DistanceToPlayer;
        DistanceToPlayer = Vector2.Distance(Center, Player.transform.position);
        if (leaveHole)
        {
            if (ExitSequence)
            {
                if (Vector2.Distance(bodyBase.transform.position, Center) < sizes.x * ChildCount * 1.2f)
                {
                    DeltaPosition = Player.transform.position - bodyBase.transform.position;
                    CorrecaoDeDirecao();
                    bodyBase.GetComponent<Rigidbody2D>().MovePosition(bodyBase.transform.position - DeltaPosition * moveSpeed * Time.fixedDeltaTime);
                    foreach (GameObject go in ChildList)
                    {
                        if (Vector2.Distance(go.transform.position, Center) < 0.5)
                        {
                            go.GetComponent<SpriteRenderer>().enabled = false;
                            go.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }
                else if (changeHole)
                {
                    if (holePos != 3)
                    {
                        holePos++;
                        vida = maxHp;
                        foreach (GameObject go in rigList)
                        {
                            go.GetComponent<DampedTransform>().weight = weight;
                        }
                        bodyBase = ChildList[0];
                        bodyBase.GetComponent<Rigidbody2D>().MovePosition(holesTransform[holePos].transform.position);
                        ChangeCenter(holePos);
                        SpawnAction = 0;
                        SpawnSequence = true;
                        ExitSequence = false;
                        delay = 0;
                        switch (holePos)
                        {
                            case 1:
                                leaveHole = false;
                                StartCoroutine(SpawnEnemies(holePos - 1));
                                break;
                            case 2:
                                changeHole = false;
                                StartCoroutine(SpawnEnemies(holePos - 1));
                                break;
                            case 3:
                                changeHole = false;
                                StartCoroutine(SpawnEnemies(holePos - 1));
                                break;
                        }
                    }
                    else
                    {
                        //se tiver todos monstros mortos
                        //foge e depois se apaga
                        //else espera ate morrerem 
                        Destroy(transform.parent.gameObject);
                    }
                }
            }
            else
            {
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
                        if (delay > shootCooldown)
                        {
                            GameObject[] bullets = new GameObject[3];
                            bullets[0] = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                            bullets[0].transform.Rotate(0, 0, 45);
                            bullets[1] = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                            bullets[1].transform.Rotate(0, 0, 0);
                            bullets[2] = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                            bullets[2].transform.Rotate(0, 0, -45);
                            bullets[0].GetComponent<Rigidbody2D>().AddForce((firePoint.right + firePoint.up) / 2 * -1 * 6, ForceMode2D.Impulse);
                            bullets[1].GetComponent<Rigidbody2D>().AddForce(firePoint.right * -1 * 6, ForceMode2D.Impulse);
                            bullets[2].GetComponent<Rigidbody2D>().AddForce((firePoint.right + firePoint.up * -1) / 2 * -1 * 6, ForceMode2D.Impulse);
                            delay = 0;
                        }
                    }
                    else ChildList[ChildCount].GetComponent<Animator>().SetBool("Attacking", false);
                }
                else if (SpawnSequence == true)
                {
                    switch (SpawnAction)
                    {
                        case 0:
                            if (Vector2.Distance(bodyBase.transform.position, Center) < sizes.x * 2 * ChildCount)
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
                            if (Vector2.Distance(bodyBase.transform.position, Center) > 0.05)
                            {
                                DeltaPosition = Center - bodyBase.transform.position;
                                CorrecaoDeDirecao();
                                bodyBase.GetComponent<Rigidbody2D>().MovePosition(bodyBase.transform.position + DeltaPosition * moveSpeed * Time.fixedDeltaTime);
                            }
                            else SpawnSequence = false;
                            if (delay > 1)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (Vector2.Distance(ChildList[i].transform.position, Center) < 1)
                                    {
                                        for (int y = i; y < 4; y++)
                                        {
                                            ChildList[y].GetComponent<SpriteRenderer>().enabled = true;
                                            ChildList[y].GetComponent<BoxCollider2D>().enabled = true;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
        else if (isEnemiesSpawned)
        {
            bool isThereIsGameObject = false;
            switch (holePos)
            {
                case 1:
                    foreach (GameObject go in spawnedEnemies)
                    {
                        if (go != null) isThereIsGameObject = true;
                    }
                    break;
            }
            if (!isThereIsGameObject)
            {
                isEnemiesSpawned = false;
                leaveHole = true;
            }
        }
        Vector2 lookdir;
        lookdir = new Vector2(Player.transform.position.x, Player.transform.position.y) - bodyBase.GetComponent<Rigidbody2D>().position;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 180;
        if (bodyBase.GetComponent<Rigidbody2D>().rotation != angle)
        {
            for (int i = 0; i < ChildCount; i++)
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
    private IEnumerator SpawnEnemies(int bossFightFase)
    {
        switch (bossFightFase)
        {
            case 0:
                Debug.Log("spawn 1");
                //instantiate aranha teto 
                spawnedEnemies[0] = Instantiate(aranhaTeto, TetoTransform[0].position, TetoTransform[0].rotation);
                //instantiate rato 
                spawnedEnemies[1] = Instantiate(rato, portasTransform[0].position, portasTransform[0].rotation);
                yield return new WaitForSeconds(1);
                //instantiate aranha teto 
                spawnedEnemies[2] = Instantiate(aranhaTeto, TetoTransform[1].position, TetoTransform[1].rotation);
                //instantiate rato 
                spawnedEnemies[3] = Instantiate(rato, portasTransform[1].position, portasTransform[1].rotation);
                isEnemiesSpawned = true;
                break;
            case 1:
                Debug.Log("spawn 2");
                //instantiate morcego 
                spawnedEnemies[0] = Instantiate(morcegoRight2Left, morcegoTransform[0].position, TetoTransform[0].rotation);
                //instantiate rato 
                spawnedEnemies[1] = Instantiate(rato, portasTransform[0].position, portasTransform[0].rotation);
                yield return new WaitForSeconds(1);
                //instantiate morcego
                spawnedEnemies[2] = Instantiate(morcegoLeft2Right, morcegoTransform[1].position, TetoTransform[1].rotation);
                //instantiate rato 
                spawnedEnemies[3] = Instantiate(rato, portasTransform[1].position, portasTransform[1].rotation);
                break;
            case 2:
                Debug.Log("spawn 3");
                //instantiate morcego 
                spawnedEnemies[0] = Instantiate(morcegoRight2Left, morcegoTransform[0].position, TetoTransform[0].rotation);
                //instantiate Aranha chao
                spawnedEnemies[1] = Instantiate(aranhaChao, portasTransform[0].position + new Vector3(0,-portasTransform[0].GetComponent<SpriteRenderer>().bounds.size.y + aranhaChao.GetComponent<SpriteRenderer>().bounds.size.y + 1, 0), portasTransform[0].rotation);
                yield return new WaitForSeconds(1);
                //instantiate morcego
                spawnedEnemies[2] = Instantiate(morcegoLeft2Right, morcegoTransform[1].position, TetoTransform[1].rotation);
                //instantiate Aranha chao
                spawnedEnemies[3] = Instantiate(aranhaChao, portasTransform[1].position + new Vector3(0,-portasTransform[1].GetComponent<SpriteRenderer>().bounds.size.y + aranhaChao.GetComponent<SpriteRenderer>().bounds.size.y + 1, 0), portasTransform[1].rotation);
                break;
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
    void ChangeCenter(int holeToGo)
    {
        Center = holesTransform[holeToGo].position;
    }
}
