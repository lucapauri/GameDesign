using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatRespawn : MonoBehaviour
{
    private float respawnTime = 6f;
    public GameObject plat; //da aggiungere un prefab per le piattaforme nel deserto

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void respawnPlat(Vector3 platPos, Quaternion platRot, Vector3 platScale)
    {
        StartCoroutine(respawnPlatCoroutine(platPos, platRot, platScale));
    }

    public IEnumerator respawnPlatCoroutine(Vector3 platPos, Quaternion platRot, Vector3 platScale)
    {
        yield return new WaitForSeconds(respawnTime);
        GameObject go = Instantiate(plat, platPos, platRot);
        go.transform.localScale = platScale;

    }



}
