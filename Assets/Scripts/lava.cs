using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava : MonoBehaviour
{
    private GlobalVariables globalVariables;
    private float verticalDestroyDistance = 2.2f;
    private float HorizontalDestroyDistance;
    private Collider collider;
    private Vector3 nemicoCityPos;
    private NemicoCity nemicoCity;


    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.FindObjectOfType<GlobalVariables>();
        collider = GetComponent<Collider>();

        HorizontalDestroyDistance = collider.bounds.extents.x;

    }

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

        nemicoCity = FindObjectOfType<NemicoCity>();
        if (nemicoCity)
        {
            nemicoCityPos = nemicoCity.transform.position;
        }

        float verticalCityDistance = Mathf.Abs(nemicoCityPos.y - transform.position.y);
        float horizontalCityDistance = Mathf.Abs(nemicoCityPos.x - transform.position.x);

        bool destroyCityPositionV = verticalCityDistance < verticalDestroyDistance;
        bool destroyCityPositionH = horizontalCityDistance < HorizontalDestroyDistance;

        if (destroyCityPositionV && destroyCityPositionH && nemicoCity!= null)
        {
            nemicoCity.enemyLife = 0;
        }



    }
}
