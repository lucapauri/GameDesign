using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoglieJumpable : MonoBehaviour
{

    private float rightVerticalDistance;
    private float rightHorizontalDistance;

    public GameObject colliderPlaceholder;
    private GlobalVariables globalVariables;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rightHorizontalDistance = colliderPlaceholder.GetComponent<Collider>().bounds.extents.x;
        rightVerticalDistance = 6f;
        globalVariables = FindObjectOfType<GlobalVariables>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
     
        float verticalJustinDistance = Mathf.Abs(globalVariables.justin.transform.position.y - transform.position.y);
        float horizontalJustinDistance = Mathf.Abs(globalVariables.justin.transform.position.x - transform.position.x);

        bool rightJustinPositionV = verticalJustinDistance < rightVerticalDistance;
        bool rightJustinPositionH = horizontalJustinDistance < rightHorizontalDistance;

        if (rightJustinPositionH && rightJustinPositionV && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Launch");

        }


    }




}
