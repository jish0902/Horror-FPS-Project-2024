using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum MagicType
{
    None,
    FireBall,
    Spark,
}


public class PlayerShoot : MonoBehaviour
{
    private PlayerInput playerInput; 
    private Rigidbody playerRigidbody; 

    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;


    public float FireballcoolDownTime = 2f; // 쿨타임 (초 단위)
    public float SparkcoolDownTime = 10f; // 쿨타임 (초 단위)
    private float FireballlastFireTime = -Mathf.Infinity; // 마지막 발사 시각
    private float SparklastFireTime = -Mathf.Infinity;// 마지막 발사 시각


    public Image FireballImage;
    public Image SparkImage;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerInput.OnFired += OnFired;
    }

    private void Update()
    {
        UpdateCooldownUI();
    }


    public void OnFired(MagicType MagicType)
    {

        switch (MagicType)
        {
            case MagicType.None:

                break;
            case MagicType.FireBall:
                if (Time.time >= FireballlastFireTime + FireballcoolDownTime)
                {
                    FireballlastFireTime = Time.time;
                    StartCoroutine(FireBall());
                }
                break;
            case MagicType.Spark:
                if (Time.time >= SparklastFireTime + SparkcoolDownTime)
                {
                    SparklastFireTime = Time.time;
                    StartCoroutine(Spark());
                }
                else
                {
                    //Debug.Log(SparklastFireTime);
                }
                break;
        }


      

        
    }


    private void UpdateCooldownUI()
    {
        if (FireballImage != null)
        {
            float fireballCooldownValue = Mathf.Clamp01((Time.time - FireballlastFireTime) / FireballcoolDownTime);
            
            FireballImage.fillAmount = Mathf.Abs(fireballCooldownValue - 1f);
        }

        if (SparkImage != null)
        {
            float sparkCooldownValue = Mathf.Clamp01((Time.time - SparklastFireTime) / SparkcoolDownTime);
            SparkImage.fillAmount = Mathf.Abs(sparkCooldownValue - 1f);
        }
    }

    IEnumerator Spark()
    {
        GetComponent<Animator>().SetTrigger("Spark");

        yield return new WaitForSeconds(0.5f);

        GameObject projectile = GameManager.Instance.objectPool.GetActor(CType.Spark).gameObject;
        projectile.transform.position = transform.position + Vector3.up;
        projectile.transform.rotation = transform.rotation;


        projectile.GetComponent<Projectile>().Launch(transform.forward);
    }

    IEnumerator FireBall()
    {
        GetComponent<Animator>().SetTrigger("FireBall");

        yield return new WaitForSeconds(0.5f);


        GameObject projectile = GameManager.Instance.objectPool.GetActor(CType.FireBall).gameObject;
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;


        projectile.GetComponent<Projectile>().Launch(transform.forward);
    }




}
