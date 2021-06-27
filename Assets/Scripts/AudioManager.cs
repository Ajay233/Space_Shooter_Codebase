using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _powerUpAudio;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Unable to find Audio_Manager AudioSource");
        } 
    }

    public void PlayPowerUpAudio()
    {
        _audioSource.clip = _powerUpAudio;
        _audioSource.Play();
    }
}
