using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucoNero : MonoBehaviour
{
    public int gadgetOn;
    private Animator anim;
    private float collapseTime;
    private AnimationClip[] clips;
    private float rightDis = 3f;
    private Justin justin;
    private GlobalVariables globalVariables;
    // Start is called before the first frame update
    void Start()
    {
        globalVariables = FindObjectOfType<GlobalVariables>();
        justin = globalVariables.justin;
        gadgetOn = 0;
        anim = GetComponent<Animator>();
        clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Armature|grab":
                    collapseTime = clip.length;
                    break;

                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (justin == null && globalVariables.justin != null)
        {
            justin = globalVariables.justin;
        }

        float vertDistance = Mathf.Abs(transform.position.y - justin.transform.position.y);
        float horizDistance = Mathf.Abs(transform.position.x - justin.transform.position.x);
        bool rightPosV = vertDistance < rightDis;
        bool rightPosH = horizDistance < rightDis;

        if (rightPosH && rightPosV)
        {
            globalVariables.justinLife = 0;
        }


        if (gadgetOn == 4)
        {
            anim.SetTrigger("Collapse");
            StartCoroutine(endCoroutine(collapseTime));
        }
    }

    private IEnumerator endCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
