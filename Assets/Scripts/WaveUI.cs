using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    [SerializeField]
    WaveSpawner waveSpawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountDownText;
    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

    // Start is called before the first frame update
    void Start()
    {
        if (waveSpawner == null)
        {
            Debug.LogError("No Spawner Referenced");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator Referenced");
            this.enabled = false;
        }
        if (waveCountDownText == null)
        {
            Debug.LogError("No waveCountDownText Referenced");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText Referenced");
            this.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (waveSpawner.state)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        previousState = waveSpawner.state;
    }

    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountDown", true);
        }
        waveCountDownText.text = "" + (int)waveSpawner.WaveCountdown;

    }

    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveIncoming", true);
            waveAnimator.SetBool("WaveCountDown", false);
            waveCountText.text = "" + (int)waveSpawner.NextWave;
        }

    }
}
