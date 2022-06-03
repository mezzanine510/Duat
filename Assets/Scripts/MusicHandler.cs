using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] float volumeChangeSpeed = 1f;
    AudioSource audioSource;
    AudioClip musicClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicClip = GetComponent<AudioSource>().clip;
        // StartCoroutine(LoopMusic());
        StartCoroutine(IncreaseVolume());
    }

    private IEnumerator IncreaseVolume()
    {
        while (audioSource.volume < 1)
        {
            audioSource.volume += volumeChangeSpeed * Time.deltaTime;
            yield return null;
        }

        Debug.Log("VOLUME INCREASE COMPLETE");
    }

    // private IEnumerator LoopMusic()
    // {
    //     while (true)
    //     {
    //         audioSource.PlayOneShot(musicClip);
    //         yield return new WaitForSeconds(39.5f);
    //     }
    // }
}
