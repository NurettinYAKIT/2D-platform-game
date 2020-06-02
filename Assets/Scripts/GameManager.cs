using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public float spawnDelay = 2f;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    public IEnumerator SpawnPlayer()
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
        gameManager.StartCoroutine(gameManager.SpawnPlayer());
    }

    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        // gameManager.StartCoroutine(gameManager.SpawnPlayer());
    }
}
