using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(endCoroutine());
    }

    private IEnumerator endCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
