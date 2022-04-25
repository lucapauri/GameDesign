using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    private simpleEnemy enemy;

    private float rotationSpeed= 50f;
    private float movSpeed = 3f;
    private float huntLimitDistance = 20f;
    private float attackDistance;



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
        switch (enemy.enemyType)
        {
            case simpleEnemy.Type.meleeEnemy:
                attackDistance = 6f;
                break;
            case simpleEnemy.Type.rangeEnemy:
                attackDistance = 25f;
                break;
        }
            
        
    }

    public override void Exit()
    {
        enemy.CancelInvoke("attack");
    }

    public override void Tik()
    {
      
        float distanceFromBase = Mathf.Abs(enemy.wayRoot.transform.position.x - enemy.target.transform.position.x);
        float distance = Mathf.Abs(enemy.transform.position.x - enemy.target.transform.position.x);

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
        }





        if (distance < attackDistance && ableToAttack == true)
        {
            switch (enemy.enemyType)
            {
                case simpleEnemy.Type.meleeEnemy:
                    Debug.Log("melee attack");
                    enemy.InvokeRepeating("attack", 1f, 1f);
                    ableToAttack = false;
                    attacking = true;
                    break;
                case simpleEnemy.Type.rangeEnemy:
                    enemy.InvokeRepeating("attack", 6f, 6f);
                    ableToAttack = false;
                    attacking = true;
                    break;

            }
        }

            if (distanceFromBase > huntLimitDistance || checkTimeline)
        {
            enemy.currentStatus = simpleEnemy.MachineStatus.Patrol;
        }

        if (distanceFromBase <= huntLimitDistance && distance >= attackDistance-2f)
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
