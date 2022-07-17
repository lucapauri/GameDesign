using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public enum type
    {
        bullet,
        dust
    }

    public type tipology;
    // Start is called before the first frame update
    void Start()
    {
      switch (tipology)
        {
            case type.bullet:
                StartCoroutine(bulletCoroutine());
                break;
            case type.dust:
                StartCoroutine(dustCoroutine());
                break;
        }
    }



    private IEnumerator bulletCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }


    private IEnumerator dustCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
