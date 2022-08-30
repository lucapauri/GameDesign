using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Justin : MonoBehaviour
{

    //moving variables
    private Vector3 previousInputVector;
    NewControls controls;
    [SerializeField] private Transform _cameraT;
    [SerializeField] private float _speed = 5f;
    private Transform planeDown;
    private Transform planeUp;
    private CharacterController _characterController;
    public Transform armarture;
    public Transform chiattone;
    private Vector3 _inputVector;
    private float _inputSpeed;
    private float rotationSpeed = 1000f;
    private Vector3 _targetDirection;
    private Vector3 _velocity;
    private bool lastGrounded;
    private float initSpeed;
    [SerializeField] private float _dashSpeed = 30f;
    private bool teleporting;

    //jump variables
    [SerializeField] private float _gravity;
    public float _groundDistance;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _jumpMask;
    [SerializeField] private LayerMask _platMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float _jumpHeight;
    public bool _isGrounded;
    private bool highJump;
    private bool onMovingPlat;


    // world interactions variables
    public GlobalVariables globalVariables;
    public Bullet bulletPrefab;
    private float shootForce = 30f;
    private float shootReloadTime = 2f;
    private bool gunLoaded;
    private float dashTime = 0.2f;
    public bool _dash;
    private float maxDashCheckDistance = 3f;
    [SerializeField] private LayerMask dashLayerMask;
    private float maxInteractCheckDistance = 10f;
    public Interactable interactable;
    private GameObject timeCapsule;
    private InventoryMenu inventoryMenu;
    public GameObject valigettaPrefab;
    public GameObject fulminePrefab;
    public GameObject pistolaPrefab;
    public bool gunTaken;
    public bool valigettaTaken;
    private bool timeTravel;
    private bool destroyed;
    public int inputMul;
    private bool onMovingEnemy;

    //animation variables
    private Animator animator;
    private float movingTime;
    private AnimationClip[] clips;
    private float grabTime;
    private float throwTime;
    private float fireTime;

    // audio variables
    public AudioSource source;

    private MenuPause menuPause;

    private void Awake()
    {
        menuPause = FindObjectOfType<MenuPause>();
        switch (globalVariables.lv)
        {
            case GlobalVariables.livello.hub:
                inputMul = 1;
                break;
            case GlobalVariables.livello.vietnam:
                inputMul = -1;
                break;
            case GlobalVariables.livello.usa:
                inputMul = -1;
                break;
        }
        controls = new NewControls();
        controls.JustinController.Jump.performed += ctx => Jump();
        controls.JustinController.TimeTravel.performed += ctx => TimeTravel();
        controls.JustinController.Shoot.performed += ctx => ShootInput();
        controls.JustinController.Dash.performed += ctx => Dash();
        controls.JustinController.InputSpeed.performed += ctx => { 
            if ((onMovingEnemy && ctx.ReadValue<float>() > 0))
            {
                _inputSpeed = 0f;
            }else
                _inputSpeed = inputMul * ctx.ReadValue<float>();
        };
        controls.JustinController.InputSpeed.canceled += ctx => _inputSpeed = 0f;
        controls.JustinController.Grab.performed += ctx => Grab();
        controls.JustinController.OpenInventory.performed += ctx =>
        {
            if (!destroyed)
            {
                inventoryMenu.setMenuTrue();
            }
        };
        controls.JustinController.OpenTimeCapsule.performed += ctx => OpenTimeCapsule();
        controls.JustinController.MenuOpen.performed += ctx => OpenMenu();
    }

    private void OnEnable()
    {
        controls.JustinController.Enable();
    }

    private void OpenMenu()
    {
        menuPause.SetMenuTrue();
    }

    private void OpenTimeCapsule()
    {
        if (!destroyed)
        {
            if (timeCapsule != null && Vector3.Distance(timeCapsule.transform.position, transform.position) < 20)
            {
                inventoryMenu.setBehaviour();
                inventoryMenu.setMenuTrue();
            }
        }
    }

    private void Grab()
    {
        if (!destroyed)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(transform.position + transform.up * 1.5f, transform.forward);
            if (Physics.Raycast(ray, out hitInfo, maxInteractCheckDistance))
            {
                interactable = hitInfo.transform.GetComponent<InventoryObject>();
                if (interactable)
                {
                    animator.SetTrigger("Grab");
                    StartCoroutine(grabEndingCoroutine(grabTime / 4));
                }
            }
        }
    }

    private void Jump()
    {
        if (!destroyed)
        {
            if (gameObject.GetComponent<Justin>().isActiveAndEnabled)
            {
                if (_isGrounded && !highJump)
                {
                    _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                    animator.SetTrigger("Jump");

                }
                if (highJump)
                {
                    _velocity.y = Mathf.Sqrt(_jumpHeight * -4f * _gravity);
                    animator.SetTrigger("Jump");

                }

                AudioClip track = Resources.Load("Audio/Justin/jump") as AudioClip;
                source.clip = track;
                source.pitch = 1;
                source.Play();
            }
        }
    }

    private void TimeTravel()
    {
        if (valigettaTaken && globalVariables.currentTimeline > 0 && !timeTravel)
        {
            teleporting = true;
            timeTravelDown();
            timeTravel = true;
        }
        //spostarsi sulla timeline sovrastante
        if (valigettaTaken && globalVariables.currentTimeline < 1 && !timeTravel)
        {
            teleporting = true;
            timeTravelUp();
            timeTravel = true;
        }
        AudioClip track = Resources.Load("Audio/Justin/tp_arrivo") as AudioClip;
        source.clip = track;
        source.pitch = 1;
        source.Play();
    }

    private void ShootInput()
    {
        if (gunLoaded == true && gunTaken && !destroyed)
        {
            teleporting = true;
            animator.SetTrigger("Fire");
            Shoot();
        }
    }


    void Start()
    {
        previousInputVector = new Vector3(0, 0, 0);
        movingTime = 1;
        teleporting = false;
        lastGrounded = true;
        //trovo gli script necessari
        globalVariables = FindObjectOfType<GlobalVariables>();
        globalVariables.justin = this;
        simpleEnemy[] enemies = FindObjectsOfType<simpleEnemy>();
        foreach (simpleEnemy enemy in enemies)
        {
            enemy.justin = this;
        }

        _gravity = globalVariables.gravity;
        _jumpHeight = globalVariables.jumpHeight;

        _characterController = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();

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

        destroyed = false;
        gunLoaded = true;
        _dash = false;
        //_gravity = -9.81f;

        if (transform.Find("fulmine_unity(Clone)") != null && transform.Find("valigetta_unity(Clone)") != null)
        {
            Destroy(transform.Find("fulmine_unity(Clone)").gameObject);
            Destroy(transform.Find("valigetta_unity(Clone)").gameObject);
        }


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

        timeTravel = false;

        AudioClip track = Resources.Load("Audio/Justin/tp_partenza") as AudioClip;
        source.clip = track;
        source.pitch = 2;
        source.Play();
        source.enabled = true;
        GetComponent<AudioListener>().enabled = true;
        GetComponentInChildren<AudioSource>().enabled = true;
        initSpeed = _speed;
    }


    void Update()
    {
        if (!_isGrounded && movingTime != 1 && !_dash)
            _speed = initSpeed / Mathf.Log(movingTime);
        if (_isGrounded && !_dash)
            _speed = initSpeed;

        if (teleporting && _isGrounded)
        {
            _inputSpeed = 0f;
        }
       
        //Ground Check
        //_isGrounded = Physics.CheckSphere(transform.position, _groundDistance, _groundMask);

        RaycastHit groundInfo;
        Ray groundRay = new Ray(transform.position, -transform.up);
        _isGrounded = Physics.Raycast(groundRay, out groundInfo, _groundDistance, _groundMask);

        RaycastHit jumpInfo;
        Ray jumpRay = new Ray(transform.position, -transform.up);
        highJump = Physics.Raycast(jumpRay, out jumpInfo, _groundDistance, _jumpMask);

        RaycastHit platInfo;
        Ray platRay = new Ray(transform.position, -transform.up);
        onMovingPlat = Physics.Raycast(platRay, out platInfo, _groundDistance, _platMask);

        RaycastHit enemyInfo;
        Ray enemyRay = new Ray(transform.position + transform.up * 1f, transform.forward);
        onMovingEnemy = Physics.Raycast(enemyRay, out enemyInfo, 5f, enemyMask);

        if (onMovingPlat)
        {
            _characterController.Move(transform.up * platInfo.collider.gameObject.GetComponent<PiattaformeMobili>().movSpeed * Time.deltaTime);
            _velocity.y += - _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
            _velocity.y += - _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        
        //animazione
        if (Mathf.Abs(_inputSpeed) > 0.0001f && _isGrounded)
        {
            animator.SetBool("Walk", true);
        }
        else if (movingTime < 1.1f)
        {
            animator.SetBool("Walk", false);
        }
        if (Math.Abs(_inputSpeed) < 0.1 && movingTime >= 1.1)
        {
            movingTime -= 0.1f;
        }
        else if (movingTime < 2.7 && Math.Abs(_inputSpeed) > 0.1)
        {
            movingTime = movingTime + 0.015f;
        }
        animator.SetFloat("Blend", Mathf.Log(movingTime));
        

        //_inputSpeed = Input.GetAxis("Horizontal");
        _inputVector = new Vector3(-1 * _inputSpeed, 0, 0);




        //raycast per gli interactable
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + transform.up * 1.5f, transform.forward);
        /*if (Physics.Raycast(ray, out hitInfo, maxInteractCheckDistance))
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
        }*/

        //raycasting per togliere i collider ai nemici in dash
        if (Physics.Raycast(ray, out hitInfo, maxDashCheckDistance, dashLayerMask) && !_dash && _inputSpeed > 0f)
        {
            
            _inputSpeed = 0;

        }



        //raycasting per togliere i collider ai nemici in dash
        if (Physics.Raycast(ray, out hitInfo, maxDashCheckDistance, dashLayerMask) && _dash)
        {

            hitInfo.collider.enabled = false;
            StartCoroutine(dashEndingCoroutine(1f, hitInfo.collider));

        }


        _targetDirection = _cameraT.TransformDirection(_inputVector).normalized;
        _targetDirection.y = 0f;
        _characterController.Move(transform.forward * Mathf.Abs(_inputSpeed) * _speed * Mathf.Log(movingTime) * Time.deltaTime);

        if (_inputVector == Vector3.zero)
            _inputVector = previousInputVector;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, _inputVector, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir, transform.up);
        previousInputVector = _inputVector;



        //dying

        if (globalVariables.justinLife == 0)
        {
            
            Destroy(gameObject);
            destroyed = true;
        }


        //ANIMAZIONE-->

        //camminata
        




        //gravity

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);


        //COMANDI-->

        //jumping
        /*if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !highJump)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            animator.SetTrigger("Jump");

        } 
        if (Input.GetKeyDown(KeyCode.Space) && highJump)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -4f * _gravity);
            animator.SetTrigger("Jump");

        }

        //aprire l'inventario
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryMenu.setMenuTrue();
        }


        //spostarsi sulla timeline sottostante
        if (Input.GetKeyDown(KeyCode.M) && valigettaTaken  && globalVariables.currentTimeline > 0 && !timeTravel)
        {
            timeTravelDown();
            timeTravel = true;
        }
        //spostarsi sulla timeline sovrastante
        if (Input.GetKeyDown(KeyCode.N) && valigettaTaken && globalVariables.currentTimeline < 1 && !timeTravel)
        {
            timeTravelUp();
            timeTravel = true;
        }
        //comando di sparo
        if (Input.GetKeyDown(KeyCode.S) && gunLoaded == true && gunTaken)
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

        //attivazione e chiusura menu inventario davanti a capsula del tempo
        if (timeCapsule != null && Vector3.Distance(timeCapsule.transform.position, transform.position) < 20 && Input.GetKeyDown(KeyCode.X))
        {
            inventoryMenu.setMenuTrue();
            inventoryMenu.setBehaviour();
        }
        */
        if(!lastGrounded && _isGrounded)
        {
            AudioClip track = Resources.Load("Audio/Justin/Landing") as AudioClip;
            source.clip = track;
            source.Play();
        }
        lastGrounded = _isGrounded;
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

        _gravity = -4f;
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

        _gravity = -4f;
        StartCoroutine(teleportUpCoroutine(throwTime/4, valigetta, fulmine));
    }

    //funzione per sparare
    private void Shoot()
    {
        GameObject pistola = Instantiate(pistolaPrefab, transform.position, transform.rotation);
        pistola.transform.localScale = transform.localScale;
        pistola.transform.SetParent(this.transform);

        StartCoroutine(shootCoroutine(fireTime/4));
        StartCoroutine(destroyGunCoroutine(fireTime/2));
        StartCoroutine(timeToShootCoroutine(shootReloadTime));

    }

    //funzione per il dash
    private void Dash()
    {
        if (!destroyed && !onMovingEnemy && !teleporting)
        {
            _speed = _dashSpeed/Mathf.Log(movingTime);
            _dash = true;
            AudioClip track = Resources.Load("Audio/Justin/dash") as AudioClip;
            source.clip = track;
            source.pitch = 1;
            source.Play();
            StartCoroutine(dashEndingCoroutine(dashTime));
        }

    }




    //COROUTINE-->

    //couroutine per aspettare il tempo di ricarica della pistola
    private IEnumerator timeToShootCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        gunLoaded = true;
    }

    private IEnumerator destroyGunCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(transform.Find("pistola_unity(Clone)").gameObject);
        teleporting = false;
    }

    private IEnumerator shootCoroutine(float time)
    {
        gunLoaded = false;
        yield return new WaitForSeconds(time);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2.5f + transform.up * 1f, Quaternion.identity);
        bullet.transform.up = transform.forward;

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(transform.forward * shootForce, ForceMode.Impulse);

    }



    //couroutine per resettare la velocità dopo il dash
    private IEnumerator dashEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        _speed = initSpeed;
        _dash = false;
    }

    //couroutine per riattivare i collider dopo il dash
    private IEnumerator dashEndingCoroutine(float time, Collider collider)
    {
        yield return new WaitForSeconds(time);
        collider.enabled = true;
        _speed = initSpeed;
        _dash = false;
    }


    //couroutine per gestire grab
    private IEnumerator grabEndingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        interactable.Interact(gameObject);
        globalVariables.inventory.Add(interactable.gameObject.name, interactable.gameObject);
        inventoryMenu.addButton(interactable.gameObject.name);
        Destroy(interactable.gameObject);
        AudioClip track = Resources.Load("Audio/Justin/Grab") as AudioClip;
        source.clip = track;
        source.pitch = 1;
        source.Play();

    }

    //coroutine per l'animazione di teletrasporto
    private IEnumerator teleportUpCoroutine(float time, GameObject valigetta, GameObject fulmine)
    {
        yield return new WaitForSeconds(time);
        globalVariables.currentTimeline++;
        _gravity = -9.81f;

        float groundDistance = transform.position.y - planeDown.position.y +1f;
        Vector3 newPosition = new Vector3(transform.position.x, planeUp.position.y + groundDistance, planeUp.position.z);
        Quaternion newRotation = transform.rotation;
        Justin justin = Instantiate(this, newPosition, newRotation);
        justin._speed = this.initSpeed;
        justin.enabled = true;
        justin.gameObject.GetComponent<Animator>().enabled = true;

        justin.gameObject.name = "Justin";
        Destroy(this.gameObject);
        destroyed = true;

    }

    //coroutine per l'animazione di teletrasporto
    private IEnumerator teleportDownCoroutine(float time, GameObject valigetta, GameObject fulmine)
    {
        yield return new WaitForSeconds(time);
        globalVariables.currentTimeline--;
        _gravity = -9.81f;
        float groundDistance = transform.position.y - planeUp.position.y;
        Vector3 newPosition = new Vector3(transform.position.x, planeDown.position.y + groundDistance, planeDown.position.z);
        Quaternion newRotation = transform.rotation;

        

        Justin justin = Instantiate(this, newPosition, newRotation);
        justin._speed = this.initSpeed;
        justin.enabled = true;
        justin.gameObject.GetComponent<Animator>().enabled = true;

        justin.gameObject.name = "Justin";
        Destroy(this.gameObject);
        destroyed = true;
    }







}
