using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : Projectile
{

    ParticleSystem ps;

    public float radius = 20f;



    public override void Initialize(float speed, float damage)
    {
        base.Initialize(speed, damage);
        ActorType = CType.Spark;

    }

    private void Awake()
    {
        Initialize(0, 20);
        ps = GetComponent<ParticleSystem>();
    }

    public override void Launch(Vector3 direction)
    {
        // 파티클 시스템 재생
        ps.Play();


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            Creature tempC = hitCollider.GetComponent<Creature>();
            if (tempC != null && tempC.ActorType != CType.Player)
            {
                tempC.OnDamaged(damage);
            }

        }

        StartCoroutine(DestroyC());
    }

    

   


}
