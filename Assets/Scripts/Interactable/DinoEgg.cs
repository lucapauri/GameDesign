using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DinoEgg : MonoBehaviour
{
    private Sequence shakeSequence;

    private float duration = 2f;
    private Vector3 strength = new Vector3(0.03f, 0f, 0.03f);
    private float randomness = 3f;
    private int vibrato = 3;
    private bool fadeout = true;
    public GameObject dino;


    void Start()
    {

        shakeSequence = DOTween.Sequence();

        shakeSequence.Append(transform.DOShakeScale(duration, strength, vibrato, randomness, fadeout).SetId("shaking").OnComplete(
              () => {
                  generateDino();
                  Destroy(this.gameObject);
              }
              )) ;

    }

    private void generateDino()
    {
        GameObject ptero=Instantiate(dino, this.transform.position, Quaternion.identity);
        BabyPtero script=ptero.GetComponent<BabyPtero>();
        script.enabled = true;

    }

    
}
