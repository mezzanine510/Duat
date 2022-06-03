using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SoulAttractor : MonoBehaviour
{
    [SerializeField]
    public GameObject followPositionsParent;

    [SerializeField]
    public GameObject[] followPositions;

    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;
    public GameObject[] souls;
    public SoulColor[] soulColors;
    SphereCollider sphereCollider;
    public int soulCounter = 0;


    void Awake()
    {
        souls = new GameObject[3];
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
        soulColors = new SoulColor[3];
    }

    private void HandleSoulCollision(Collider other)
    {
        if (soulCounter == 3) return;

        GameObject soul = other.gameObject;
        SoulController soulController = soul.GetComponent<SoulController>();

        if (!soulController) return;

        if (soulController.target) return;

        foreach (GameObject targetPosition in followPositions)
        {
            FollowPositionHandler followHandler = targetPosition.GetComponent<FollowPositionHandler>();

            if (followHandler.occupied == true)
            {
                continue;
            }

            followHandler.occupied = true;
            soulController.target = targetPosition;
            soulController.followPosition = targetPosition;
            souls[soulCounter] = soul;
            soulCounter++;
            soulColors[soulCounter - 1] = soulController.soulColor;
            audioSource.PlayOneShot(audioClips[0]);
            soulController.BrightenLight();

            break;
        }
    }

    private void HandleUnlockCollision(Collider other)
    {
        UnlockHandler unlockHandler = other.GetComponent<UnlockHandler>();

        if (!unlockHandler) return;

        if (unlockHandler.isOpen) return;

        if (soulCounter < 3) return; // expand this later

        GameObject[] unlockTargets = unlockHandler.unlockTargets;

        int targetCounter = 0;

        foreach (GameObject soul in souls)
        {
            GameObject target = unlockHandler.unlockTargets[targetCounter];
            // soul.transform.parent = target.transform;
            SoulController soulController = soul.GetComponent<SoulController>();
            soulController.speed = 1.5f;
            soulController.target = target;
            soulController.SetUnlockingAnimation();
            targetCounter++;
        }

        unlockHandler.isOpen = true;
        audioSource.PlayOneShot(audioClips[1]);
        StartCoroutine(unlockHandler.PlayOpenAnimation());
        StartCoroutine(ReturnSouls(unlockHandler.delayBeforeReturningSouls));
    }

    private void HandleUnlockColorCollision(Collider other)
    {
        UnlockHandlerColor unlockHandlerColor = other.GetComponent<UnlockHandlerColor>();

        if (!unlockHandlerColor) return;

        if (unlockHandlerColor.isOpen) return;

        if (soulCounter < 3) return; // expand this later

        bool validColors = CheckColors(unlockHandlerColor);

        if (validColors)
        {
            Debug.Log("<color=green>COLORS VALID</color>");
        }
        else
        {
            Debug.Log("<color=red>COLORS NOT VALID</color>");
            ReturnSoulsToSpawn();

            foreach (GameObject position in followPositions)
            {
                position.GetComponent<FollowPositionHandler>().occupied = false;
            }

            return;
        }

        GameObject[] unlockTargets = unlockHandlerColor.unlockTargets;

        foreach (GameObject soul in souls)
        {
            SoulController soulController = soul.GetComponent<SoulController>();

            foreach (GameObject target in unlockTargets)
            {
                Debug.Log("<color=yellow>TARGET: " + target + "</color>");
                SoulColor targetColor = target.GetComponent<BarrierKeyhole>().color;

                if (soulController.soulColor != targetColor)
                {
                    continue;
                }

                soulController.speed = 1.5f;
                soulController.target = target;
                soulController.SetUnlockingAnimation();
            }
        }

        unlockHandlerColor.isOpen = true;
        audioSource.PlayOneShot(audioClips[1]);
        StartCoroutine(unlockHandlerColor.PlayOpenAnimation());
        StartCoroutine(ReturnSouls(unlockHandlerColor.delayBeforeReturningSouls));
    }

    private bool CheckColors(UnlockHandlerColor unlockHandlerColor)
    {
        SoulColor[] unlockColors = unlockHandlerColor.unlockColors;

        int colorCounter = 0;

        foreach (SoulColor color in unlockColors)
        {
            if (soulColors.Contains(color)) colorCounter++;
        }

        if (colorCounter == unlockColors.Length) return true;
        else return false;
    }

    private void ReturnSoulsToSpawn()
    {
        foreach (GameObject soul in souls)
        {
            SoulController soulController = soul.GetComponent<SoulController>();
            soulController.ReturnToSpawn();
        }

        Array.Clear(souls, 0, souls.Length);
        soulCounter = 0;
        audioSource.PlayOneShot(audioClips[0]);
    }

    private IEnumerator ReturnSouls(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject soul in souls)
        {
            SoulController soulController = soul.GetComponent<SoulController>();
            soulController.ResetFollowSpeed();
            soulController.ResetFollowTarget();
            soulController.SetFloatingAnimation();
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=purple>Collider: " + other.gameObject.name + "</color>");
        if (other.tag == "SoulSphere") HandleSoulCollision(other);
        if (other.tag == "UnlockTarget") HandleUnlockCollision(other);
        if (other.tag == "UnlockColorTarget") HandleUnlockColorCollision(other);
    }
}
