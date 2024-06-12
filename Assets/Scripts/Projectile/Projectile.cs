using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Projectile : Actor
{
    public float speed = 10f;
    public float damage;
    protected Rigidbody rb;

    public float DestroyTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Initialize(speed, damage);
    }

    public virtual void Initialize(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;

    }

    public abstract void Launch(Vector3 direction);

    public IEnumerator DestroyC()
    {
        Debug.Log("DestroyC start");
        yield return new WaitForSeconds(DestroyTime);
        GameManager.Instance.objectPool.ReturnCreature(this);
        Debug.Log("DestroyC end");

    }



    private void OnTriggerEnter(Collider other)
    {
        OnActive(other.transform);

    }

    private void OnCollisionEnter(Collision collision)
    {
        OnActive(collision.transform);
    }


    protected virtual void OnActive(Transform collision)
    {
        if (collision.GetComponent<Actor>().ActorType == CType.Player)
            return;

    }

}
