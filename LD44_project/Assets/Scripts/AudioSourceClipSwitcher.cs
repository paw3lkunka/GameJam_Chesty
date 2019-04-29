using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceClipSwitcher : MonoBehaviour
{
    public List<AudioClip> clips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SwitchClip(int n)
    {
        audioSource.clip = clips[n];
    }
}
