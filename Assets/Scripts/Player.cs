using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityStandardAssets._2D.Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    public int fallBoundry = -20;
    [SerializeField]
    private StatusIndicator statusIndicator;

    private AudioManager audioManager;
    public string deathSoundName = "Death";
    public string damageSoundName = "Damaged";

    private PlayerStats stats;
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
        stats = PlayerStats.instance;
        stats.currentHealth = stats.maxHealth;

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
        GameManager.instance.onToggleUpgradeMenu += OnUpgrageMenuToggle;

        InvokeRepeating("RegenerateHealth", 1 / stats.healthRegenRate, 1 / stats.healthRegenRate);
    }

    private void Update()
    {
        if (transform.position.y <= fallBoundry)
        {
            DamagePlayer(999999);
        }
    }

    private void OnUpgrageMenuToggle(bool active)
    {
        Debug.Log("Paused.");
        GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().enabled = !active;
        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.enabled = !active;
        }
    }

    void RegenerateHealth()
    {
        stats.currentHealth += 1;
        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }

    private void OnDestroy()
    {
        GameManager.instance.onToggleUpgradeMenu -= OnUpgrageMenuToggle;
    }


}
