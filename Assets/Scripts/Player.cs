using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();
    public int fallBoundry = -20;
    [SerializeField]
    private StatusIndicator statusIndicator;

    private AudioManager audioManager;
    public string deathSoundName = "Death";
    public string damageSoundName = "Damaged";
    public void DamagePlayer(int damage)
    {
        stats.currentHealth -= damage;
        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);

        if (stats.currentHealth <= 0)
        {
            //Sound
            audioManager.PlaySound(deathSoundName);

            Debug.Log("Player Killed");
            GameManager.KillPlayer(this);
        }
        else
        {
            //Sound
            audioManager.PlaySound(damageSoundName);
        }
    }

    private void Start()
    {
        stats.Init();
        if (statusIndicator == null)
        {
            Debug.LogError("Status indicator null");
        }
        else
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager Found! ");
        }
    }

    private void Update()
    {
        if (transform.position.y <= fallBoundry)
        {
            DamagePlayer(999999);
        }
    }

}
