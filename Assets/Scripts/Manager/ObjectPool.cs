using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] private List<Actor> Actors;

    [SerializeField] private int poolSize = 10;

    [SerializeField]
    private Dictionary<CType, Queue<Actor>> poolDictionary = new Dictionary<CType, Queue<Actor>>();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (CType type in Enum.GetValues(typeof(CType)))
        {
            poolDictionary[type] = new Queue<Actor>();
        }


        foreach (var actors in Actors)
        {

            for (int i = 0; i < poolSize; i++)
            {
                Actor actor = Instantiate(actors);

                CType type = actor.ActorType;

                actor.transform.SetParent(this.transform);
                actor.gameObject.SetActive(false);

                if (type == CType.None)
                    continue;

            
                poolDictionary[actor.ActorType].Enqueue(actor);
            }
            
        }

      
        
    }

    public void Restart()
    {
        /*foreach (var actors in poolDictionary.Values)
        {
            foreach (var a in actors)
            {
                if(a.ActorType == CType.None || a.ActorType == CType.FireBall || a.ActorType == CType.Spark)
                    break;

                a.GetComponent<Creature>().Initialize();

                if(a.gameObject.activeSelf == true)
                    a.gameObject.SetActive(false);

            }
        }
*/
        foreach(Actor a in transform.GetComponentsInChildren<Actor>())
        {
           
            if (a.ActorType == CType.None || a.ActorType == CType.FireBall || a.ActorType == CType.Spark)
                break;

            a.GetComponent<Creature>().Initialize();

            if (a.gameObject.activeSelf == true)
                a.gameObject.SetActive(false);

        }




    }

    public Actor GetActor(CType type) 
    {
        if (type == CType.None)
            return null;

        if (poolDictionary.ContainsKey(type) && poolDictionary[type].Count > 0)
        {
            Actor actor = poolDictionary[type].Dequeue();
            actor.gameObject.SetActive(true);
            //actor.transform.SetParent(null);

            return actor;
        }
        else
        {
            Debug.LogError("없는 타입");
        }

        return null;

    }

    public void ReturnCreature(Actor actor)
    {
        if (actor.ActorType == CType.None)
            return;
        //actor.transform.SetParent(transform);

        actor.gameObject.SetActive(false);
        poolDictionary[actor.ActorType].Enqueue(actor);
    }

   
}
