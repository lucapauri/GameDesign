using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BabyPtero : MonoBehaviour
{

    public GlobalVariables globalVariables;
    public Transform wayroot;

    public Sequence walkingSequence;
    public Sequence startingSequence;
    public Sequence downSequence;

    private Vector3[] startPos = new Vector3[1];
    private Vector3[] downPos =  new Vector3[1];

    private int originalTimeline=0;
    private int currentTimeline = 1;

    private float targetVisibleDistance = 15f;

    private GameObject target;
    private bool targetFound;
    private bool readyToKill;

    private float rotationSpeed= 30f;
    private float movSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        targetFound = false;
        readyToKill = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
        wayroot = GameObject.FindGameObjectWithTag("PteroWayroot").transform;

        starting();

    }

    private void Update()
    {
        if (!targetFound && readyToKill)
        {
            foreach (simpleEnemy intruder in globalVariables.enemies)
            {
                bool intrusion = intruder.originalTimeline != originalTimeline;
                bool sameTimeline = intruder.currentTimeline == currentTimeline;
                float distanceFromIntruder = Mathf.Abs(wayroot.transform.position.x - intruder.transform.position.x);
                float verticalDistance = Mathf.Abs(wayroot.transform.position.y - intruder.transform.position.y);
                if (distanceFromIntruder < targetVisibleDistance && intrusion && sameTimeline && verticalDistance < 5f)
                {
                    target = intruder.gameObject;
                    targetFound = true;
                    walkingSequence.Pause();

                }
            }
        }

        if (targetFound)
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0f;
            targetDirection.z = 0f;
            targetDirection.Normalize();


            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir, transform.up);

            Vector3 movVec = Vector3.forward * movSpeed * Time.deltaTime;

            transform.Translate(movVec);

            if ((target.transform.position.x - transform.position.x) < 2f)
            {
                target.GetComponent<simpleEnemy>().enemyLife -= 1;
                downPatroling();
                targetFound = false;
            }
        }
    }


    private void searchPath()
    {
        if (wayroot != null && wayroot.childCount > 0)
        {
            Vector3[] pathPositions = new Vector3[wayroot.childCount];
            for (int i = 0; i < wayroot.childCount; i++)
            {
                pathPositions[i] = wayroot.GetChild(i).position;
                pathPositions[i].z = transform.position.z;
                pathPositions[i].y = transform.position.y;
            }
            walkingSequence = DOTween.Sequence();

            walkingSequence.Append(transform.DOPath(pathPositions, 8, PathType.CatmullRom, PathMode.Full3D, resolution: 10).SetEase(Ease.Linear).SetLookAt(0.01f)
                .SetId("walking").OnComplete(
                () => searchPath()

                ));

        }
    }

    private void downPatroling()
    {
        downSequence = DOTween.Sequence();
        downPos[0] = new Vector3(wayroot.GetChild(0).transform.position.x, wayroot.transform.position.y-3f, transform.position.z);


        downSequence.Append(transform.DOPath(downPos, 3, PathType.CatmullRom, PathMode.TopDown2D, resolution: 10).SetEase(Ease.Linear)
            .SetId("down").OnComplete(
            () => searchPath()

            ));
    }

    private void starting()
    {
        startingSequence = DOTween.Sequence();
        startPos[0] = new Vector3(transform.position.x, wayroot.transform.position.y, transform.position.z);


        startingSequence.Append(transform.DOPath(startPos, 3, PathType.CatmullRom, PathMode.TopDown2D, resolution: 10).SetEase(Ease.Linear)
            .SetId("start").OnComplete(
            () => {
                searchPath();
                readyToKill = true;
            }

            ));
    }

   
    
}
