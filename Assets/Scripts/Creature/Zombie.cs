using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Creature
{
    [SerializeField]
    Transform target;

    NavMeshAgent agent;

    public float radius = 10f; // 구체의 반지름

    public override void Initialize(float maxHealth)
    {

        MaxHealth = 10f;

        base.Initialize(MaxHealth);

        ActorType = CType.Zombie;
    }

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
    }


    private void OnTriggerStay(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            animator.SetTrigger("Attack");

            p.OnDamaged(Health);
        }
    }

    




    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    private void FixedUpdate()
    {
        bool t = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius * 5);
        foreach (var hitCollider in hitColliders)
        {
            Player player = hitCollider.GetComponent<Player>();
            if (player != null)
            {
                target = player.transform;
                agent.SetDestination(target.position);
                animator.SetFloat("Movement", agent.velocity.magnitude);

                t = false;
                return; 
            }
        }

        if (t == true)
        {
            target = null;
            agent.isStopped = true;
            animator.SetFloat("Movement", 0);
        }
           

    }
}
