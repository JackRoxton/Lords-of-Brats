using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float shakeDuration = 0.2f;
    public AnimationCurve shake;

    public AnimationCurve hitStop;
    float hitStopLength = 0.35f;

    bool inShake = false;
    Vector3 startPos = Vector3.zero;

    public void ScreenShake()
    {
        if (inShake)
        {
            StopAllCoroutines();
            transform.position = startPos;
        }
        StartCoroutine(_ScreenShake());
    }

    public IEnumerator _ScreenShake()
    {
        inShake = true;
        startPos = transform.position;
        float time = 0f;
        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            float strength = shake.Evaluate(time / shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
        inShake = false;
    }

    public void HitStop()
    {
        StartCoroutine(_HitStop());
    }

    IEnumerator _HitStop()
    {
        float timer = 0;
        while (timer < hitStopLength)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = hitStop.Evaluate(timer / hitStopLength);
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            yield return null;
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
    }
}
