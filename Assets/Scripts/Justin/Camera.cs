using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private float _inputSpeed;
    private float _speed = 5f;

    private CharacterController _characterController;
    private GlobalVariables globalVariables;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        globalVariables = FindObjectOfType<GlobalVariables>();

    }

    // Update is called once per frame
    void Update()
    {
        // _inputSpeed = Input.GetAxis("Horizontal");
        //_characterController.Move(transform.right * _inputSpeed * _speed * Time.deltaTime);

        transform.position = new Vector3(globalVariables.justin.transform.position.x, globalVariables.justin.transform.position.y, transform.position.z);

    }
}
