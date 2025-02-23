using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class simpleEnemy : MonoBehaviour
{

    public Justin justin;
    public GlobalVariables globalVariables;
    public Transform wayRoot;

    public GameObject target;

    //Componenti del nemico
    public Animator enemyAnimator;
    public CharacterController controller;
    public bool standing;
    public AudioSource source;


    private FiniteStateMachine<simpleEnemy> finiteStateMachine;


    public int currentTimeline;
    public int originalTimeline;

    //prefab dei proiettili
    public enemyBullet bulletPrefab;

    //dati del nemico
    public int enemyLife;
    public bool readyToPatrol;
    private AnimationClip[] clips;
    private float TrexAttackTime;
    private float DeathTime;
    private float TomahawkTime;
    private bool dead;
    public float huntLimitDistance;
    public bool onPlatform;
    public Transform shotPoint;

    //groundCheck
    private float _groundDistance = 1f;
    [SerializeField] private LayerMask _enemyGroundMask;
    public bool _isGrounded;
    private float _gravity = -9.81f;


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

    public enum Type //tipo di nemico
    {
        meleeEnemy,
        rangeEnemy
    }

    public enum Specials //enum per indicare i  nemici speciali
    {
        none,
        trex,
        robot,
        indiano
    }

    public MachineStatus currentStatus;
    public Origin currentOrigin;
    public Type enemyType;
    public Specials special;




    // Start is called before the first frame update
    void Start()
    {

        globalVariables = FindObjectOfType<GlobalVariables>();
        if (special != Specials.trex)
        {
            source = GetComponent<AudioSource>();
            source.enabled = true;
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

        if (GetComponent<CharacterController>())
        {
            controller = GetComponent<CharacterController>();
        }
        
        //cerco la durata delle clip di attacco, mi servirà per far matchare l'istante in cui il nemico colpisce con quello in cui il target perde vita
        if (GetComponent<Animator>())
        {
            enemyAnimator = GetComponent<Animator>();
            clips = enemyAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "TrexAttack":
                        TrexAttackTime = clip.length;
                        break;

                    case "Death":
                        DeathTime = clip.length;
                        break;
                    case "Armature|TomahawkLaunch":
                        TomahawkTime = clip.length;
                        break;

                    default:
                        DeathTime = 0;
                        break;
                }
            }
        }
      

        if (special == Specials.trex)
        {
            enemyLife = 1000;
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            source = sources[2];
        }
        else
        {
            enemyLife = 1;
        }


        readyToPatrol = true;

        finiteStateMachine = new FiniteStateMachine<simpleEnemy>(this);

        //STATES
        State patrolState = new PatrolState("patrol", this);
        State attackState = new AttackState("attack", this);
        State fallingState = new FallingState("falling", this);


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

        if (shotPoint == null)
        {
            shotPoint = transform;
        }

       
            globalVariables.enemies.Add(this);

        StartCoroutine(firstTargetCoroutine());

    }

    // Update is called once per frame
    void Update()
    {
        /*RaycastHit groundInfo;
        Ray groundRay = new Ray(transform.position, -transform.up);
        _isGrounded = !Physics.Raycast(groundRay, out groundInfo, _groundDistance, _enemyGroundMask);

        if (_isGrounded)
        {
            currentStatus = MachineStatus.Falling;
        }*/

        if (GetComponent<CharacterController>())
        {
            controller.Move(transform.up * Time.deltaTime * _gravity);
        }

        finiteStateMachine.Tik();



        if (enemyLife == 0 && dead==false)
        {
            dead = true;
            globalVariables.enemies.Remove(this);
            if (GetComponent<Animator>())
            {
                enemyAnimator.SetTrigger("Death");
            }
            AudioClip track = Resources.Load("Audio/Justin/Damage") as AudioClip;
            source.clip = track;
            source.pitch = 2;
            source.spatialBlend = 1;
            source.Play();
            //Debug.Log("audioEnemyDamageOn");
            StartCoroutine(DeathCoroutine(DeathTime));
        }


    }

    //funzione di attacco, unica per tutti i tipi di nemici
    public void attack()
    {

        switch (enemyType)
        {
            //definisco il comportamento per i nemici di tipo melee
            case simpleEnemy.Type.meleeEnemy:

                readyToPatrol = false;

                //attivo animazione di attacco
                if (GetComponent<Animator>())
                {
                    enemyAnimator.SetTrigger("Attack");
                }
                //controllo se il target è un intruso oppure justin
                if(target.GetComponent<simpleEnemy>()!=null)
                {
                   
                        //controllo se il nemico è speciale, se lo è cerco di far si che la vita venga tolta al target nell'istante in cui l'animazione di attacco effettivamento colpisce il bersaglio
                        switch (special)
                        {
                            case Specials.trex:
                            source.Stop();
                            StartCoroutine(hitTargetCoroutine(TrexAttackTime / 3));
                            break;

                            default:
                                StartCoroutine(hitTargetCoroutine(0.5f));
                                break;
                        }
                   

                }
                else
                {
                    //come sopra ma per justin
                    switch (special)
                    {
                        case Specials.trex:
                            source.Stop();
                            StartCoroutine(hitTargetCoroutine(TrexAttackTime / 3));
                            break;

                        default:
                            StartCoroutine(hitTargetCoroutine(0.5f));
                            break;
                    }
                }

                break;

                //definisco comportamento per nemici range
            case simpleEnemy.Type.rangeEnemy:

                switch (special)
                {
                    case Specials.indiano:
                        if (GetComponent<Animator>())
                        {
                            enemyAnimator.SetTrigger("Attack");
                        }
                        string name = "Meshes/tomahawk";
                        GameObject tom = Instantiate(Resources.Load(name) as GameObject, transform.position, transform.rotation);
                        tom.transform.localScale = transform.localScale;
                        StartCoroutine(specialBulletCoroutine(TomahawkTime/6, Specials.indiano, tom));
                        break;

                    default:
                        enemyBullet bullet = Instantiate(bulletPrefab, shotPoint.position, Quaternion.identity);
                        bullet.GetComponent<enemyBullet>().currentOrigin = enemyBullet.Origin.Original;
                        bullet.shooter = this;
                        bullet.transform.up = transform.forward;
                        bullet.transform.SetParent(shotPoint);
                        bullet.shotPoint = shotPoint;
                        break;
                }
                break;
        }
       
       
    }

    public void specialSound()
    {
        switch (special)
        {
            case Specials.trex:
                AudioClip track2 = Resources.Load("Audio/Enemies/Trex/DinoRoar") as AudioClip;
                source.clip = track2;
                source.pitch = 2;
                source.spatialBlend = 1;
                source.loop = false;
                source.Play();
                break;

            default:
              
                break;
        }
    }

    


    //coroutine che aspetta un pò prima di togliere la vita al target colpito, il tempo timeToHIt indica il tempo che l'animazione impega a far partire il colpo
    public IEnumerator hitTargetCoroutine(float timeToHit)
    {
        yield return new WaitForSeconds(timeToHit);
        // se il nemico è all' altezza giusta infliggo danno, se no il colpo parte ma non fa danno andando a vuoto (tipo se justin sta saltando)
        float verticalDistance = Mathf.Abs(target.transform.position.y - transform.position.y);

        if (target.GetComponent<simpleEnemy>() && verticalDistance < 1f)
        {
            target.GetComponent<simpleEnemy>().enemyLife -= 1;
            
        }
        else if (target.GetComponent<Justin>() && target.GetComponent<Justin>()._isGrounded)
        {
            globalVariables.justinDamage();
           
        }

        //do il tempo al nemico di finire l'animazione di attacco prima di tornare eventualmente in patrol
        switch (special)
        {
            case Specials.trex:
                StartCoroutine(waitPatrolCoroutine(TrexAttackTime * 2 / 3));
                break;

            default:
                StartCoroutine(waitPatrolCoroutine(0));
                break;
        }
    }

    public IEnumerator specialBulletCoroutine(float timeToShoot, Specials spec, GameObject tomahawk)
    {
        yield return new WaitForSeconds(timeToShoot);
        Destroy(tomahawk);
        switch (spec)
        {
            case Specials.indiano:

                enemyBullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1.26f + transform.up * 0.8f, transform.rotation);
                bullet.GetComponent<enemyBullet>().currentOrigin = enemyBullet.Origin.Original;
                bullet.shooter = this;
                bullet.transform.up = transform.forward;
                break;

            default:
        
                break;
        }
    }


    //coroutine che aspetta che l'animiazione di attacco finisca prima di far tornare in patrol il nemico
    public IEnumerator waitPatrolCoroutine(float timeToEndAnim)
    {
     
        yield return new WaitForSeconds(timeToEndAnim);
        readyToPatrol = true;
    }

    public IEnumerator DeathCoroutine(float timeToEndAnim)
    {

        yield return new WaitForSeconds(timeToEndAnim);
        Destroy(this.gameObject);
    }

    public IEnumerator firstTargetCoroutine()
    {

        yield return new WaitForSeconds(0.1f);
        target = justin.gameObject;
    }




}
