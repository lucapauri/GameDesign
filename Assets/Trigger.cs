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


    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        Door = GameObject.FindGameObjectWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            float horizontalDistance = Mathf.Abs(enemy.transform.position.x - transform.position.x);
            float verticalDistance = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            bool triggerPosition = horizontalDistance <= horizontalTriggerDistance && verticalDistance <= verticalTriggerDistance;
            bool destroyPosition = horizontalDistance > horizontalTriggerDistance && horizontalDistance <= horizontalDestroyDistance && verticalDistance <= verticalTriggerDistance;

            if (destroyPosition && enemy.GetComponent<simpleEnemy>().special == simpleEnemy.Specials.robot)
            {
                Transform wayroot = enemy.GetComponent<simpleEnemy>().wayRoot;

                GameObject respawnRobot = Instantiate(enemy.gameObject, wayroot.position, Quaternion.identity);
                respawnRobot.GetComponent<simpleEnemy>().wayRoot = wayroot;

                globalVariables.enemies.Remove(enemy.GetComponent<simpleEnemy>());
                globalVariables.enemies.Add(respawnRobot.GetComponent<simpleEnemy>());
                Destroy(enemy);
            }
            else if (triggerPosition && enemy.GetComponent<simpleEnemy>().special == simpleEnemy.Specials.robot)
            {
                Destroy(Door);
            }
        }
        
    }
}
