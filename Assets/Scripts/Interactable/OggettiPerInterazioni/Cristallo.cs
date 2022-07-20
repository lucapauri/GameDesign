using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristallo : MonoBehaviour
{
    private GlobalVariables globalVariables;
    private AudioSource source;
    private AudioClip track;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        source = GetComponent<AudioSource>();
        track = Resources.Load("Audio/Oggetti/Cristallo") as AudioClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            globalVariables.crystalsNumber++;
            Destroy(gameObject);
            source.clip = track;
            source.pitch = 1;
            source.Play();
            Debug.Log("audioCristalloOn");
        }
    }


}
