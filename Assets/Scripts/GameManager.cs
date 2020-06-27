using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int remainingLives
    {
        get
        {
            return _remainingLives;
        }
    }
    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public CameraShake cameraShake;
    public float spawnDelay = 2f;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    public delegate void UpgradeMenuCallBack(bool active);
    public UpgradeMenuCallBack onToggleUpgradeMenu;
    private AudioManager audioManager;

    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    [SerializeField]
    private int startingMoney;
    public static int money;

    [SerializeField]
    private WaveSpawner waveSpawner;

    [Range(0f, 2f)]
    public float gameSpeed = 1.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        _remainingLives = maxLives;
    }

    private void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No camera shake referenced on Gamemanager");
        }
        _remainingLives = maxLives;
        money = startingMoney;

        audioManager = AudioManager.instance;

        if (audioManager == null)
        {
            Debug.LogError("No audio Manager available");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void EndGame()
    {
        //Sound
        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("GAME OVER!");
        gameOverUI.SetActive(true);
    }
    public IEnumerator _SpawnPlayer()
    {
        audioManager.PlaySound(respawnCountdownSoundName);
        // GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(clone, 3f);
    }


    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives -= 1;
        if (_remainingLives <= 0)
        {
            instance.EndGame();
        }
        else
        {
            instance.StartCoroutine(instance._SpawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        instance._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        //Sound
        audioManager.PlaySound(_enemy.deathSoundName);

        //Particles
        GameObject particleClone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        //CameraShake
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);

        money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");

        //Cleanup
        Destroy(_enemy.gameObject);
        Destroy(particleClone, 3f);
    }
}
