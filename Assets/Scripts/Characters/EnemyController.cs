using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD };

[RequireComponent(typeof(NavMeshAgent))] //确保NavMeshAgent一定存在(没有时会自动创建)
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{

    private EnemyStates enemyStates;

    private NavMeshAgent agent;

    private Animator anim;

    private Collider coll;

    private CharacterStats characterStats;

    [Header("Basic Settings")]

    public float sightRadius;

    public bool isGuard;

    private float speed;

    private GameObject attackTarget;

    public float lookAtTime;

    private float remainLookAtTime;

    private float lastAttackTime;

    private Quaternion guardRotaion;

    [Header("Patrol State")]

    public float patrolRange;

    private Vector3 wayPoint;

    private Vector3 guardPos;

    //bool配合动画
    bool isWalk;

    bool isChase;

    bool isFollow;
    bool isDead;
    bool playerDead;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        speed = agent.speed;
        guardPos = transform.position;
        guardRotaion = transform.rotation;

        remainLookAtTime = lookAtTime;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //FIXME:场景切换后修改掉
        GameManager.Instance.AddObserver(this);
    }

    //切换场景时启用
    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}

    void OnDisable()
    {
        if (GameManager.IsInitialized)
            return;
        GameManager.Instance.RemoveObserver(this);
    }

    void Update()
    {
        if (characterStats.CurrentHealth == 0)
            isDead = true;

        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDead);
    }
    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;

        //如果发现player 切换到chase
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("Founded Player");
        }
        switch (enemyStates)
        {

            case EnemyStates.GUARD:
                isChase = false;

                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                        isWalk = false;
                    transform.rotation = Quaternion.Lerp(transform.rotation, guardRotaion, 0.01f);
                }

                break;
            case EnemyStates.PATROL:

                isChase = false;
                agent.speed = speed * 0.5f;

                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }


                break;
            case EnemyStates.CHASE:

                //TODO : 配合动画

                isWalk = false;
                isChase = true;

                agent.speed = speed;

                if (!FoundPlayer())
                {
                    //TODO : 拉脱回到上一个状态
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;

                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;

                }

                //TODO : 在攻击范围内则攻击
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;

                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        //暴击判断
                        characterStats.isCritical = Random.value < characterStats.attackData.critialChance;
                        //执行攻击
                        Attack();

                    }

                }

                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                agent.enabled = false;
                Destroy(gameObject, 2f);
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);//面向敌人
        if (TargetInAttackRange())
            anim.SetTrigger("Attack");//近身攻击动画
        else if (TargetInSkillRange())
            anim.SetTrigger("Skill");//技能攻击动画
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);//看这个球体范围内是否有敌人

        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }

        }

        return false;
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else
            return false;

    }

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        //FIXME:可能出现的问题
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }


    //Animation Event

    void Hit()
    {
        if (attackTarget != null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        //获胜动画
        //停止所有移动
        //停止Agent

        anim.SetBool("Win", true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;

    }
}
