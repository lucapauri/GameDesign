using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DinoEgg : MonoBehaviour
{
    private Sequence shakeSequence;

    private float duration = 2f;
    private Vector3 strength = new Vector3(3f, 0f, 3f);
    private float randomness = 50f;
    private int vibrato = 15;
    private bool fadeout = true;


    void Start()
    {

        shakeSequence = DOTween.Sequence();

        shakeSequence.Append(transform.DOShakeScale(duration, strength, vibrato, randomness, fadeout).SetId("shaking").OnComplete(
              () => Destroy(this.gameObject)
              ));

    }

    
}
