using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava : MonoBehaviour
{
    private GlobalVariables globalVariables;
    private float verticalDestroyDistance = 2.2f;
    private float HorizontalDestroyDistance;
    private Collider collider;


    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.FindObjectOfType<GlobalVariables>();
        collider = GetComponent<Collider>();

        HorizontalDestroyDistance = collider.bounds.extents.x;
        
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            globalVariables.justinLife = 0;

        }
        else if (collision.gameObject.layer != 6)
        {
            Destroy(collision.gameObject);
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            float verticalEnemyDistance = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            float horizontalEnemyDistance = Mathf.Abs(enemy.transform.position.x - transform.position.x);


            bool destroyPositionVertical = verticalEnemyDistance < verticalDestroyDistance;
            bool destroyPositionHorizontal = horizontalEnemyDistance < HorizontalDestroyDistance;

            if (destroyPositionVertical && destroyPositionHorizontal)
            {
                globalVariables.enemies.Remove(enemy.GetComponent<simpleEnemy>());
              
                Destroy(enemy.gameObject);
            }

        }

        float verticalJustinDistance = Mathf.Abs(globalVariables.justin.transform.position.y - transform.position.y);
        float horizontalJustinDistance = Mathf.Abs(globalVariables.justin.transform.position.x - transform.position.x);

        bool destroyJustinPositionV = verticalJustinDistance < verticalDestroyDistance;
        bool destroyJustinPositionH = horizontalJustinDistance < HorizontalDestroyDistance;

        if (destroyJustinPositionV && destroyJustinPositionH)
        {
            Destroy(globalVariables.justin.gameObject);
        }



    }
}
