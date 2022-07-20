using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

   private GlobalVariables globalVariables;
    private bool firstColl;
    private AudioSource source;


    // Start is called before the first frame update
    void Start()
    {
        firstColl = true;
        globalVariables = FindObjectOfType<GlobalVariables>();
        source = GetComponent<AudioSource>();
        AudioClip track = Resources.Load("Audio/Justin/shot") as AudioClip;
        source.clip = track;
        source.pitch = 1;
        source.Play();
        Debug.Log("audioShotOn");
    }

    void OnCollisionEnter(Collision collision)

    {
        Destroy(gameObject);


        // codice per la collisione del proiettile con dei nemici
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8 && firstColl)
        {

            teleportEnemy(collision.gameObject, globalVariables.currentTimeline);

        }

        //codice per la collisione del proiettile con proiettili nemici
        if (collision.gameObject.GetComponent<enemyBullet>() || collision.gameObject.GetComponent<Teleportable>())
        {
            teleportBullet(collision.gameObject, globalVariables.currentTimeline);
        }
        firstColl = false;
    }

    //funzione per il teletrasporto dei nemici
    public void teleportEnemy(GameObject enemy, int timeline)
    {
        float bias = globalVariables.upPlaneHeight - globalVariables.downPlaneHeight;
        Vector3 upPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + bias, enemy.transform.position.z);
        Vector3 downPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y - bias, enemy.transform.position.z);
        Quaternion newRot = enemy.transform.rotation;
        Vector3 oldPlat = Vector3.zero;
        if (enemy.GetComponent<NemicoCity>())
        {
            oldPlat = enemy.GetComponent<NemicoCity>().platform.position;
        }

        globalVariables.enemies.Remove(enemy.GetComponent<simpleEnemy>());

        if (timeline > 0)
        {
            GameObject go = Instantiate(enemy, downPos, newRot);

            if (go.GetComponent<simpleEnemy>())
            {
                simpleEnemy script = go.GetComponent<simpleEnemy>();
                script.enabled = true;
                script.currentOrigin = simpleEnemy.Origin.TeleportedDown;
            }
            else if (go.GetComponent<NemicoCity>())
            {

                NemicoCity script = go.GetComponent<NemicoCity>();
                script.enabled = true;
                script.currentOrigin = NemicoCity.Origin.TeleportedDown;
                script.oldPlatform = oldPlat;
            }

            Animator enemyAnimator = go.GetComponent<Animator>();
            if (enemyAnimator)
            {
                enemyAnimator.enabled = true;
            }

        }
        else
        {

            GameObject go = Instantiate(enemy, upPos, newRot);
            if (go.GetComponent<simpleEnemy>())
            {
                simpleEnemy script = go.GetComponent<simpleEnemy>();
                script.enabled = true;
                script.currentOrigin = simpleEnemy.Origin.TeleportedUp;
            }
            else if (go.GetComponent<NemicoCity>())
            {

                NemicoCity script = go.GetComponent<NemicoCity>();
                script.enabled = true;
                script.currentOrigin = NemicoCity.Origin.TeleportedUp;
                script.oldPlatform = oldPlat;
            }


            Animator enemyAnimator = go.GetComponent<Animator>();
           
            if (enemyAnimator)
            {
                enemyAnimator.enabled = true;
            }
            
        }
        Destroy(enemy);

        //aggiungi un'interfaccia generica per poi istanziare quella anzichè il gameobject, in modo da abilitare gli script come fai quando sposti justin
    }

    //funzione per il teletrasporto dei proiettili
    public void teleportBullet(GameObject bullet, int timeline)
    {
        Quaternion newRot = bullet.transform.rotation;
        Destroy(bullet);


        if (timeline > 0)
        {
            float verticalShift = bullet.transform.position.y - globalVariables.upPlaneHeight;
            Vector3 downPos = new Vector3(bullet.transform.position.x, globalVariables.downPlaneHeight + verticalShift, bullet.transform.position.z);


            GameObject go = Instantiate(bullet, downPos, newRot);
            go.GetComponent<enemyBullet>().currentOrigin = enemyBullet.Origin.Teleported;
            if (go.GetComponent<enemyBullet>())
            {
                enemyBullet script = go.GetComponent<enemyBullet>();
                script.enabled = true;
            }
            else
            {
                Teleportable script = go.GetComponent<Teleportable>();
                script.enabled = true;
            }
           

        }
        else
        {
            float verticalShift = bullet.transform.position.y - globalVariables.downPlaneHeight;
            Vector3 upPos = new Vector3(bullet.transform.position.x, globalVariables.upPlaneHeight + verticalShift, bullet.transform.position.z);

            GameObject go = Instantiate(bullet, upPos, newRot);
            go.GetComponent<enemyBullet>().currentOrigin = enemyBullet.Origin.Teleported;
            if (go.GetComponent<enemyBullet>())
            {
                enemyBullet script = go.GetComponent<enemyBullet>();
                script.enabled = true;
            }
            else
            {
                Teleportable script = go.GetComponent<Teleportable>();
                script.enabled = true;
            }

        }
    }
}
