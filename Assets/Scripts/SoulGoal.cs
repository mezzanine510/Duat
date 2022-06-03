using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGoal : MonoBehaviour
{
    [SerializeField]
    float maxSouls = 3f;

    Animator animator;
    AudioSource audioSource;
    AudioClip soulGatherClip;
    private int numberOfSouls;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        soulGatherClip = audioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        GameObject[] souls = other.GetComponent<SoulAttractor>().souls;

        if (!souls[0]) return;
        
        GameObject[] followPositions = other.GetComponent<SoulAttractor>().followPositions;
        // FollowPositionHandler followPositionHandler = other.gameObject.GetComponent<FollowPositionHandler>();
        
        foreach (GameObject soul in souls)
        {
            if (!soul) continue;

            soul.transform.parent = transform;
            soul.GetComponent<SoulController>().target = gameObject;
            Destroy(soul.gameObject, 3f);
            numberOfSouls++;

            if (numberOfSouls >= maxSouls)
            {
                animator.SetBool("soulsDelivered", true);
                Destroy(gameObject, 4f);
            }
        }

        foreach (GameObject position in followPositions)
        {
            position.GetComponent<FollowPositionHandler>().occupied = false;
        }

        audioSource.PlayOneShot(soulGatherClip);

        other.gameObject.GetComponent<SoulAttractor>().soulCounter = 0;
        Array.Clear(souls, 0, souls.Length);
    }
}
