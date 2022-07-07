using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PiattaformeMobili : MonoBehaviour
{
    public Transform platformWay;

    public Sequence walkingSequence;
    private float pathDuration = 12f;

    // Start is called before the first frame update
    void Start()
    {
        searchPath();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void searchPath()
    {
        if ( platformWay!= null && platformWay.childCount > 0)
        {
            Vector3[] pathPositions = new Vector3[platformWay.childCount];
            for (int i = 0; i < platformWay.childCount; i++)
            {
                pathPositions[i] = platformWay.GetChild(i).position;
                pathPositions[i].z = transform.position.z;
                pathPositions[i].x = transform.position.x;
            }
            walkingSequence = DOTween.Sequence();

            walkingSequence.Append(transform.DOPath(pathPositions, pathDuration, PathType.CatmullRom, PathMode.Full3D, resolution: 10).SetEase(Ease.Linear)
                .SetId("walking").OnComplete(
                () => searchPath()

                ));

        }


    }
}
