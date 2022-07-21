using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenaInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(changeSceneCoroutine());
    }

    private IEnumerator changeSceneCoroutine()
    {
        yield return new WaitForSeconds(10f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hub");
    }


}
