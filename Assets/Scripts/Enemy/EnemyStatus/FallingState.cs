using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{

    private simpleEnemy enemy;

    private Vector3 _velocity;
    private float movSpeed= -9.81f;

    public FallingState(string name, simpleEnemy _enemy) : base(name)
    {
        enemy = _enemy;

    }

    public override void Enter()
    {
    
    }

    public override void Tik()
    {
        Vector3 targetDirection = enemy.transform.up;

        Vector3 movVec = targetDirection * movSpeed * Time.deltaTime;

        enemy.transform.Translate(movVec);


        if (enemy._isGrounded && !enemy.standing)
        {
            enemy.currentStatus = simpleEnemy.MachineStatus.Patrol;
        }
        else if (enemy._isGrounded && enemy.standing)
        {
            enemy.currentStatus = simpleEnemy.MachineStatus.Attack;
        }


    }


    public override void Exit()
    {
       

    }

   
}
