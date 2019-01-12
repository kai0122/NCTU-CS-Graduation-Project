using UnityEngine;
using System;
using System.Collections;


public class ShortTween
{
    public static IEnumerator Play(float duration, float startValue, float endValue, Action<float> callback)
    {
        float start = Time.time;
        float end = start + duration;
        float durationInv = 1f / duration;
        float startMulDurationInv = start / duration;
        for (float t = Time.time; t < end; t = Time.time)
        {
            callback(Mathf.Lerp(startValue, endValue, t * durationInv - startMulDurationInv));
            yield return null;
        }
        callback(endValue);
    }
    public static IEnumerator Play(float duration, Action<float> callback)
    {
        return Play(duration, 0f, 1f, callback);
    }
}