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
    private float targetVisibleDistance = 20f;




    public PatrolState(string name, simpleEnemy _enemy) : base(name)
    {
        enemy = _enemy;

    }


    public override void Enter()
    {
        searchPath();
    }

    public override void Exit()
    {
        walkingSequence.Kill();
    }

    public override void Tik()
    {

        if (Mathf.Abs(enemy.wayRoot.transform.position.x - enemy.justin.transform.position.x) < targetVisibleDistance && enemy.globalVariables.currentTimeline == enemy.currentTimeline)
        {
            enemy.target = enemy.justin.gameObject;
            enemy.currentStatus = simpleEnemy.MachineStatus.Attack;
        }


        foreach (simpleEnemy intruder in enemy.globalVariables.enemies)
        {
            bool intrusion = intruder.originalTimeline != enemy.originalTimeline;
            bool sameTimeline = intruder.currentTimeline == enemy.currentTimeline;
            if (Mathf.Abs(enemy.wayRoot.transform.position.x - intruder.transform.position.x) < targetVisibleDistance && intrusion && sameTimeline)
            {
                enemy.target = intruder.gameObject;
                enemy.currentStatus = simpleEnemy.MachineStatus.Attack;
                Debug.Log("intruder found ");
            }
        }


    }


    //funzione che crea il percorso
    private void searchPath()
    {
        if (enemy.wayRoot != null && enemy.wayRoot.childCount > 0)
        {
            Vector3[] pathPositions = new Vector3[enemy.wayRoot.childCount];
            for (int i = 0; i < enemy.wayRoot.childCount; i++)
            {
                pathPositions[i] = enemy.wayRoot.GetChild(i).position;
                pathPositions[i].z = enemy.transform.position.z;
                pathPositions[i].y = enemy.transform.position.y;
            }
            walkingSequence = DOTween.Sequence();

            walkingSequence.Append(enemy.transform.DOPath(pathPositions, pathDuration, PathType.CatmullRom, PathMode.Full3D, resolution: 10).SetEase(Ease.Linear)
                .SetLookAt(0.01f).SetId("walking").OnComplete(
                () => searchPath()

                ));

        }


    }
}

