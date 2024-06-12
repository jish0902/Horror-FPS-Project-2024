using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    public Vector3 gravity = new Vector3(0, -1f, 0); // 중력 가속도

    public float radius = 5f;

    

    public override void Initialize(float speed, float damage)
    {
        base.Initialize(speed, damage);
        ActorType = CType.FireBall;

    }


    public override void Launch(Vector3 direction)
    {
        Debug.Log(direction);
        StartCoroutine(DestroyC());

        Quaternion rotation = Quaternion.AngleAxis(15, Vector3.right);
        Vector3 adjustedDirection = rotation * direction;

        rb.velocity = adjustedDirection.normalized * speed;

        rb.useGravity = false;
    }


    void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }


    protected override void OnActive(Transform collision)
    {
       

        if (collision.GetComponent<Actor>() != null && collision.GetComponent<Actor>().ActorType == CType.Player)
            return;

        Debug.Log(collision.name);
        if(collision.name == "Player")
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            Creature tempC = hitCollider.GetComponent<Creature>();
            if (tempC != null && tempC.ActorType != CType.Player)
            {
                
                tempC.OnDamaged(damage);
            }

        }

        GameManager.Instance.objectPool.ReturnCreature(this);
        StopAllCoroutines();
    }

}
