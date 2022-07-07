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

    public Transform wayrootList;
    public Transform wayRoot;


    //prefab dei proiettili
    public BulletNemicoCity bulletPrefab;

    //dati del nemico
    public int enemyLife;
    public bool readyToPatrol;
    private AnimationClip[] clips;
    private float DeathTime;
    private bool dead;
    public float huntLimitDistance;

    //groundCheck
    private float _groundDistance = 1f;
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


        //cerco la wayroot giusta
        wayRoot = wayrootList.GetChild(0);
        for (int i = 0; i < wayrootList.childCount; i++)
        {
            Transform way = wayrootList.GetChild(i);
            float newDistance = Vector3.Distance(this.transform.position, way.position);
            float oldDistance = Vector3.Distance(this.transform.position, wayRoot.position);

            if (newDistance <= oldDistance)
            {
               wayRoot = way;
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
        finiteStateMachine.AddTransition(patrolState, fallingState, () => currentStatus == MachineStatus.Falling);
        finiteStateMachine.AddTransition(attackState, fallingState, () => currentStatus == MachineStatus.Falling);
        finiteStateMachine.AddTransition(fallingState, patrolState, () => currentStatus == MachineStatus.Patrol);
        finiteStateMachine.AddTransition(fallingState, attackState, () => currentStatus == MachineStatus.Attack);


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

        huntLimitDistance = Mathf.Abs(wayRoot.transform.position.x - wayRoot.GetChild(0).transform.position.x);

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
        _isGrounded = !Physics.Raycast(groundRay, out groundInfo, _groundDistance, _enemyGroundMask);

        if (_isGrounded)
        {
            currentStatus = MachineStatus.Falling;
        }

        finiteStateMachine.Tik();



        if (enemyLife == 0 && dead == false)
        {
            dead = true;
            if (GetComponent<Animator>())
            {
                enemyAnimator.SetTrigger("Death");
            }
            StartCoroutine(DeathCoroutine(DeathTime));
        }


    }

    //funzione di attacco, unica per tutti i tipi di nemici
    public void attack()
    {
        readyToPatrol = false;
        BulletNemicoCity bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 3f + transform.up * 2f, Quaternion.identity);
        bullet.shooter = this;
        bullet.transform.up = transform.forward;
       
  }

    public IEnumerator DeathCoroutine(float timeToEndAnim)
    {

        yield return new WaitForSeconds(timeToEndAnim);
        Destroy(this.gameObject);
    }




}
