using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    private PlayerStats stats;

    [SerializeField]
    private float healthMultiplier = 1.3f;
    [SerializeField]
    private float movementSpeedMultiplier = 1.1f;
    [SerializeField]
    private int upgradeCost = 50;
    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    private void UpdateValues()
    {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {

        if (GameManager.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
        GameManager.money -= upgradeCost;
        UpdateValues();
        AudioManager.instance.PlaySound("Money");
        Debug.Log("Health Upgraded!");


    }

    public void UpgradeSpeed()
    {
        if (GameManager.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        //TODO Maybe a POPUP or some kind of notification to the user.
        stats.movementSpeed = Mathf.Round(stats.movementSpeed * movementSpeedMultiplier);
        GameManager.money -= upgradeCost;
        UpdateValues();
        AudioManager.instance.PlaySound("Money");
        Debug.Log("Speed Upgraded!");



    }
}
