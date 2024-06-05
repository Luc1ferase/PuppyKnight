using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD };

[RequireComponent(typeof(NavMeshAgent))] //确保NavMeshAgent一定存在(没有时会自动创建)
public class EnemyController : MonoBehaviour
{

    public EnemyStates enemyStates;

    private NavMeshAgent agent;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        SwitchStates();
    }
    void SwitchStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                break;
            case EnemyStates.CHASE:
                break;
            case EnemyStates.DEAD:
                break;
        }
    }

}
