using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    public AudioClip SuccessSound;
    public AudioClip FailureSound;

    public AudioClip FishEscapeSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySuccessSound()
    {
        audioSource.PlayOneShot(SuccessSound);
    }

    public void PlayFailureSound()
    {
        audioSource.PlayOneShot(FailureSound);
    }

    public void PlayFishEscapeSound()
    {
        audioSource.PlayOneShot(FishEscapeSound);
    }
}
