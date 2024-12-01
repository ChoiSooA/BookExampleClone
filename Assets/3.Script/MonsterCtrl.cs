using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;
    public float traceDist = 10f;
    public float attackDist = 2.0f;
    public bool isDie = false;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    GameObject bloodEffect;

    private int hp = 100;

    PlayerCtrl player;


    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    private void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerCtrl>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        //agent.destination = playerTr.position;

        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

    }

    private void Update()
    {
        if (agent.remainingDistance >= 2.0f)
        {
            Vector3 direction = agent.desiredVelocity;
            if (direction != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(direction);
                monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * 10f);
            }
        }
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }


    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTr.position);    //Agent의 목적지 지정
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    isDie = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);

                    yield return new WaitForSeconds(3.0f);

                    hp = 100;
                    isDie = false;
                    state = State.IDLE;
                    GetComponent<CapsuleCollider>().enabled = true;

                    this.gameObject.SetActive(false);
                    break;
            }
            yield return new WaitForSeconds(0.3f);  //0.3초마다 계속 몬스터 state 체크함
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);
            
        }
    }

    public void OnDamage(Vector3 pos, Vector3 normal)   //몬스터 죽었을때
    {
        

        Quaternion rot = Quaternion.LookRotation(normal);
        ShowBloodEffect(pos, rot);

        //StartCoroutine(Damage(30));
        Damage(30);
    }

    /*public IEnumerator Damage(int attack)
    {
        anim.SetTrigger(hashHit);
        hp -= attack;
        state = State.IDLE;
        if (hp <= 0)
        {
            state = State.DIE;
            player.DisplayScore(50);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //state = State.TRACE;
    }*/

    public void Damage(int attack)
    {
        //Debug.Log(gameObject.name);
        anim.SetTrigger(hashHit);
        hp -= attack;
        if (hp <= 0)
        {
            state = State.DIE;
            player.DisplayScore(50);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1f);
    }

    private void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();

        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }


}
