using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialState : State
{

    private simpleEnemy enemy;


    
    public SpecialState(string name, simpleEnemy _enemy) : base(name)
    {
        enemy = _enemy;

    }


    public override void Enter()
    {
       // se serve aggiungo variabili per i nemici speciali nella start
        switch (enemy.special)
        {
            case simpleEnemy.Specials.trexFriend:

                break;
           
        }


    }

    public override void Exit()
    {
        
    }

    public override void Tik()
    {
        //implemento comportamento speciale con richiamo alla funzione apposta i simpleEnemy
        switch (enemy.special)
        {
           
            case simpleEnemy.Specials.trexFriend:
                float distance = Vector3.Distance(enemy.transform.position, enemy.justin.transform.position);
                if (distance<8f && Input.GetKeyDown(KeyCode.X))
                {
                    enemy.currentStatus = simpleEnemy.MachineStatus.Patrol;
                    GameObject[] spear = GameObject.FindGameObjectsWithTag("spear");
                    enemy.specialInteraction(spear[0]);
                    enemy.specialInteraction(spear[1]);
                    
                }

                break;
          
        }



    }


}