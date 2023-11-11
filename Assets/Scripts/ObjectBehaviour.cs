using System.Linq;
using UnityEngine;

/// <summary>
/// ObjectBehaviour is a script on each of the falling objects' prefabs which controls their behaviour in several ways.
/// </summary>
public class ObjectBehaviour : MonoBehaviour
{
    [SerializeField] float lifeTime = 15f;
    float currentLife;
    [SerializeField] int damageValue = -1;
    int scoreValue = 1;
    AudioSource source;
    Rigidbody rigidBody;
    [SerializeField] bool usesQuickFalling = false;

    private void Start()
    {
        currentLife = Time.time;
        source = GetComponent<AudioSource>();
        source.volume = 0.1f;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time >= currentLife + lifeTime) // delete object once it reaches time limit
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -1) // delete object once it goes below the ground
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (usesQuickFalling) // for daggers which fall 5x quicker than other objects
        {
            rigidBody.AddForce(5f * Physics.gravity, ForceMode.Acceleration); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SwordControl swordControl = other.GetComponent<SwordControl>();
        if (swordControl != null) // if object is hit with player swords, play sound
        {
            int swordSoundIndex = Random.Range(0, SwordControl.swordSounds.Count() - 1);
            if (!source.isPlaying)
            {
                source.clip = (AudioClip)SwordControl.swordSounds[swordSoundIndex];
                source.Play();
            }
        }
        TouchGround hitGround = other.GetComponent<TouchGround>();
        if (hitGround != null) // if object touches ground outside scorezone, increase score
        {
            hitGround.UpdateScore(scoreValue);
            hitGround.UpdateScoreDisplay();
            Destroy(gameObject);
        }
        ScoreZone hitScoreZone = other.GetComponent<ScoreZone>();
        if (hitScoreZone != null) // if object lands in scorezone, increase or reduce health
        {
            hitScoreZone.UpdateHealth(damageValue);
            hitScoreZone.UpdateHealthDisplay();
            AudioSource scoreSource = hitScoreZone.GetComponent<AudioSource>();
            if (damageValue > 0) // if object is a healing potion, play healing sound
            {
                scoreSource.clip = SwordControl.healingSound;
                scoreSource.Play();
            } else // else play damage sound
            {
                scoreSource.clip = SwordControl.damageSound; 
                scoreSource.Play();
            }
            Destroy(gameObject);
        }
    }
}
