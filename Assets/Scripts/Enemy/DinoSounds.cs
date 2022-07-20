using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoSounds : MonoBehaviour
{
   
    private AudioSource source;
    private float maxDistance = 0.4f;
    public LayerMask groundMask;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, -Vector3.up);
        if ((Physics.Raycast(ray, out hitInfo, maxDistance, groundMask)) && !source.isPlaying)
        {
            source.Play();
        }
        
    }
}
