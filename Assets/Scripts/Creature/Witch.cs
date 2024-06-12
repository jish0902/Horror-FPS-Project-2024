using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Witch : Creature
{
    public float radius = 20f;
    public float spawnRange = 5f;

    private float lastAttack;
    public float CoolTime = 2f;

    private State currentState;
    private NavMeshAgent agent;
    private Transform player;

    public List<GameObject> Zombies = new List<GameObject>();

    public override void Heal(float amount)
    {
        Health += amount;
    }

    public override void OnDamaged(float damage)
    {
        base.OnDamaged(damage);
    }

    public override void Dead()
    {
        base.Dead();
        GameManager.Instance.GameEnd(true);
    }


    public void OnAttack()
    {
        // Attack logic here
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Moving;
    }

    void Update()
    {
        agent.SetDestination(player.position);


        switch (currentState)
        {
            case State.Idle:
                LookForPlayer();
                break;
            case State.Moving:
                MoveToPlayer();
                break;
            case State.Attack1:
                StartCoroutine(Attack1());
                break;
            case State.Attack2:
                Attack2();
                break;
        }
    }

    private void LookForPlayer()
    {
       bool playerFound = false;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            Player p = hitCollider.GetComponent<Player>();
            if (p != null)
            {
                //player = p.transform;
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance > agent.stoppingDistance)
                {
                    Debug.Log(distance +", " + agent.stoppingDistance);
                    ChangeState(State.Moving);
                    playerFound = true;
                }
                break;
            }
        }

        if (!playerFound)
        {
            animator.SetBool("move", false);
            //agent.isStopped = true;

            return;
        }
    }

    private void MoveToPlayer()
    {
        animator.SetBool("move", true);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= agent.stoppingDistance)
        {
            if (lastAttack + CoolTime < Time.time)
            {
                lastAttack = Time.time;
                int attackType = Random.Range(0, 4);
                if (attackType == 0)
                {
                    ChangeState(State.Attack2);

                }
                else
                {
                    ChangeState(State.Attack1);

                }
            }
        }
        else if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            ChangeState(State.Idle);
        }
    }

    private IEnumerator Attack1()
    {

        for (int i = 0; i < 5; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            Vector3 temp = GetValidNavMeshPosition(randomOffset + transform.position);

            if (temp != Vector3.zero)
            {
                ChangeState(State.Moving);
                yield return SpawnZombie(temp);
                break;
            }
        }
        ChangeState(State.Idle);
    }

    private void Attack2()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnRange * 3, spawnRange * 3), 0, Random.Range(-spawnRange * 3, spawnRange * 3));
            Vector3 temp = GetValidNavMeshPosition(randomOffset + transform.position);

            if (temp != Vector3.zero)
            {
                agent.Move(temp);
                break;
            }
        }
        ChangeState(State.Idle);
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
    }

    public override void Initialize(float maxHealth = 10)
    {
        MaxHealth = 50f;
        base.Initialize(MaxHealth);
        ActorType = CType.witch;
    }

    IEnumerator SpawnZombie(Vector3 pos)
    {
        Debug.Log("spawn");

        GetComponent<Animator>().SetTrigger("Spawn");
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 3; i++)
        {
            GameObject zombie = GameManager.Instance.objectPool.GetActor(CType.Zombie).gameObject;
            zombie.transform.position = pos;
            zombie.transform.rotation = transform.rotation;
            Zombies.Add(zombie);
        }
       
    }

    Vector3 GetValidNavMeshPosition(Vector3 randomPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, spawnRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero; // 유효하지 않은 경우
    }

    private enum State
    {
        Idle,
        Moving,
        Attack1,
        Attack2
    }
}
