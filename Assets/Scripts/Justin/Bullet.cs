using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

   private GlobalVariables globalVariables;


    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
    }

    void OnCollisionEnter(Collision collision)

    {
        // codice per la collisione del proiettile con dei nemici
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {

            teleportEnemy(collision.gameObject, globalVariables.currentTimeline);

        }

        //codice per la collisione del proiettile con proiettili nemici
        if (collision.gameObject.GetComponent<enemyBullet>() || collision.gameObject.GetComponent<Teleportable>())
        {
            teleportBullet(collision.gameObject, globalVariables.currentTimeline);
        }

        Destroy(gameObject);

    }

    //funzione per il teletrasporto dei nemici
    public void teleportEnemy(GameObject enemy, int timeline)
    {
        float bias = globalVariables.upPlaneHeight - globalVariables.downPlaneHeight;
        globalVariables.enemies.Remove(enemy.GetComponent<simpleEnemy>());
        Vector3 upPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + bias, enemy.transform.position.z);
        Vector3 downPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y - bias, enemy.transform.position.z);
        Quaternion newRot = enemy.transform.rotation;
        Destroy(enemy);

        if (timeline > 0)
        {
            GameObject go = Instantiate(enemy, downPos, newRot);
            simpleEnemy script = go.GetComponent<simpleEnemy>();
            Animator enemyAnimator = go.GetComponent<Animator>();
            script.enabled = true;
            enemyAnimator.enabled = true;
            script.currentOrigin = simpleEnemy.Origin.TeleportedDown;

        }
        else
        {

            GameObject go = Instantiate(enemy, upPos, newRot);
            simpleEnemy script = go.GetComponent<simpleEnemy>();
            Animator enemyAnimator = go.GetComponent<Animator>();
            script.enabled = true;
            enemyAnimator.enabled = true;
            script.currentOrigin = simpleEnemy.Origin.TeleportedUp;
        }

        //aggiungi un'interfaccia generica per poi istanziare quella anzichè il gameobject, in modo da abilitare gli script come fai quando sposti justin
    }

    //funzione per il teletrasporto dei proiettili
    public void teleportBullet(GameObject bullet, int timeline)
    {
        //aggiungere codice per far si che gli oggetti vengano teletrasportati all'altezza a cui vengono colpiti
        Vector3 upPos = new Vector3(bullet.transform.position.x, globalVariables.upPlaneHeight + 4f, bullet.transform.position.z);
        Vector3 downPos = new Vector3(bullet.transform.position.x, globalVariables.downPlaneHeight + 4f, bullet.transform.position.z);
        Quaternion newRot = bullet.transform.rotation;
        Destroy(bullet);


        if (timeline > 0)
        {

            GameObject go = Instantiate(bullet, downPos, newRot);
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

            GameObject go = Instantiate(bullet, upPos, newRot);
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
