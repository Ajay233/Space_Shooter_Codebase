using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;
    private Player _player;
    private Animator _animator;
    private BoxCollider2D _body;

    [SerializeField]
    private AudioClip _explosionAudio;
    [SerializeField]
    private AudioClip _enemyLaserAudio;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _minDelayInLaserFire = 3f;
    private float _maxDelayInLaserFire = 7f;

    private Transform _thruster1;
    private Transform _thruster2;
    private bool _enemyDestroyed;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Unable to find player");
        }

        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Unable to get animator");
        }

        _body = gameObject.GetComponent<BoxCollider2D>();
        if (_body == null)
        {
            Debug.LogError("Unable to get Rigidbody2D");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Unable to find Enemy AudioSource");
        }

        _thruster1 = transform.GetChild(0);
        if (_thruster1 == null)
        {
            Debug.LogError("Unable to get thruster 1");
        }

        _thruster2 = transform.GetChild(1);
        if (_thruster2 == null)
        {
            Debug.LogError("Unable to get thruster 2");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Unable to find Game_manager");
        }
        _enemyDestroyed = false;
        setStartPosition();
        StartCoroutine(FireLaserRoutine(_minDelayInLaserFire, _maxDelayInLaserFire));
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.IncreaseScore(10);
                _gameManager.DecreaseWaveSize();
            }
            else
            {
                Debug.Log("Player not found yet");
            }
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.clip = _explosionAudio;
            _audioSource.Play();
            DestroyEnemy();
        }

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.DecreaseLifeBar();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.clip = _explosionAudio;
            _audioSource.Play();
            DestroyEnemy();
        }
    }

    void setStartPosition()
    {
        transform.position = new Vector3(Random.Range(-8.8f, 8.8f), 8, 0);
    }

    void moveEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            transform.position = new Vector3(Random.Range(-8.8f, 8.8f), 8, 0);
            // Destroy(this.gameObject);
        }
    }

    void DestroyEnemy()
    {
        _speed = 2;
        _enemyDestroyed = true;
        Destroy(_body);
        Destroy(_thruster1.gameObject, 0.5f);
        Destroy(_thruster2.gameObject, 0.5f);
        Destroy(this.gameObject, 2.633f);
    }

    public void SetEnemySpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    IEnumerator FireLaserRoutine(float min, float max)
    {
        while (_enemyDestroyed == false)
        {
            if (transform.position.y > -2)
            {
                Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
                _audioSource.clip = _enemyLaserAudio;
                _audioSource.Play();
            }
            yield return new WaitForSeconds(Random.Range(min, max));
        }
    }

}
