using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnValigetta : MonoBehaviour
{
    public ValigettaEmpty valigetta;
    private GameObject justin;

    // Start is called before the first frame update
    void Start()
    {
        justin = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 10 && valigetta.taken())
        {
            //ritorno con valigetta
            Debug.Log("Sei tornato con la valigetta");
        }
    }
}
