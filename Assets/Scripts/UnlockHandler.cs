using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject[] unlockTargets;

    [SerializeField]
    public float delayBeforeReturningSouls = 1f;

    [SerializeField]
    float delayBeforeAnimation = 1f;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Animation lowerWallBarrier;

    private int numberOfTargets;
    public bool isOpen = false;

    void Start()
    {
        numberOfTargets = unlockTargets.Length;
    }

    public IEnumerator PlayOpenAnimation()
    {
        yield return new WaitForSeconds(delayBeforeAnimation);
        animator.Play("LowerWallBarrier");
        yield return null;
    }
}
