using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundMenu : MonoBehaviour
{
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(firstCoroutine());
        StartCoroutine(soundChangeCoroutine());
    }

    private IEnumerator firstCoroutine()
    {
        yield return new WaitForSeconds(0);
        AudioClip track = Resources.Load("Audio/Soundtrack/JustinJingle") as AudioClip;
        source.clip = track;
        source.loop = false;
        source.pitch = 1;
        source.Play();

    }

    private IEnumerator soundChangeCoroutine()
    {
        yield return new WaitForSeconds(8.3f);
        AudioClip track = Resources.Load("Audio/Soundtrack/JustinFullTheme") as AudioClip;
        source.clip = track;
        source.loop = true;
        source.pitch = 1;
        source.Play();

    }
}
