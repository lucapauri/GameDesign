using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoSounds : MonoBehaviour
{
   
    private AudioSource source;
    private float maxDistance = 0.2f;
    public LayerMask groundMask;
    private bool oldRay;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        oldRay = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, -Vector3.up);
        bool newRay = (Physics.Raycast(ray, out hitInfo, maxDistance, groundMask));

        if (newRay != oldRay && !oldRay)
        {
            source.Play();
        }

        oldRay = newRay;
        
    }
}
