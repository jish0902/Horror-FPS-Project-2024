using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class Creature : Actor
{
    public int Id;

    private float health;
    public float Health
    {   
        get
        {
            return health;
        }

        set
        {
            health = value;

            if(hpController != null)
                hpController.SetHpfloat(health / MaxHealth);
        }
    }
    public float MaxHealth { get; protected set; }

    public bool IsDead { get; protected set; } = false;

    private HpController hpController;


    protected Animator animator;


    private void Awake()
    {

        hpController = GetComponentInChildren<HpController>();


        Initialize();

    }

    private void OnEnable()
    {
        Initialize();

    }


    public virtual void Initialize(float maxHealth = 10f)
    {
        MaxHealth = maxHealth;
        Health = MaxHealth;
        animator = GetComponent<Animator>();

        Debug.Log("√ ±‚»≠");

    }


    public virtual void Dead()
    {
        IsDead =true;
        animator.SetTrigger("Dead"); 


        Invoke(nameof(DeadToObjectpool), 2f);
    }


    private void DeadToObjectpool()
    {
        GameManager.Instance.objectPool.ReturnCreature(this);

    }

    public abstract void Heal(float amount);
  

    public virtual void OnDamaged(float damage)
    {
        Health -= damage;


        if (Health <= 0)
        {
            Dead();
        }

    }
}
