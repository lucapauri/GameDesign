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
    private AudioSource source;
    private AudioClip track;


    void Start()
    {
        source = GetComponent<AudioSource>();
        track = Resources.Load("Audio/Oggetti/Uovo") as AudioClip;
        source.clip = track;
        source.pitch = 1.5f;
        source.Play();
        

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
        Quaternion instRot = Quaternion.LookRotation(Vector3.right, Vector3.up);
        GameObject ptero=Instantiate(dino, this.transform.position, instRot);
        BabyPtero script=ptero.GetComponent<BabyPtero>();
        script.enabled = true;

    }

    
}
