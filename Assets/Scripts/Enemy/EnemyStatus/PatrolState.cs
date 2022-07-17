using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolState : State
{

    private simpleEnemy enemy;


    //variabili per il controllo del patroling
    public Sequence walkingSequence;
    private float pathDuration = 10f;
    private float targetVisibleDistance = 12f;

    private Vector3 targetPos;
    private Transform target;
    private float rotationSpeed = 50f;
    private float movSpeed = 2f;
    private float _gravity = -9.81f;


    public PatrolState(string name, simpleEnemy _enemy) : base(name)
    {
        enemy = _enemy;

    }


    public override void Enter()
    {
        target = enemy.wayRoot.GetChild(0);
        if (enemy.GetComponent<Animator>())
        {
            enemy.enemyAnimator.SetBool("Walk", true);
        }

     
        if (enemy.special == simpleEnemy.Specials.robot)
        {
            targetVisibleDistance = 4f;
        }
    }

    public override void Exit()
    {
        walkingSequence.Kill();
    }

    public override void Tik()
    {

        float distanceFromJustin = Mathf.Abs(enemy.transform.position.x - enemy.justin.transform.position.x);
        bool TimelineCheck = enemy.globalVariables.currentTimeline == enemy.currentTimeline;
        if (distanceFromJustin < targetVisibleDistance && TimelineCheck)
        {
            enemy.target = enemy.justin.gameObject;
            enemy.currentStatus = simpleEnemy.MachineStatus.Attack;
        }


        foreach (simpleEnemy intruder in enemy.globalVariables.enemies)
        {
            if(intruder!= null)
            {
                bool intrusion = intruder.originalTimeline != enemy.originalTimeline;
                bool sameTimeline = intruder.currentTimeline == enemy.currentTimeline;
                float distanceFromIntruder = Mathf.Abs(enemy.transform.position.x - intruder.transform.position.x);
                if (distanceFromIntruder < targetVisibleDistance && intrusion && sameTimeline)
                {
                    enemy.target = intruder.gameObject;
                    enemy.currentStatus = simpleEnemy.MachineStatus.Attack;
                }
            }
        }

        patrol();
        
    }

    private void patrol()
    {
        targetPos = new Vector3(target.position.x, enemy.transform.position.y, target.position.z);
        Vector3 targetDirection = targetPos - enemy.transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDirection, step, 0.0f);
        enemy.transform.rotation = Quaternion.LookRotation(newDir, enemy.transform.up);

        float firstPointDistance = Mathf.Abs(enemy.wayRoot.GetChild(0).position.x - enemy.transform.position.x);
        float secondPointDistance = Mathf.Abs(enemy.wayRoot.GetChild(1).position.x - enemy.transform.position.x);

        if (firstPointDistance <= 1f)
        {
           
            target = enemy.wayRoot.GetChild(1);
        }

        else if (secondPointDistance <= 1f)
        {

            target = enemy.wayRoot.GetChild(0);
        }

        enemy.controller.Move(targetDirection * Time.deltaTime * movSpeed);

        if (enemy._isGrounded)
        {
            enemy.controller.Move(enemy.transform.up * Time.deltaTime * _gravity);
        }

    }
}

