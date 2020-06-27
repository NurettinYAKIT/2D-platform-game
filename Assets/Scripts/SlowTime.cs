using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    public TimeManager tm;
    public float isTime = 0.25f;
    public bool completeTime = false;

    public float slowTime = 0.2f;
    private bool isTimeSlowed = false;

    private float initialFixedDeltaTime;
    private void Start()
    {
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(StartCountdown());
        }
    }

    private void slowTimeSwitch()
    {
        if (isTimeSlowed)
        {
            SlowTimeOff();
        }
        else
        {
            SlowTimeOn();
        }
    }

    private void SlowTimeOn()
    {

        isTimeSlowed = true;
        Time.timeScale = slowTime;
        Time.fixedDeltaTime = slowTime * Time.deltaTime;
    }
    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue = 1)
    {
        SlowTimeOn();
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(0.3f);
            currCountdownValue--;
        }
        SlowTimeOff();
    }
    private void SlowTimeOff()
    {

        isTimeSlowed = false;
        Time.timeScale = 1f;
        // Time.fixedDeltaTime = Time.deltaTime;
        Time.fixedDeltaTime = initialFixedDeltaTime;


    }

}
