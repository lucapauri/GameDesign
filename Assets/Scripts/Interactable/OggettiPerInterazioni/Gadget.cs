using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gadget : MonoBehaviour
{
    private Justin justin;
    private GlobalVariables globalVariables;
    private float rightDis = 2f;
    private Animator anim;
    private BucoNero bucoNero;
    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        justin = globalVariables.justin;
        anim = GetComponent<Animator>();
        bucoNero = FindObjectOfType<BucoNero>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (justin == null && globalVariables.justin!= null)
        {
            justin = globalVariables.justin;
        }


        float vertDistance = Mathf.Abs(transform.position.y - justin.transform.position.y);
        float horizDistance = Mathf.Abs(transform.position.x - justin.transform.position.x);
        bool rightPosV = vertDistance < rightDis;
        bool rightPosH= horizDistance < rightDis;

        if (rightPosH && rightPosV )
        {
            Debug.Log("right");
            anim.SetTrigger("TurnOn");
            bucoNero.gadgetOn++;
            this.enabled = false;
        }


    }
}
