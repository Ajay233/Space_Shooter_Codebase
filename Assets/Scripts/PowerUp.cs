using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _powerUpID;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Unable to find Audio_manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        movePowerup();
    }

    void movePowerup()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0: player.TripleShotActive(); break;
                    case 1: player.SpeedBoostActive(); break;
                    default: player.ShieldActive(); break;
                }
            }

            // Could have also used AudioSource.PlayClipAtPoint
            _audioManager.PlayPowerUpAudio();
            Destroy(this.gameObject);
        }
    }
}
