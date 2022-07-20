using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownCamera : MonoBehaviour
{
    private float _inputSpeed;
    private float _speed = 5f;

    private float verticalShift;
    private float horizOffset = 0f;
    public float vertOffset;
    private float offsetSmoothing = 1f;

    private CharacterController _characterController;
    private GlobalVariables globalVariables;

    private Vector3 futurePos;
    public Transform justin;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        globalVariables = FindObjectOfType<GlobalVariables>();

    }

    // Update is called once per frame
    void Update()
    {
        justin = globalVariables.justin.transform;

        if (justin != null)
        {
            if (globalVariables.currentTimeline == 1)
            {
                verticalShift = globalVariables.justin.transform.position.y - globalVariables.upPlaneHeight;
            }
            if (globalVariables.currentTimeline == 0)
            {
                verticalShift = globalVariables.justin.transform.position.y - globalVariables.downPlaneHeight;
            }

            if (justin.transform.forward.x > 0f)
            {
                futurePos = new Vector3(justin.position.x + horizOffset, globalVariables.downPlaneHeight + verticalShift + vertOffset, transform.position.z);
            }

            else
            {
                futurePos = new Vector3(justin.position.x - horizOffset, globalVariables.downPlaneHeight + verticalShift + vertOffset, transform.position.z);
            }

            transform.position = Vector3.Lerp(transform.position, futurePos, offsetSmoothing * Time.deltaTime);
        }

    }
}
