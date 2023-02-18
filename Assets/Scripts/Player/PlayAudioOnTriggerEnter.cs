using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{

    public Sound clip;
    private AudioSystem audioSystem;
    public string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        audioSystem = FindObjectOfType<AudioSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            audioSystem.Play("Music Test Pilot");
        }
    }
}
