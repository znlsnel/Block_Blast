using System;
using System.Collections;
using UnityEngine;

public class Utils : Singleton<Utils>   
{
    public void SetTimer(Action ac, float time)
    {
        StartCoroutine(Timer(ac, time));
    }

    IEnumerator Timer(Action ac, float time)
    {
        yield return new WaitForSeconds(time);
        ac?.Invoke();

	}
}
