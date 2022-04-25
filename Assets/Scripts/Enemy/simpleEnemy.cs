using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class simpleEnemy : MonoBehaviour
{

    public Justin justin;
    public GlobalVariables globalVariables;
    public Transform wayRoot;

    public GameObject target;


    private FiniteStateMachine<simpleEnemy> finiteStateMachine;


    public int currentTimeline;
    public int originalTimeline;

    //prefab dei proiettili
    public enemyBullet bulletPrefab;

    //dati del nemico
    public int enemyLife;


    public enum MachineStatus
    {
        Patrol,
        Attack
    }

    public enum Origin
    {
       Original,
       TeleportedUp,
       TeleportedDown
    }

    public enum Type
    {
        meleeEnemy,
        rangeEnemy
    }

    public MachineStatus currentStatus;
    public Origin currentOrigin;
    public Type enemyType;




    // Start is called before the first frame update
    void Start()
    {
        

        switch (currentOrigin)
        {
            case Origin.Original:
                currentTimeline = originalTimeline;
                break;
            case Origin.TeleportedDown:
                currentTimeline = 0;
                break;
            case Origin.TeleportedUp:
                currentTimeline = 1;
                break;

        }

        

        enemyLife = 1;

        finiteStateMachine = new FiniteStateMachine<simpleEnemy>(this);

        //STATES
        State patrolState = new PatrolState("begin", this);
        State attackState = new AttackState("puzzle", this);
       

        //TRANSITIONS
        finiteStateMachine.AddTransition(patrolState, attackState, () => currentStatus == MachineStatus.Attack);
        finiteStateMachine.AddTransition(attackState, patrolState, () => currentStatus == MachineStatus.Patrol);


        finiteStateMachine.SetState(patrolState);

        globalVariables.enemies.Add(this);


    }

    // Update is called once per frame
    void Update()
    {
   
       if (target == null)
        {
            target = justin.gameObject;
        }
       
            finiteStateMachine.Tik();

        if (enemyLife == 0)
        {
            globalVariables.enemies.Remove(this);
            Destroy(gameObject);
        }


    }

    public void attack()
    {
        switch (enemyType)
        {
            case simpleEnemy.Type.meleeEnemy:
                Debug.Log("melee");

                if(target.GetComponent<simpleEnemy>()!=null)
                {
                    Debug.Log("melee attack on enemy");
                    target.GetComponent<simpleEnemy>().enemyLife -= 1;
                }
                else
                {
                    globalVariables.justinLife -= 1;
                    Debug.Log("justin life:" + globalVariables.justinLife);
                }

                break;

            case simpleEnemy.Type.rangeEnemy:

                enemyBullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2f + transform.up * 2f, Quaternion.identity);
                bullet.shooter = this;
                bullet.transform.up = transform.forward;
                break;
        }

       
    }





}
