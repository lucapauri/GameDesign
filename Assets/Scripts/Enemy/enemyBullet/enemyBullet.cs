using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public GlobalVariables globalVariables;

    private float shootForce;

    public simpleEnemy shooter;
    public Transform shotPoint;

    public Transform _target;

    public GameObject explosion;

    Rigidbody rb;

    private Vector3 toTarget;
    private Vector3 targetDirection;

    private float bulletStartPosY;
    private float decreaseImpulse = 0.9f;
    private float movSpeedForward;
    private float movSpeedDown;
    private bool readyToFire;
    public AudioSource source;
    private float lifeTime;

    public enum Origin
    {
        Original,
        Teleported
    }

    public Origin currentOrigin;

    // Start is called before the first frame update
    void Start()
    {
        
        bulletStartPosY = transform.position.y - shooter.transform.position.y;
        _target = shooter.target.transform;
        rb = GetComponent<Rigidbody>();
        movSpeedForward = 4f;
        movSpeedDown = 0.1f;
        globalVariables = FindObjectOfType<GlobalVariables>();
        readyToFire = false;
        if (shooter.special != simpleEnemy.Specials.robot)
        {
            lifeTime = 10f;
        }
        else
        {
            lifeTime = 2f;
        }

        StartCoroutine(lifetimeOutCoroutine(lifeTime));
        StartCoroutine(readyToFireCoroutine());

        toTarget = _target.transform.position + globalVariables.justin.transform.up * 1.5f - transform.position;

        if (currentOrigin == Origin.Original)
        {
            targetDirection = toTarget.normalized;
        }
        else
        {
            targetDirection = transform.up;
        }
        

    }

    private void FixedUpdate()
    {
        if (readyToFire && shooter.special!= simpleEnemy.Specials.robot)
        {
            Vector3 newDir = targetDirection * movSpeedForward - transform.right * movSpeedDown;
            rb.MovePosition(transform.position + newDir * Time.deltaTime);
        }
    }



    void OnCollisionEnter(Collision collision)

    {
        if (shooter.special != simpleEnemy.Specials.robot)
        {
            switch (collision.gameObject.layer)
            {
                case 10: // justin

                    globalVariables.justinDamage();

                    break;


                case 8: //nemici 
                    collision.gameObject.GetComponent<simpleEnemy>().enemyLife -= 1;
                    break;


                case 11: //distruttibili
                    Destroy(collision.gameObject);
                    break;
            }

            Vector3 explosionPoint = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            if (explosion != null)
            {
                Instantiate(explosion, explosionPoint, Quaternion.identity);
            }
        }

        Destroy(gameObject);

    }

    private IEnumerator lifetimeOutCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (explosion != null)
        {
            Vector3 explosionPoint = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            Instantiate(explosion, explosionPoint, Quaternion.identity);
        }

        if (shooter.special == simpleEnemy.Specials.robot)
        {
            globalVariables.justinDamage();
        }
        Destroy(gameObject);

    }

    private IEnumerator readyToFireCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (shooter.special != simpleEnemy.Specials.robot)
        {
            shotPoint.DetachChildren();
            readyToFire = true;
            source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
            AudioClip track = Resources.Load("Audio/Oggetti/EnemyShoot") as AudioClip;
            source.clip = track;
            source.Play();
        }
    }


}
