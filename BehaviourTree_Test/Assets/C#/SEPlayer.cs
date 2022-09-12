using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    public AudioClip SuccessSound;
    public AudioClip FailureSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySuccessSound()
    {
        audioSource.clip = SuccessSound;
        audioSource.Play();
    }

    public void PlayFailureSound()
    {
        audioSource.clip = FailureSound;
        audioSource.Play();
    }
}
