using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[SerializeField]
    //private float _speed = 6;
    private Player _player;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;

    //private Animator _asteroidAnimator;
    private CircleCollider2D _asteroidCollider;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Unable to find player");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Unable to find spawn manager");
        }

        //_asteroidAnimator = gameObject.GetComponent<Animator>();
        //if (_asteroidAnimator == null)
        //{
        //    Debug.LogError("Unable to find asteroid animator");
        //}

        _asteroidCollider = gameObject.GetComponent<CircleCollider2D>();
        if (_asteroidCollider == null)
        {
            Debug.LogError("Unable to find asteroid circle collider 2D");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Unable to find Game_Manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
        MoveAsteroid();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            //_asteroidAnimator.SetTrigger("OnAsteroidDestroyed");
            // Destroy(_asteroidCollider);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(_asteroidCollider);
            _player.IncreaseScore(5);
            _gameManager.DecreaseWaveSize();
            Destroy(this.gameObject, 0.2f);
        }

        if (other.tag == "Player")
        {
            _player.DecreaseLifeBar();
        }
    }

    void MoveAsteroid()
    {
        transform.Translate(Vector3.down * 2 * Time.deltaTime);
        if(transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-8.8f, 8.8f), 9, 0);
        }
    }

    // TODO:
    // Asteroid movement - moves down like enemies, wraps if not destroyed
    // Asteroid destroyed - decrease the waveSize, increment player points by 5
}
