using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenAlpha : ThunderTweener
{

    [Range(0f, 1f)] public float from = 1f;
    [Range(0f, 1f)] public float to = 1f;

    Image mSource;

    /// <summary>
    /// Cached version of 'audio', as it's always faster to cache.
    /// </summary>

    public Image image
    {
        get
        {
            if (mSource == null)
            {
                mSource = GetComponent<Image>();

                if (mSource == null)
                {
                    mSource = GetComponent<Image>();

                    if (mSource == null)
                    {
                        Debug.LogError("TweenAlpha  needs an Image to work with", this);
                        enabled = false;
                    }
                }
            }
            return mSource;
        }
    }

    [System.Obsolete("Use 'value' instead")]
    public float alpha { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Audio source's current volume.
    /// </summary>

    public float value
    {
        get
        {
            return image != null ? mSource.color.a : 0f;
        }
        set
        {
            if (image != null)
            {
                Color color = image.color;
                color.a -= value;

                mSource.color = color;
            }
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;
        mSource.enabled = (mSource.color.a > 0.01f);
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenAlpha Begin(GameObject go, float duration, float targetVolume)
    {
        TweenAlpha comp = ThunderTweener.Begin<TweenAlpha>(go, duration);
        comp.from = comp.value;
        comp.to = targetVolume;

        if (targetVolume > 0f)
        {
            var s = comp.image;
            s.enabled = true;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }

}
