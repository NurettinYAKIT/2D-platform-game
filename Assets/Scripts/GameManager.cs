using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public CameraShake cameraShake;
    public float spawnDelay = 2f;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    private void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No camera shake referenced on Gamemanager");
        }
    }

    public IEnumerator _SpawnPlayer()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(clone, 3f);
    }


    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        gameManager.StartCoroutine(gameManager._SpawnPlayer());
    }

    public static void KillEnemy(Enemy enemy)
    {
        gameManager._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
