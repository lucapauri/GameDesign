using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class JustinManagerMenu : MonoBehaviour
{

    public Transform wayroot;
    private Sequence walkingSequence;

    public Animator anim;

    public GameObject valigettaPrefab;
    public GameObject fulminePrefab;
    public GameObject pistolaPrefab;
    public GameObject logoPanel;


    private float movingTime;
    private AnimationClip[] clips;
    private float throwTime;
    private float fireTime;



    // Start is called before the first frame update
    void Start()
    {
        searchPath();
        anim = GetComponent<Animator>();
        anim.SetBool("Walk", true);

        clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {

                case "Armature|throw":
                    throwTime = clip.length;
                    break;

                case "Armature|sparo":
                    fireTime = clip.length;
                    break;

                default:
                    break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void searchPath()
    {

        if (wayroot != null && wayroot.childCount > 0)
        {
            Vector3[] pathPositions = new Vector3[wayroot.childCount];
            for (int i = 0; i < wayroot.childCount; i++)
            {
                pathPositions[i] = wayroot.GetChild(i).position;
                pathPositions[i].y = transform.position.y;
            }
            walkingSequence = DOTween.Sequence();

            walkingSequence.Append(transform.DOPath(pathPositions, 3, PathType.CatmullRom, PathMode.Full3D, resolution: 10).SetEase(Ease.Linear).SetLookAt(0.01f)
                .SetId("walking").OnComplete(
                () =>
                {
                    anim.SetBool("Walk", false);
                    logoPanel.GetComponent<Animator>().SetTrigger("ScaleUp");
                }

                ));

        }
    }

    public void throwInst()
        {
        GameObject valigetta = Instantiate(valigettaPrefab, transform.position, transform.rotation);
        valigetta.transform.localScale = transform.localScale;

        GameObject fulmine = Instantiate(fulminePrefab, transform.position, transform.rotation);
        fulmine.transform.localScale = transform.localScale;

        valigetta.transform.SetParent(transform);
        fulmine.transform.SetParent(transform);

        StartCoroutine(throwEndCoroutine(throwTime/4));
     
        }

    private IEnumerator throwEndCoroutine(float timeToEndAnim)
    {
        yield return new WaitForSeconds(timeToEndAnim);
        if (transform.Find("fulmine_unity(Clone)") != null && transform.Find("valigetta_unity(Clone)") != null)
        {
            Destroy(transform.Find("fulmine_unity(Clone)").gameObject);
            Destroy(transform.Find("valigetta_unity(Clone)").gameObject);
            Destroy(gameObject);
        }
    }

    public void fireInst()
    {
        GameObject pistola = Instantiate(pistolaPrefab, transform.position, transform.rotation);
        pistola.transform.localScale = transform.localScale;
        pistola.transform.SetParent(transform);

        StartCoroutine(fireEndCoroutine(fireTime/2));

    }

    private IEnumerator fireEndCoroutine(float timeToEndAnim)
    {
        yield return new WaitForSeconds(timeToEndAnim);
        Destroy(transform.Find("pistola_unity(Clone)").gameObject);
    }

}
