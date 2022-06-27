using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    private simpleEnemy enemy;

    private float rotationSpeed = 50f;
    private float movSpeed = 3f;
    private float huntLimitDistance = 20f;
    private float attackDistance; //distanza al di sotto della quale posso attaccare il target



    private bool ableToAttack;
    private bool attacking;
    private bool checkTimeline;


    public AttackState(string name, simpleEnemy _enemy) : base(name)
    {
        enemy = _enemy;

    }


    public override void Enter()
    {
        ableToAttack = true;
        attacking = false;

        //definisco distanze di attacco diverse per nemici melee oppure range
        switch (enemy.enemyType)
        {
            case simpleEnemy.Type.meleeEnemy:
                attackDistance = 6f;
                break;
            case simpleEnemy.Type.rangeEnemy:
                attackDistance = 20f;
                break;
        }


    }

    public override void Exit()
    {
        //smetto di attaccare
        enemy.CancelInvoke("attack");
    }

    public override void Tik()
    {
        if (enemy.target == null)
        {
            enemy.target = enemy.globalVariables.gameObject;
        }
        float distance = Mathf.Abs(enemy.transform.position.x - enemy.target.transform.position.x); // distanza dal target

        //check per impedire che il nemico cerchi di attaccare il target se si trova su un altra timeline
        if (enemy.target.GetComponent<Justin>() != null)
        {
            checkTimeline = enemy.currentTimeline != enemy.globalVariables.currentTimeline;
        }
        else if (enemy.target.GetComponent<simpleEnemy>() != null)
        {
            checkTimeline = enemy.currentTimeline != enemy.target.GetComponent<simpleEnemy>().currentTimeline;
        }
        else
        {
            checkTimeline = true;
            enemy.CancelInvoke("attack");
        }


        //controllo che non sia un nemico che sta fermo
    if (enemy.standing == false)
    {
            float distanceFromBase = Mathf.Abs(enemy.wayRoot.transform.position.x - enemy.target.transform.position.x); //distanza dal punto di patroling

            if (distance < attackDistance)
        {
             //il nemico ruota in modo da puntare il target

            Vector3 targetDirection = enemy.target.transform.position - enemy.transform.position;
            targetDirection.y = 0f;
            targetDirection.z = 0f;
            targetDirection.Normalize();

            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDirection, step, 0.0f);
            enemy.transform.rotation = Quaternion.LookRotation(newDir, enemy.transform.up);

                //se il nemico non sta già attaccando invoco la funzione attack di simpleEnemy
                // in base al tipo di nemico l'attacco verrà ripetuto con tempi diversi
            if (ableToAttack == true)
            {
                switch (enemy.enemyType)
                {
                    case simpleEnemy.Type.meleeEnemy:
                        enemy.InvokeRepeating("attack", 1f, 5f);
                        ableToAttack = false;
                        attacking = true;
                        break;
                    case simpleEnemy.Type.rangeEnemy:
                        enemy.InvokeRepeating("attack", 1f, 5f);
                        ableToAttack = false;
                        attacking = true;
                        break;

                }
            }
        }

            // se il target si allontana dal nemico lui torna in patrol
        if ((distanceFromBase > huntLimitDistance && enemy.readyToPatrol) || (checkTimeline && enemy.readyToPatrol))
        {
            enemy.currentStatus = simpleEnemy.MachineStatus.Patrol;
        }

        //se il target si sta allontanando il nemico lo insegue per un pò
        if (distanceFromBase <= huntLimitDistance && distance >= attackDistance - 2f)
        {



            Vector3 targetDirection = enemy.target.transform.position - enemy.transform.position;
            targetDirection.y = 0f;
            targetDirection.z = 0f;
            targetDirection.Normalize();


            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDirection, step, 0.0f);
            enemy.transform.rotation = Quaternion.LookRotation(newDir, enemy.transform.up);

            Vector3 movVec = Vector3.forward * movSpeed * Time.deltaTime;

            enemy.transform.Translate(movVec);
        }

    }


    //di qui in poi codice per i nemici fermi in una posizione
    // identico a attacco di quelli sopra ma non si può muovere
        else
        {
            attackDistance = 20f;
          
            if (distance < attackDistance && enemy.globalVariables.currentTimeline == enemy.currentTimeline)
            {
                Vector3 targetDirection = enemy.target.transform.position - enemy.transform.position;
                targetDirection.y = 0f;
                targetDirection.z = 0f;
                targetDirection.Normalize();

                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDirection, step, 0.0f);
                enemy.transform.rotation = Quaternion.LookRotation(newDir, enemy.transform.up);

                if (ableToAttack == true)
                {
                    switch (enemy.enemyType)
                    {
                        case simpleEnemy.Type.meleeEnemy:
                            enemy.InvokeRepeating("attack", 1f, 5f);
                            ableToAttack = false;
                            attacking = true;
                            break;
                        case simpleEnemy.Type.rangeEnemy:
                            enemy.InvokeRepeating("attack", 1f, 5f);
                            ableToAttack = false;
                            attacking = true;
                            break;

                    }
                }
            }

            if(distance > attackDistance+0.2f || enemy.globalVariables.currentTimeline != enemy.currentTimeline)
            {
                enemy.CancelInvoke("attack");
                ableToAttack = true;
            }

        }



        /* switch ((distance))
     {
         case (< 1.0):
             Debug.Log("attacco");
             break;
         case (> 8.0):
             enemy.currentStatus = simpleEnemy.MachineStatus.Patrol;
             break;

         default:

             Vector3 targetDirection = enemy.justin.transform.position - enemy.transform.position;
             targetDirection.y = 0f;
             targetDirection.z = 0f;
             targetDirection.Normalize();

             float step = rotationSpeed * Time.deltaTime;
             Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDirection, step, 0.0f);
             enemy.transform.rotation = Quaternion.LookRotation(newDir, enemy.transform.up);

             Vector3 movVec = Vector3.forward * movSpeed * Time.deltaTime;

             enemy.transform.Translate(movVec);
             break;
     }*/



    }


}
