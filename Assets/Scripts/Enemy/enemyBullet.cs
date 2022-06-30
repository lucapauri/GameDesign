using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public GlobalVariables globalVariables;

    private float shootForce;

    public simpleEnemy shooter;

    public Transform _target;

    Rigidbody rb;

    private float bulletStartPosY;
    private float decreaseImpulse = 0.55f;


    // Start is called before the first frame update
    void Start()
    {
        
        bulletStartPosY = transform.position.y - shooter.transform.position.y;
        _target = shooter.target.transform;
        rb = GetComponent<Rigidbody>();
        float time = Mathf.Sqrt( bulletStartPosY/ 4.9f);
        shootForce = (_target.position.x - shooter.transform.position.x) / (0.5f * time * time);
        shootForce -= decreaseImpulse * (shootForce);
        rb.AddForce(transform.up * Mathf.Abs(shootForce) * 2, ForceMode.Impulse);


        globalVariables = FindObjectOfType<GlobalVariables>();

        //todo aggiungere calcoli per muovere proiettile anche in y
    }



    void OnCollisionEnter(Collision collision)

    {

        switch (collision.gameObject.layer)
        {
            case 10: // justin

                globalVariables.justinLife -= 1;
                break;


            case 8: //nemici 
                collision.gameObject.GetComponent<simpleEnemy>().enemyLife -= 1;
                break;


            case 11: //distruttibili
                     //codice per l'interazione del proiettile con oggetti distruttibili
                break;
        }




        Destroy(gameObject);

    }


}
