using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockHandlerColor : MonoBehaviour
{
    [SerializeField]
    public GameObject[] unlockTargets;

    [SerializeField]
    public float delayBeforeReturningSouls = 1f;

    [SerializeField]
    float delayBeforeAnimation = 1f;

    [SerializeField]
    Animator animator;

    public SoulColor[] unlockColors;
    private int numberOfTargets;
    public bool isOpen = false;

    void Start()
    {
        numberOfTargets = unlockTargets.Length;
        unlockColors = new SoulColor[numberOfTargets];
        int colorCounter = 0;

        foreach (GameObject target in unlockTargets)
        {
            unlockColors[colorCounter] = target.GetComponent<BarrierKeyhole>().color;
            Debug.Log("<color=green>" + unlockColors[colorCounter] + "</color>");
            colorCounter++;
        }
    }

    public IEnumerator PlayOpenAnimation()
    {
        yield return new WaitForSeconds(delayBeforeAnimation);
        animator.Play("LowerBarrier");
        yield return null;
    }
}
