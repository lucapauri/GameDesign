using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private GlobalVariables globalVariables;

    private GameObject Door;

    private float verticalTriggerDistance = 2.2f;
    private float horizontalTriggerDistance = 2f;
    private float horizontalDestroyDistance = 4f;

    private bool wrongFall;
    private bool rightFall;
    private simpleEnemy target;


    // Start is called before the first frame update
    void Start()
    {
        target = null;
        wrongFall= false;
        rightFall = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
        Door = GameObject.FindGameObjectWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        if (wrongFall)
        {
            respawn(target);
            wrongFall = false;

        }
        else if (rightFall)
        {
            noRespawn(target);
            rightFall = false;
        }
        else 
        {
            foreach (simpleEnemy enemy in globalVariables.enemies)
            {
                float horizontalDistance = Mathf.Abs(enemy.transform.position.x - transform.position.x);
                float verticalDistance = Mathf.Abs(enemy.transform.position.y - transform.position.y);
                bool triggerPosition = horizontalDistance <= horizontalTriggerDistance && verticalDistance <= verticalTriggerDistance;
                bool destroyPosition = horizontalDistance > horizontalTriggerDistance && horizontalDistance <= horizontalDestroyDistance && verticalDistance <= verticalTriggerDistance;

                if (destroyPosition && enemy.GetComponent<simpleEnemy>().special == simpleEnemy.Specials.robot)
                {
                    wrongFall = true;
                    target = enemy;
                }
                else if (triggerPosition && enemy.GetComponent<simpleEnemy>().special == simpleEnemy.Specials.robot)
                {
                    rightFall = true;
                    target = enemy;

                }
            }
        }
        
    }

   private void respawn(simpleEnemy enemy)
    {
        Transform wayroot = enemy.GetComponent<simpleEnemy>().wayRoot;

        GameObject respawnRobot = Instantiate(enemy.gameObject, wayroot.position, Quaternion.identity);
        respawnRobot.GetComponent<simpleEnemy>().wayRoot = wayroot;
        respawnRobot.GetComponent<simpleEnemy>().currentOrigin = simpleEnemy.Origin.Original;

        globalVariables.enemies.Remove(enemy.GetComponent<simpleEnemy>());
        globalVariables.enemies.Add(respawnRobot.GetComponent<simpleEnemy>());
        Destroy(enemy.gameObject);
        target = null;
    }

    private void noRespawn(simpleEnemy enemy)
    {
        globalVariables.enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        Destroy(Door);
        target = null;
    }
}
