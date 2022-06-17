using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Justin : MonoBehaviour
{

    //moving variables
    [SerializeField] private Transform _cameraT;
    private float _speed = 5f;
    private Transform planeDown;
    private Transform planeUp;
    private CharacterController _characterController;
    public Transform armarture;
    public Transform chiattone;
    private Vector3 _inputVector;
    private float _inputSpeed;
    private float rotationSpeed = 100f;
    private Vector3 _targetDirection;
    private Vector3 _velocity;

    //jump variables
    [SerializeField] private float _gravity;
     private float _groundDistance = 1f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpHeight = 3f;
    private bool _isGrounded;


    // world interactions variables
    public GlobalVariables globalVariables;
    public Bullet bulletPrefab;
    private float shootForce = 30f;
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
    public GameObject valigettaPrefab;
    public GameObject fulminePrefab;

    //animation variables
    private Animator animator;
    private float movingTime;
    private AnimationClip[] clips;
    private float grabTime;
    private float throwTime;
    private float fireTime;



    void Start()
    {

        //trovo gli script necessari
        globalVariables = FindObjectOfType<GlobalVariables>();
        globalVariables.justin = this;
        simpleEnemy[] enemies = FindObjectsOfType<simpleEnemy>();
        foreach (simpleEnemy enemy in enemies)
        {
            enemy.justin = this;
        }


        _characterController = GetComponent<CharacterController>();


        //trovo i piani per lo spostamento 
        planeDown = GameObject.FindGameObjectWithTag("PlaneDown").transform;
        planeUp = GameObject.FindGameObjectWithTag("PlaneUp").transform;

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

        transform.DetachChildren();
        armarture.SetParent(transform);
        chiattone.SetParent(transform);



        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.justin = this;
            if (enemy.special == simpleEnemy.Specials.none)
            {
                enemy.target = this.gameObject;
            }
        }

        clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Armature|grab":
                    grabTime = clip.length;
                    break;

                case "Armature|throw":
                    throwTime = clip.length;
                    break;

                case "Armature|sparo":
                    fireTime = clip.length;
                    break;

                default:
                    break;
            }
        }

    }


    void Update()
    {


        //Ground Check
        _isGrounded = Physics.CheckSphere(transform.position, _groundDistance, _groundMask);

   
        if (_isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }


        _inputSpeed = Input.GetAxis("Horizontal");
        _inputVector = new Vector3(-1 * _inputSpeed, 0, 0);



        _targetDirection = _cameraT.TransformDirection(_inputVector).normalized;
        _targetDirection.y = 0f;
        _characterController.Move(transform.forward * Mathf.Abs(_inputSpeed) * _speed * Time.deltaTime);


        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, _inputVector, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir, transform.up);





        //animazione
        if (Math.Abs(_inputSpeed) > 0.1)
            animator.SetBool("Walk", true);
        if (Math.Abs(_inputSpeed) < 0.1)
            animator.SetBool("Walk", false);
        if (!animator.GetBool("Walk") || Math.Abs(_inputSpeed) < 0.1)
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
        if (Mathf.Abs(_inputSpeed) > 0.1 && _isGrounded)
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
        if (Input.GetKeyDown(KeyCode.M)  && globalVariables.currentTimeline > 0)
        {
            timeTravelDown();
        }
        //spostarsi sulla timeline sovrastante
        if (Input.GetKeyDown(KeyCode.N)  && globalVariables.currentTimeline < 1)
        {
            timeTravelUp();
        }
        //comando di sparo
        if (Input.GetKeyDown(KeyCode.S) && gunLoaded == true)
        {
            animator.SetTrigger("Fire");
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
        GameObject valigetta = Instantiate(valigettaPrefab, transform.position, transform.rotation);
        valigetta.transform.localScale = transform.localScale;

        GameObject fulmine = Instantiate(fulminePrefab, transform.position, transform.rotation);
        fulmine.transform.localScale = transform.localScale;

        valigetta.transform.SetParent(transform);
        fulmine.transform.SetParent(transform);


        animator.SetTrigger("Throw");
        StartCoroutine(teleportDownCoroutine(throwTime/4, valigetta, fulmine));

    }

    //passo al time sequence superiore
    private void timeTravelUp()
    {
        GameObject valigetta = Instantiate(valigettaPrefab, transform.position, transform.rotation);
        valigetta.transform.localScale = transform.localScale;

        GameObject fulmine = Instantiate(fulminePrefab, transform.position, transform.rotation);
        fulmine.transform.localScale = transform.localScale;

        valigetta.transform.SetParent(transform);
        fulmine.transform.SetParent(transform);

        animator.SetTrigger("Throw");
        StartCoroutine(teleportUpCoroutine(throwTime/4, valigetta, fulmine));
    }

    //funzione per sparare
    private void Shoot()
    {
        StartCoroutine(shootCoroutine(fireTime/2));
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

    private IEnumerator shootCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2.5f + transform.up * 2f, Quaternion.identity);
        bullet.transform.up = transform.forward;

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(transform.forward * shootForce, ForceMode.Impulse);

        gunLoaded = false;
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
        Debug.Log("colliderEnabled");
    }


    //couroutine per gestire grab
    private IEnumerator grabEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        interactable.Interact(gameObject);
        globalVariables.inventory.Add(interactable.gameObject.name, interactable.gameObject);
        Debug.Log("interactable");
        inventoryMenu.addButton(interactable.gameObject.name);
        Debug.Log("oggetto aggiunto all'inventario");
        Destroy(interactable.gameObject);

    }

    //coroutine per l'animazione di teletrasporto
    private IEnumerator teleportUpCoroutine(float time, GameObject valigetta, GameObject fulmine)
    {
        yield return new WaitForSeconds(time);
        globalVariables.currentTimeline++;
        /*transform.DetachChildren();
        _groundCheck.transform.parent = transform;
        armarture.transform.parent = transform;
        chiattone.transform.parent = transform;*/
        Vector3 newPosition = new Vector3(transform.position.x, planeUp.position.y + 1f, planeUp.position.z);
        Quaternion newRotation = transform.rotation;

        Destroy(this.gameObject);
        Destroy(valigetta);
        Destroy(fulmine);

        Justin justin = Instantiate(this, newPosition, newRotation);
        justin.enabled = true;
        justin.gameObject.GetComponent<Animator>().enabled = true;

        justin.gameObject.name = "Justin";
    }

    //coroutine per l'animazione di teletrasporto
    private IEnumerator teleportDownCoroutine(float time, GameObject valigetta, GameObject fulmine)
    {
        yield return new WaitForSeconds(time);
        globalVariables.currentTimeline--;

        Vector3 newPosition = new Vector3(transform.position.x, planeDown.position.y + 1f, planeUp.position.z);
        Quaternion newRotation = transform.rotation;

        Destroy(this.gameObject);
        Destroy(valigetta);
        Destroy(fulmine);

        Justin justin = Instantiate(this, newPosition, newRotation);
        justin.enabled = true;
        justin.gameObject.GetComponent<Animator>().enabled = true;

        justin.gameObject.name = "Justin";
    }







}
