using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 15f;
    float currentLife;
    [SerializeField]
    int damageValue = -1;
    int scoreValue = 1;
    AudioSource source;

    private void Start()
    {
        currentLife = Time.time;
        source = GetComponent<AudioSource>();
        source.volume = 0.1f;
    }

    private void Update()
    {
        if (Time.time >= currentLife + lifeTime)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SwordControl swordControl = other.GetComponent<SwordControl>();
        if (swordControl != null)
        {
            int swordSoundIndex = Random.Range(0, SwordControl.swordSounds.Count() - 1);
            source.clip = (AudioClip)SwordControl.swordSounds[swordSoundIndex];
            if (!source.isPlaying) source.Play();
        }
        TouchGround hitGround = other.GetComponent<TouchGround>();
        if (hitGround != null) // if object touches ground outside scorezone, increase score
        {
            hitGround.UpdateScore(scoreValue);
            hitGround.UpdateScoreDisplay();
            Destroy(gameObject);
        }
        ScoreZone hitScoreZone = other.GetComponent<ScoreZone>();
        if (hitScoreZone != null) // if object lands in scorezone, reduce health
        {
            hitScoreZone.UpdateHealth(damageValue);
            hitScoreZone.UpdateHealthDisplay();
            AudioSource scoreSource = hitScoreZone.GetComponent<AudioSource>();
            if (damageValue > 0) // if object is a healing potion
            {
                scoreSource.clip = SwordControl.healingSound;
                scoreSource.Play();
            } else
            {
                scoreSource.clip = SwordControl.damageSound; 
                scoreSource.Play();
            }
            Destroy(gameObject);
        }
    }
}
