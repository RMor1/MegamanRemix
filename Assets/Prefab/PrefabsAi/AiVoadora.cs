using System.Collections.Generic;
using UnityEngine;
public class AiVoadora : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private GameObject Comeco;
    [Range(0, 3)] [SerializeField] public int QuantidadeDeIntermediarios;
    [SerializeField] private GameObject Intermediario1;
    [SerializeField] private GameObject Intermediario2;
    [SerializeField] private GameObject Intermediario3;
    [SerializeField] private GameObject Final;
    private int PosicaoAtual;
    private Vector2 DeltaPosition;
    private bool Voltando = false;
    private int Action;
    [SerializeField] private bool verRange = false;
    [SerializeField] private float SeguirRange, AtaqueRange, DetecçãoRange;
    private GameObject Player;
    private Animator AiAnimator;
    private float DistanceToPlayer;
    [SerializeField] int Vida;
    private enum AiTypeList
    {
        melee, ranged
    }
    [SerializeField] private AiTypeList TipoDeAI;
    private GameObject HitBoxAttack;
    void Start()
    {
        HitBoxAttack = GameObject.Find("DamageHitBox");
        AiAnimator = gameObject.GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        AiAnimator.SetBool("IsStandby", true);
        Action = -1;
        PosicaoAtual = -1;
        DeltaPosition = Comeco.transform.position - transform.position;
        CorrecaoDeDirecao();
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
            Gizmos.DrawWireSphere(transform.position, DetecçãoRange);
        }
    }
    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (DistanceToPlayer < DetecçãoRange)
        {
            AiAnimator.SetBool("IsStandby", false);
        }
        if (AiAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flying") == true)
        {
            if (DistanceToPlayer < AtaqueRange)
            {
                Action = 2;
                AiAnimator.SetBool("IsAttacking", true);
            }
            else if (DistanceToPlayer < SeguirRange)
            {
                Action = 1;
            }
            else
            {
                Action = 0;
            }
        }
    }
    void FixedUpdate()
    {
        if (TipoDeAI == AiTypeList.ranged)
        {
            //Angulo entre player e Inimigo 
            Vector2 lookdir = new Vector2(Player.transform.position.x, Player.transform.position.y) - rb.position;
            float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        }
        rb.velocity = new Vector2(0, 0);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        switch (Action)
        {
            case 0:
                //Segue Caminho Predeterminado
                MoveTo(gameObject.transform.position, DeltaPosition);
                break;
            case 1:
                //Segue Player
                MoveTo(gameObject.transform.position, DeltaPosition);
                break;
            case 2:
                //AtacaPlaye
                break;
        }
        switch (Action)
        {
            case 0:
                switch (QuantidadeDeIntermediarios)
                {
                    case 0:
                        if (PosicaoAtual == 0) DeltaPositionDefine(Final);
                        else if (PosicaoAtual == 4) DeltaPositionDefine(Comeco);
                        break;
                    case 1:
                        if (PosicaoAtual == 0)
                        {
                            DeltaPositionDefine(Intermediario1);
                        }
                        else if (PosicaoAtual == 1)
                        {
                            switch (Voltando)
                            {
                                case false:
                                    DeltaPositionDefine(Final);
                                    break;
                                case true:
                                    DeltaPositionDefine(Comeco);
                                    break;
                            }
                        }
                        else if (PosicaoAtual == 4)
                        {
                            DeltaPositionDefine(Intermediario1);
                        }
                        break;
                    case 2:
                        if (PosicaoAtual == 0)
                        {
                            DeltaPositionDefine(Intermediario1);
                        }
                        else if (PosicaoAtual == 1)
                        {
                            switch (Voltando)
                            {
                                case false:
                                    DeltaPositionDefine(Intermediario2);
                                    break;
                                case true:
                                    DeltaPositionDefine(Comeco);
                                    break;
                            }
                        }
                        else if (PosicaoAtual == 2)
                        {
                            switch (Voltando)
                            {
                                case false:
                                    DeltaPositionDefine(Final);
                                    break;
                                case true:
                                    DeltaPositionDefine(Intermediario1);
                                    break;
                            }
                        }
                        else if (PosicaoAtual == 4)
                        {
                            DeltaPositionDefine(Intermediario2);
                        }
                        break;
                    case 3:
                        if (PosicaoAtual == 0)
                        {
                            DeltaPositionDefine(Intermediario1);
                        }
                        else if (PosicaoAtual == 1)
                        {
                            switch (Voltando)
                            {
                                case false:
                                    DeltaPositionDefine(Intermediario2);
                                    break;
                                case true:
                                    DeltaPositionDefine(Comeco);
                                    break;
                            }
                        }
                        else if (PosicaoAtual == 2)
                        {
                            switch (Voltando)
                            {
                                case false:
                                    DeltaPositionDefine(Intermediario3);
                                    break;
                                case true:
                                    DeltaPositionDefine(Intermediario1);
                                    break;
                            }
                        }
                        else if (PosicaoAtual == 3)
                        {
                            switch (Voltando)
                            {
                                case false:
                                    DeltaPositionDefine(Final);
                                    break;
                                case true:
                                    DeltaPositionDefine(Intermediario2);
                                    break;
                            }
                        }
                        else if (PosicaoAtual == 4)
                        {
                            DeltaPositionDefine(Intermediario3);
                        }
                        break;
                }
                break;
            case 1:
                DeltaPositionDefine(Player);
                break;
            case 2:
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == Comeco.GetComponent<CircleCollider2D>()) { PosicaoAtual = 0; CorrecaoDeDirecao(); }
        else if (collision == Final.GetComponent<CircleCollider2D>()) { PosicaoAtual = 4; CorrecaoDeDirecao(); }
        else if (collision == Intermediario1.GetComponent<CircleCollider2D>()) { PosicaoAtual = 1; CorrecaoDeDirecao(); }
        else if (collision == Intermediario2.GetComponent<CircleCollider2D>()) { PosicaoAtual = 2; CorrecaoDeDirecao(); }
        else if (collision == Intermediario3.GetComponent<CircleCollider2D>()) { PosicaoAtual = 3; CorrecaoDeDirecao(); }
        if (PosicaoAtual == 0)
        {
            Voltando = false;
        }
        else if (PosicaoAtual == 4)
        {
            Voltando = true;
        }
    }

    void MoveTo(Vector3 Start, Vector3 DeltaPos)
    {
        rb.MovePosition(Start + (DeltaPos * MoveSpeed * Time.fixedDeltaTime));
    }
    /// <summary>
    /// Transforma a velocidade Vector2 em uma quantidade decimal, com a soma da velocidade.x e velocidade.y sendo igual a 1.
    /// </summary>
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
    /// <summary>
    /// Define a direção para onde o player vai
    /// </summary>
    void DeltaPositionDefine(GameObject FinalPosition)
    {
        DeltaPosition = FinalPosition.transform.position - transform.position;
        CorrecaoDeDirecao();
    }
    void AlertOnAnimationEnd(string AnimationName)
    {
        if (AnimationName.Equals("AttackStart"))
        {
            HitBoxAttack.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (AnimationName.Equals("AttackEnd"))
        {
            HitBoxAttack.GetComponent<BoxCollider2D>().enabled = false;
            AiAnimator.SetBool("IsAttacking", false);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        Vida--;
        if (Vida <=0 )
        {
            Destroy(gameObject);
        }
    }
}