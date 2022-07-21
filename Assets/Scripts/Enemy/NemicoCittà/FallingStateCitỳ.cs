using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStateCity : State
{

    private NemicoCity enemy;

    private Vector3 _velocity;
    private float movSpeed = -9.81f;

    public FallingStateCity(string name, NemicoCity _enemy) : base(name)
    {
        enemy = _enemy;

    }

    public override void Enter()
    {
        Debug.Log("falling");
    }

    public override void Tik()
    {
        Vector3 targetDirection = enemy.transform.up;

        Vector3 movVec = targetDirection * movSpeed * Time.deltaTime;

        enemy.transform.Translate(movVec);


        if (enemy._isGrounded)
        {
            enemy.currentStatus = NemicoCity.MachineStatus.Patrol;
        }


    }


    public override void Exit()
    {


    }


}
