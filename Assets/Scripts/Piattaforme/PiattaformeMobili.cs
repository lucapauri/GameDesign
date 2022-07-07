using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PiattaformeMobili : MonoBehaviour
{
    public Transform platformWay;

    public Sequence walkingSequence;
    private float pathDuration = 12f;

    public float movSpeed;
    private bool started;

    // Start is called before the first frame update
    void Start()
    {
        //searchPath();

        movSpeed = 0.8f;
        started = false;

        float startTime = Random.Range(0.01f, 3f);
        StartCoroutine(startingCoroutine(startTime));
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (started)
        {
            Vector3 movVec = Vector3.up * movSpeed * Time.deltaTime;
            transform.Translate(movVec);
        }

    }


    public IEnumerator movSpeedCoroutine()
    {
        yield return new WaitForSeconds(4f);
        movSpeed = movSpeed * -1;
        StartCoroutine(movSpeedCoroutine());
    }


    public IEnumerator startingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        started = true;
        StartCoroutine(movSpeedCoroutine());
        
    }





}
