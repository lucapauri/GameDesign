using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustinRespawn : MonoBehaviour
{
    public GameObject justin;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void respawn()
    {
        StartCoroutine(respawnCoroutine());
    }

    public IEnumerator respawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameObject go = Instantiate(justin, transform.position, transform.rotation);
        go.GetComponent<JustinManagerMenu>().enabled = true;

    }
}
