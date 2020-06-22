using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _currentHealth;
        public int damage = 40;
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

    public EnemyStats stats = new EnemyStats();
    [Header("Optional:")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    public Transform deathParticles;
    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;
    public string deathSoundName = "Explosion";
    private void Start()
    {
        stats.Init();

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
        if (deathParticles == null)
        {
            Debug.LogError("No death parciles on Enemy!");
        }
    }
    public void Damage(int damage)
    {
        Debug.Log("Enemy Damaged " + damage);

        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            Debug.Log("Enemy Killed");
            GameManager.KillEnemy(this);
        }
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.DamagePlayer(stats.damage);
            Damage(99999);
        }
    }

}
