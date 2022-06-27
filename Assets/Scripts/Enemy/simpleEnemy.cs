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
    public bool standing;


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
    private bool dead;


    public enum MachineStatus //stati della macchina agli stati finiti
    {
        Patrol,
        Attack,
        Special
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
        trexFriend,
        babyPtero
    }

    //specie del nemico
    public enum spieces
    {

    }

    public MachineStatus currentStatus;
    public Origin currentOrigin;
    public Type enemyType;
    public Specials special;




    // Start is called before the first frame update
    void Start()
    {

        globalVariables = FindObjectOfType<GlobalVariables>();
        
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
                    case "TrexAttack":
                        TrexAttackTime = clip.length;
                        break;

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

        finiteStateMachine = new FiniteStateMachine<simpleEnemy>(this);

        //STATES
        State patrolState = new PatrolState("patrol", this);
        State attackState = new AttackState("attack", this);
        State specialState = new SpecialState("special", this);


        //TRANSITIONS
        finiteStateMachine.AddTransition(patrolState, attackState, () => currentStatus == MachineStatus.Attack);
        finiteStateMachine.AddTransition(attackState, patrolState, () => currentStatus == MachineStatus.Patrol);
        finiteStateMachine.AddTransition(specialState, patrolState, () => currentStatus == MachineStatus.Patrol);

        //scelgo lo stato iniziale
        if (special == Specials.none && standing==false)
        {
            finiteStateMachine.SetState(patrolState);
        }
        else
        {
            if (standing == false)
            {
                finiteStateMachine.SetState(specialState);
                currentStatus = MachineStatus.Special;
            }
            else
            {
                finiteStateMachine.SetState(attackState);
                currentStatus = MachineStatus.Attack;
            }
        }

        globalVariables.enemies.Add(this);
        target = globalVariables.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
       
            finiteStateMachine.Tik();

        if (enemyLife == 0 && dead==false)
        {
            dead = true;
            globalVariables.enemies.Remove(this);
            enemyAnimator.SetTrigger("Death");
            Debug.Log(DeathTime);
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

                //attivo animazione di attacco 
                enemyAnimator.SetTrigger("Attack");
                //controllo se il target è un intruso oppure justin
                if(target.GetComponent<simpleEnemy>()!=null)
                {
                    //controllo se il nemico è speciale, se lo è cerco di far si che la vita venga tolta al target nell'istante in cui l'animazione di attacco effettivamento colpisce il bersaglio
                    switch (special)
                    {
                        case Specials.trexFriend:
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
                        case Specials.trexFriend:
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

                enemyBullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 3f + transform.up * 2f, Quaternion.identity);
                bullet.shooter = this;
                bullet.transform.up = transform.forward;
                break;
        }
       
       
    }

    //funzione uica per far fare interazioni speciali ai  nemici speciali
    public void specialInteraction(GameObject obj)
    {
        //cerco il tipo di nemico speciale
        switch (special)
        {
            case simpleEnemy.Specials.trexFriend:
                Destroy(obj);
                break;
           
        }


    }


    //coroutine che aspetta un pò prima di togliere la vita al target colpito, il tempo timeToHIt indica il tempo che l'animazione impega a far partire il colpo
    public IEnumerator hitTargetCoroutine(float timeToHit)
    {
        yield return new WaitForSeconds(timeToHit);
        if (target.GetComponent<simpleEnemy>())
        {
            target.GetComponent<simpleEnemy>().enemyLife -= 1;
            //se il nemico è stato ucciso do il tempo al nemico di finire l'animazione di attacco per poi farlo tornare in patrol
            if (target.GetComponent<simpleEnemy>().enemyLife == 0)
            {
                switch (special)
                {
                    case Specials.trexFriend:
                        StartCoroutine(enemyKilledCoroutine(TrexAttackTime * 2/3 ));
                        break;

                    default:
                        StartCoroutine(enemyKilledCoroutine(0));
                        break;
                }
            }
        }
        else if (target.GetComponent<Justin>())
        {
            globalVariables.justinLife -= 1;
           
        }
    }


    //coroutine che aspetta che l'animiazione di attacco finisca prima di far tornare in patrol il nemico
    public IEnumerator enemyKilledCoroutine(float timeToEndAnim)
    {
     
        yield return new WaitForSeconds(timeToEndAnim);
        readyToPatrol = true;
    }

    public IEnumerator DeathCoroutine(float timeToEndAnim)
    {

        yield return new WaitForSeconds(timeToEndAnim);
        Destroy(this.gameObject);
    }




}
