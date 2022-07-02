using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogCamera : MonoBehaviour
{

    public float moveTime;
    public Sequence walkingSequence;
    public Transform dialogEndPoint;
    private Vector3[] pathPositions = new Vector3[2];



    // Start is called before the first frame update
    void Start()
    {
        pathPositions[0] = transform.position;
        pathPositions[1] = dialogEndPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveCamera()
    {
        walkingSequence = DOTween.Sequence();
        walkingSequence.Append(transform.DOPath(pathPositions, moveTime, PathType.CatmullRom, PathMode.Full3D, resolution: 10).SetEase(Ease.InOutSine)
               .SetLookAt(0.01f).SetId("woving"));
           
    }
}
