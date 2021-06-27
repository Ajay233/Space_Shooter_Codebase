using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _lifeSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartInstructions;
    [SerializeField]
    private Text _waveCompleteText;
    [SerializeField]
    private Text _warningText;
    [SerializeField]
    private Text _waveNumberText;
    private Transform _panel;
    private bool _showWaveNumberWarning;
    [SerializeField]
    private AudioClip _warningClip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartInstructions.gameObject.SetActive(false);
        _panel = transform.GetChild(7);
        if (_panel == null)
        {
            Debug.LogError("Unable to find the UI Panel");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Unable to find AudioSource");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdatePlayerLivesDisplay(int lives)
    {
        _livesImage.sprite = _lifeSprites[lives];
    } 

    public void DisplayGameOver()
    {
        StartCoroutine(ToggleGameOver());
    }

    IEnumerator ToggleGameOver()
    {
        _restartInstructions.gameObject.SetActive(true);
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }

    public void OpenPauseMenu()
    {
        _panel.gameObject.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        _panel.gameObject.SetActive(false);
    }

    public void ShowWaveComplete()
    {
        StartCoroutine(ShowWaveCompleteRoutine());
    }

    IEnumerator ShowWaveCompleteRoutine()
    {
        _waveCompleteText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _waveCompleteText.gameObject.SetActive(false);
    }

    public void ShowNewWaveBanner(int waveNumber)
    {
        StartCoroutine(ShowNewWaveBannerRoutine(waveNumber));
    }

    IEnumerator ShowNewWaveBannerRoutine(int waveNumber)
    {
        yield return new WaitForSeconds(3);
        _audioSource.clip = _warningClip;
        _audioSource.volume = 0.4f;
        _audioSource.Play();
        _warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _warningText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        _warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _warningText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        _audioSource.Pause();
        _audioSource.volume= 1;
        _waveNumberText.text = "Wave - " + waveNumber + " INCOMING";  // Could make this more specific later on
        _waveNumberText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _waveNumberText.gameObject.SetActive(false);
        // Stop playing warning sound effect
    }

}
