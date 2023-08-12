using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    private Transform playerTransform;
    private bool isLowEnough;

    private float maxHeight = 15f;
    private float minHeight = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerTransform = GetComponent<Transform>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && isLowEnough == true)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }

        AltitudeCheck();  
    }

    private void AltitudeCheck()
    {
        if (playerTransform.position.y > minHeight && playerTransform.position.y < maxHeight)
        {
            isLowEnough = true;
            Debug.Log("isLowEnough");
        }
        else
        {
            isLowEnough = false;

            if (playerTransform.position.y >= maxHeight)
            {
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }
            
            Debug.Log("outOfRange");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }

    }

}
