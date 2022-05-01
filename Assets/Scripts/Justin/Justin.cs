using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Justin : MonoBehaviour
{

    //moving variables
    [SerializeField] private Transform _cameraT;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 3f;
    private CharacterController _characterController;
    public Transform armarture;
    public Transform chiattone;
    private Vector3 _inputVector;
    private float _inputSpeed;
    private Vector3 _targetDirection;
    private Vector3 _velocity;

    //jump variables
    [SerializeField] private float _gravity;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpHeight = 3f;
    private bool _isGrounded;


    // world interactions variables
    public GlobalVariables globalVariables;
    private Camera[] activeCamera;
    public Bullet bulletPrefab;
    private float shootForce=30f;
    private float shootReloadTime = 10f;
    private bool gunLoaded;
    private float dashTime = 0.5f;
    public bool _dash;
    private float maxDashCheckDistance = 15f;
    [SerializeField] private LayerMask dashLayerMask;
    private float maxInteractCheckDistance = 5f;
    public Interactable interactable;
    private GameObject timeCapsule;
    private InventoryMenu inventoryMenu;

    //animation variables
    private Animator animator;
    private float movingTime;
    private AnimationClip[] clips;
    private float grabTime;




    void Start()
    {

        //trovo gli script necessari
        globalVariables = FindObjectOfType<GlobalVariables>();
        simpleEnemy[] enemies = FindObjectsOfType<simpleEnemy>();
        foreach (simpleEnemy enemy in enemies)
        {
            enemy.justin = this;
        }


        _characterController = GetComponent<CharacterController>();

        //trovo la capsula del tempo
        timeCapsule = GameObject.FindGameObjectWithTag("TimeCapsule");
        //trovo il menu inventario (canvas)
        inventoryMenu = FindObjectOfType<InventoryMenu>();
        //trovo l'animator
        animator = GetComponent<Animator>();

        //inizializzo variabili

        gunLoaded = true;
        _dash = false;
        _gravity = -9.81f;


        activeCamera = FindObjectsOfType<Camera>();
        foreach (Camera camera in activeCamera)
        {
            camera.transform.parent = transform;
        }

        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.justin = this;
            enemy.target = this.gameObject;
        }

        clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "grab":
                    grabTime = clip.length;
                    break;

                default:
                    break;
            }
        }

    }


    void Update()
    {
        

            //Ground Check
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);


            if (_isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

           
            _inputSpeed = Input.GetAxis("Horizontal");


            _targetDirection = _cameraT.TransformDirection(_inputVector).normalized;
            _targetDirection.y = 0f;
            _characterController.Move(transform.forward * _inputSpeed * _speed * Time.deltaTime);


            //animazione
            if (Math.Abs(_inputSpeed) > 0.1)
                animator.SetBool("Walk", true);
            if (Math.Abs(_inputSpeed) < 0.1)
                animator.SetBool("Walk", false);
            if (!animator.GetBool("Walk") || _inputSpeed < 0)
                movingTime = 0;
            else
                movingTime = movingTime + 0.003f;
            animator.SetFloat("Blend", movingTime);

            //gravity

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);


            //raycast per gli interactable
            RaycastHit hitInfo;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hitInfo, maxInteractCheckDistance))
            {
                interactable = hitInfo.transform.GetComponent<InventoryObject>();
                if (interactable)
                {

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                    animator.SetTrigger("Grab");
                    StartCoroutine(grabEndingCoroutine(grabTime / 4));  
                    }

                }
            }

            //raycasting per togliere i collider ai nemici in dash
            if (Physics.Raycast(ray, out hitInfo, maxDashCheckDistance, dashLayerMask) && _dash == true)
            {

                hitInfo.collider.enabled = false;
                StartCoroutine(dashEndingCoroutine(1f, hitInfo.collider));

            }

        /*//raycasting per il climbing
        RaycastHit hitClimb;
        Ray rayClimb = new Ray(transform.position, -transform.right);
        if (Physics.Raycast(rayClimb, out hitClimb, 10f, ClimbLayerMask) && Input.GetKeyDown(KeyCode.X))
        {
            _gravity = 0f;
            climbing = true;
        }


        //climbing
        if (climbing == true)
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _characterController.Move(transform.up * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                _characterController.Move(-transform.up * _speed * Time.deltaTime);

                if (_isGrounded)
                {
                    climbing = false;
                    _gravity = -9.81f;
                }
            }


        }*/


        //dying

        if (globalVariables.justinLife == 0)
        {
            transform.DetachChildren();
            Destroy(armarture.gameObject);
            Destroy(chiattone.gameObject);
            Destroy(gameObject);
        }


        //ANIMAZIONE-->

        //camminata
        if (Mathf.Abs(_inputSpeed)> 0.1 && _isGrounded)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }







        //COMANDI-->

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
                animator.SetTrigger("Jump");
                
            }

            //gravity
            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            //spostarsi sulla timeline sottostante
            if (Input.GetKeyDown(KeyCode.M) && _isGrounded && globalVariables.currentTimeline > 0)
            {
                timeTravelDown();
            }
            //spostarsi sulla timeline sovrastante
            if (Input.GetKeyDown(KeyCode.N) && _isGrounded && globalVariables.currentTimeline < 1)
            {
                timeTravelUp();
            }
            //comando di sparo
            if (Input.GetKeyDown(KeyCode.S) && gunLoaded == true)
            {
                Shoot();
            }

            //comando per il dash
            if (Input.GetKeyDown(KeyCode.J))
            {
                Dash();
            }

            //comando per usare un oggetto in inventario
            if (Input.GetKeyDown(KeyCode.Return) && globalVariables.inventory.Count > 0)
            {
                //useInventoryObject(string objectName);
            }
          
            //attivazione e chiusura menu inventario
            if (timeCapsule != null && Vector3.Distance(timeCapsule.transform.position, transform.position) < 4 && Input.GetKeyDown(KeyCode.X))
                inventoryMenu.setMenuTrue();

    }


    //passo al time sequence inferiore
    private void timeTravelDown()
    {
        globalVariables.currentTimeline--;
        transform.DetachChildren();
        _groundCheck.transform.parent = transform;
        armarture.transform.parent = transform;
        chiattone.transform.parent = transform;
        Vector3 newPosition= new Vector3(transform.position.x, transform.position.y - 22f, transform.position.z - 7.31f);
        Quaternion newRotation = transform.rotation;
        Destroy(this.gameObject);

        Justin justin=Instantiate(this, newPosition, newRotation);
        justin.enabled = true;
        justin.gameObject.GetComponent<Animator>().enabled = true;

        justin.gameObject.name = "Justin";
    }

    //passo al time sequence superiore
    private void timeTravelUp()
    {
        globalVariables.currentTimeline++;
        transform.DetachChildren();
        _groundCheck.transform.parent = transform;
        armarture.transform.parent = transform;
        chiattone.transform.parent = transform;
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 22f, transform.position.z +7.31f);
        Quaternion newRotation = transform.rotation;

        Destroy(this.gameObject);

        Justin justin = Instantiate(this, newPosition, newRotation);
        justin.enabled = true;
        justin.gameObject.GetComponent<Animator>().enabled = true;

        justin.gameObject.name = "Justin";
    }

    //funzione per sparare
    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward*2.5f + transform.up *2f, Quaternion.identity);
        bullet.transform.up = transform.forward;

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(transform.forward * shootForce, ForceMode.Impulse);

        gunLoaded = false;
        StartCoroutine(timeToShootCoroutine(shootReloadTime));

    }

    //funzione per il dash
    private void Dash()
    {
        _speed = 50f;
        _dash = true;
        StartCoroutine(dashEndingCoroutine(dashTime));

    }

    //funzione per usare un oggetto in inventario
    private void useInventoryObject(string objectName)
    {

    }







    //COROUTINE-->

    //couroutine per aspettare il tempo di ricarica della pistola
    private IEnumerator timeToShootCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        gunLoaded = true;
    }

    //couroutine per resettare la velocità dopo il dash
    private IEnumerator dashEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        _speed = 5f;
        _dash = false;
    }

    //couroutine per riattivare i collider dopo il dash
    private IEnumerator dashEndingCoroutine(float time, Collider collider)
    {
        yield return new WaitForSeconds(time);
        collider.enabled = true;
    }


    //couroutine per gestire grab
    private IEnumerator grabEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        interactable.Interact(gameObject);
        globalVariables.inventory.Add(interactable.gameObject.name, interactable.gameObject);
        inventoryMenu.addButton(interactable.gameObject.name);
        Debug.Log("oggetto aggiunto all'inventario");
        Destroy(interactable.gameObject);

    }






}
