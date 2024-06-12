using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    public override void Initialize(float maxHealth)
    {
        maxHealth = 100;

        base.Initialize(maxHealth);

        ActorType = CType.Player;
    }


    public override void Heal(float amount)
    {
        Health += amount;

    }

    private float lastDamagedTime;
    public override void OnDamaged(float damage)
    {

        if(lastDamagedTime + 0.5f < Time.time)
        {
            lastDamagedTime = Time.time;
            base.OnDamaged(damage);

        }
    }

    public override void Dead()
    {
        base.Dead();

        GameManager.Instance.GameEnd(false);
    }

}
