using TMPro;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public int health = 100;
    int healthMax = 100;
    int healthMin = 0;
    [SerializeField]
    TextMeshProUGUI healthLabel;
    AudioClip gameOverSound;

    private void OnTriggerEnter(Collider other)
    {

    }

    public void UpdateHealth(int value)
    {
        health += value;
        if (health > healthMax)
        {
            health = healthMax;
        }
        else if (health <= healthMin)
        {
            health = healthMin;
            AudioSource source = GetComponent<AudioSource>();
            gameOverSound = (AudioClip)Resources.Load("Audio/Crash");
            source.clip = gameOverSound;
            source.Play();
            PauseControl.instance.GameOver();
        }
    }

    public void UpdateHealthDisplay()
    {
        healthLabel.SetText("Health: " + health);
    }
}
