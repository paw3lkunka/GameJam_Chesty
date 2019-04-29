using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Audio Source component has an AudioClip option.  The audio
// played in this example comes from AudioClip and is called audioData.

[RequireComponent(typeof(AudioSource))]
public class PlayAudioAnimation : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> clips;
    [HideInInspector]
    public int clipNumber;
    AudioSource audioData;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        audioData = GetComponent<AudioSource>();
        audioData.clip = clips[clipNumber];
        audioData.Play(0);
    }

    void OnGUI()
    {
        
    }
}