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

    private Animator anim;
    private simpleEnemy Robot;

    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        Door = GameObject.FindGameObjectWithTag("Door");
        anim = GetComponent<Animator>();

        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            if (enemy.GetComponent<simpleEnemy>().special == simpleEnemy.Specials.robot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Door);
           if (Robot != null)
        {

            float horizontalDistance = Mathf.Abs(Robot.transform.position.x - transform.position.x);
            float verticalDistance = Mathf.Abs(Robot.transform.position.y - transform.position.y);
            bool triggerPosition = horizontalDistance <= horizontalTriggerDistance && verticalDistance <= verticalTriggerDistance;
            bool destroyPosition = horizontalDistance > horizontalTriggerDistance && horizontalDistance <= horizontalDestroyDistance && verticalDistance <= verticalTriggerDistance;

            if (destroyPosition)
            {
                respawn(Robot);
            }
            else if (triggerPosition)
            {
                noRespawn(Robot);
                anim.SetTrigger("Down");
                Debug.Log("Down");

            }
        }
        else
        {
            StartCoroutine(searchRobotCoroutine());
        }
                   
                   
        
    }

   private void respawn(simpleEnemy enemy)
    {
        Transform wayroot = enemy.GetComponent<simpleEnemy>().wayRoot;

        GameObject respawnRobot = Instantiate(enemy.gameObject, wayroot.position, Quaternion.identity);
        respawnRobot.GetComponent<simpleEnemy>().enabled = true;
        respawnRobot.GetComponent<simpleEnemy>().wayRoot = wayroot;
        respawnRobot.GetComponent<simpleEnemy>().currentOrigin = simpleEnemy.Origin.Original;

        globalVariables.enemies.Remove(enemy.GetComponent<simpleEnemy>());
        globalVariables.enemies.Add(respawnRobot.GetComponent<simpleEnemy>());
        Destroy(enemy.gameObject);
    }

    private void noRespawn(simpleEnemy enemy)
    {
        globalVariables.enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        Destroy(Door);
    }

    private IEnumerator searchRobotCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            if (enemy.GetComponent<simpleEnemy>().special == simpleEnemy.Specials.robot)
            {
                Robot = enemy;
            }

        }
    }
}
