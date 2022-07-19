using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoglieJumpable : MonoBehaviour
{
    NewControls controls;
    private float rightVerticalDistance;
    private float rightHorizontalDistance;

    public GameObject colliderPlaceholder;
    private GlobalVariables globalVariables;
    private Animator anim;

    private void Awake()
    {
        controls = new NewControls();
        controls.JustinController.Jump.performed += ctx => JumpHigh();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    private void JumpHigh()
    {
        float verticalJustinDistance = Mathf.Abs(globalVariables.justin.transform.position.y - transform.position.y);
        float horizontalJustinDistance = Mathf.Abs(globalVariables.justin.transform.position.x - transform.position.x);

        bool rightJustinPositionV = verticalJustinDistance < rightVerticalDistance;
        bool rightJustinPositionH = horizontalJustinDistance < rightHorizontalDistance;
        if (rightJustinPositionH && rightJustinPositionV)
        {
            anim.SetTrigger("Launch");

        }
    }

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
     
    }




}
