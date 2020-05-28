using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int health = 100;
    }

    public PlayerStats playerStats = new PlayerStats();
    public int fallBoundry = -20;
    public void DamagePlayer(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            Debug.Log("Player Killed");
            GameManager.KillPlayer(this);
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
