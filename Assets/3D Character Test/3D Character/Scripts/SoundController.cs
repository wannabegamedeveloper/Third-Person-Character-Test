using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
