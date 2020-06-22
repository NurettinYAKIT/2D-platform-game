using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

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

    private AudioManager audioManager;

    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

        audioManager = AudioManager.instance;

        if (audioManager == null)
        {
            Debug.LogError("No audio Manager available");
        }
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
            gameManager.EndGame();
        }
        else
        {
            gameManager.StartCoroutine(gameManager._SpawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gameManager._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        //Sound
        audioManager.PlaySound(_enemy.deathSoundName);

        //Particles
        GameObject particleClone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        //CameraShake
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);

        //Cleanup
        Destroy(_enemy.gameObject);
        Destroy(particleClone, 3f);
    }
}
