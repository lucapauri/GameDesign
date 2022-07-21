using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class NemicoCity : MonoBehaviour
{

    public Justin justin;
    public GlobalVariables globalVariables;

    public GameObject target;

    //Componenti del nemico
    public Animator enemyAnimator;
    public bool standing;


    private FiniteStateMachine<NemicoCity> finiteStateMachine;


    public int currentTimeline;
    public int originalTimeline;

    public Transform platformList;
    public Transform platform;
    public Vector3 oldPlatform;



    //prefab dei proiettili
    public BulletNemicoCity bulletPrefab;

    //dati del nemico
    public int enemyLife;
    public bool readyToPatrol;
    private AnimationClip[] clips;
    private float DeathTime;
    private bool dead;
    public float huntLimitDistance;
    public Transform shotPoint;

    //groundCheck
    private float _groundDistance = 0.3f;
    [SerializeField] private LayerMask _enemyGroundMask;
    public bool _isGrounded;


    public enum MachineStatus //stati della macchina agli stati finiti
    {
        Patrol,
        Attack,
        Falling
    }

    public enum Origin //enum per dire se il nemico è stato teletrasportato e dove
    {
        Original,
        TeleportedUp,
        TeleportedDown
    }

  


    public MachineStatus currentStatus;
    public Origin currentOrigin;
    public Type enemyType;
    



    // Start is called before the first frame update
    void Start()
    {
        
        globalVariables = FindObjectOfType<GlobalVariables>();
        justin = globalVariables.justin;


       
        platform = platformList.GetChild(0);
        for (int i = 0; i < platformList.childCount; i++)
        {
            Transform plat = platformList.GetChild(i);
            float newDistance = Vector3.Distance(this.transform.position, plat.position);
            float oldDistance = Vector3.Distance(this.transform.position, platform.position);

            if (newDistance <= oldDistance)
            {
                platform = plat;
            }
             

        }



        // voglio che il mio nemico sappia se si trova nella sua timeline oppure no
        switch (currentOrigin)
        {
            case Origin.Original:
                currentTimeline = originalTimeline;
                break;
            case Origin.TeleportedDown:
                currentTimeline = 0;
                break;
            case Origin.TeleportedUp:
                currentTimeline = 1;
                break;

        }

        dead = false;

        //cerco la durata delle clip di attacco, mi servirà per far matchare l'istante in cui il nemico colpisce con quello in cui il target perde vita
        if (GetComponent<Animator>())
        {
            enemyAnimator = GetComponent<Animator>();
            clips = enemyAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {

                    case "Death":
                        DeathTime = clip.length;
                        break;

                    default:
                        DeathTime = 0;
                        break;
                }
            }
        }



        enemyLife = 1;
        readyToPatrol = true;

        finiteStateMachine = new FiniteStateMachine<NemicoCity>(this);

        //STATES
        State patrolState = new PatrolStateCity("patrol", this);
        State attackState = new AttackStateCity("attack", this);
        State fallingState = new FallingStateCity("falling", this);


        //TRANSITIONS
        finiteStateMachine.AddTransition(patrolState, attackState, () => currentStatus == MachineStatus.Attack);
        finiteStateMachine.AddTransition(attackState, patrolState, () => currentStatus == MachineStatus.Patrol);
        //finiteStateMachine.AddTransition(patrolState, fallingState, () => currentStatus == MachineStatus.Falling);
        //finiteStateMachine.AddTransition(attackState, fallingState, () => currentStatus == MachineStatus.Falling);
        //finiteStateMachine.AddTransition(fallingState, patrolState, () => currentStatus == MachineStatus.Patrol);
        //finiteStateMachine.AddTransition(fallingState, attackState, () => currentStatus == MachineStatus.Attack);
        

        //scelgo lo stato iniziale
        if (standing)
        {
            finiteStateMachine.SetState(attackState);
            currentStatus = MachineStatus.Attack;
        }

        else
        {
            finiteStateMachine.SetState(patrolState);
            currentStatus = MachineStatus.Patrol;
        }

        huntLimitDistance = platform.GetComponent<Collider>().bounds.extents.x - 0.2f;

        target = justin.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (justin == null)
        {
            justin = globalVariables.justin;
            target = justin.gameObject;
        }


        RaycastHit groundInfo;
        Ray groundRay = new Ray(transform.position, -transform.up);
        _isGrounded = Physics.Raycast(groundRay, out groundInfo, _groundDistance, _enemyGroundMask);
        Debug.Log("grounded "+_isGrounded);
        if (!_isGrounded)
        {
            transform.Translate(-Vector3.up * Time.deltaTime*3f);
        }

        finiteStateMachine.Tik();



        if (enemyLife == 0 && dead == false)
        {
            dead = true;
            if (GetComponent<Animator>())
            {
                enemyAnimator.SetTrigger("Death");
            }
            StartCoroutine(DeathCoroutineAndRespawn(DeathTime));
        }


    }

    //funzione di attacco, unica per tutti i tipi di nemici
    public void attack()
    {
        readyToPatrol = false;
        BulletNemicoCity bullet = Instantiate(bulletPrefab, shotPoint.position, Quaternion.identity);
        bullet.GetComponent<BulletNemicoCity>().currentOrigin = BulletNemicoCity.Origin.Original;
        bullet.shooter = this;
        bullet.transform.up = transform.forward;
        bullet.transform.SetParent(shotPoint);
        bullet.shotPoint = shotPoint;

    }

    public IEnumerator DeathCoroutineAndRespawn(float timeToEndAnim)
    {

        yield return new WaitForSeconds(timeToEndAnim);
        GameObject go = Instantiate(this.gameObject, oldPlatform + Vector3.up*3f, transform.rotation); //sostituire oldPlatform con platform?
        NemicoCity script = go.GetComponent<NemicoCity>();
        script.enabled = true;
        switch (currentOrigin)
        {
            case Origin.Original:
                script.currentOrigin = Origin.Original;
                break;
            case Origin.TeleportedDown:
                script.currentOrigin = Origin.TeleportedUp;
                break;
            case Origin.TeleportedUp:
                script.currentOrigin = Origin.TeleportedDown;
                break;

        }
        Destroy(this.gameObject);
        

    }




}
