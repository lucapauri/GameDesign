using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolStateCity : State
{

    private NemicoCity enemy;


    //variabili per il controllo del patroling
    public Sequence walkingSequence;
    private float pathDuration = 10f;
    private float targetVisibleDistance = 9f;


    private Vector3 targetPos;
    private float shift;

    private float rotationSpeed = 50f;
    private float movSpeed = 2f;

    private Vector3 movVec;



    public PatrolStateCity(string name, NemicoCity _enemy) : base(name)
    {
        enemy = _enemy;

    }


    public override void Enter()
    {
        //searchPath();
        if (enemy.GetComponent<Animator>())
        {
            enemy.enemyAnimator.SetBool("Walk", true);
        }

        shift = enemy.platform.gameObject.GetComponent<Collider>().bounds.extents.x;

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
            enemy.currentStatus = NemicoCity.MachineStatus.Attack;
        }
            targetPos = new Vector3(enemy.platform.position.x + shift, enemy.transform.position.y, enemy.platform.position.z);

            Vector3 targetDirection = targetPos - enemy.transform.position;
            targetDirection.y = 0f;
            targetDirection.Normalize();

            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDirection, step, 0.0f);
            enemy.transform.rotation = Quaternion.LookRotation(newDir, enemy.transform.up);


            if (enemy.platform.GetComponent<PiattaformeMobili>())
        {
            movVec = Vector3.forward * movSpeed * Time.deltaTime + Vector3.up * enemy.platform.GetComponent<PiattaformeMobili>().movSpeed * Time.deltaTime;

        }

        else
        {
            movVec = Vector3.forward * movSpeed * Time.deltaTime;
        }
           
            enemy.transform.Translate(movVec);



        if (Vector3.Distance(targetPos, enemy.transform.position) <= 1f)
        {
            shift = shift * -1;
            
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

