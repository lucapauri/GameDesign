using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateCity : State
{

    private NemicoCity enemy;

    private float rotationSpeed = 50f;
    private float movSpeed = 3f;
    private float attackDistance = 12f; //distanza al di sotto della quale posso attaccare il target
    private float targetVisibleDistance = 11f;



    private bool ableToAttack;
    private bool attacking;
    private bool checkTimeline;


    public AttackStateCity(string name, NemicoCity _enemy) : base(name)
    {
        enemy = _enemy;

    }


    public override void Enter()
    {
        ableToAttack = true;
        attacking = false;
        

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
            float verticalDistance = Mathf.Abs(enemy.target.transform.position.y - enemy.transform.position.y);
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
                   
                    enemy.InvokeRepeating("attack", 2f, 5f);
                    ableToAttack = false;
                    attacking = true;
                      
                }
            }

            // se il target si allontana dal nemico lui torna in patrol
            bool targetTooFar = distance > targetVisibleDistance;



            if ((targetTooFar || checkTimeline))
            {

                enemy.currentStatus = NemicoCity.MachineStatus.Patrol;
            }
            


        }


    }


}
