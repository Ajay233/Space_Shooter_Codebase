using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip _affirmativeClip;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Unable to find AudioSource");
        }
    }

    public void LaunchGame()
    {
        StartCoroutine(LaunchGameRoutine());   
    }

    IEnumerator LaunchGameRoutine()
    {
        _audioSource.clip = _affirmativeClip;
        _audioSource.Play();
        yield return new WaitForSeconds(0.32f);
        SceneManager.LoadScene(1);
    }
}
