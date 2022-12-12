using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public void PlayClip(AudioClip clip, float percentage = 0f)
    {
        if (percentage > 0f)
        {
            float randomFloat = Random.Range(0f, 100f);
            if (randomFloat > percentage)
            {
                return;
            }
        }

        SoundManager.Instance.PlaySFX(this.gameObject, clip);
    }

    public void PlayRandomClip(List<AudioClip> clips, float percentage = 0f)
    {
        if (percentage > 0f)
        {
            float randomFloat = Random.Range(0f, 100f);
            if (randomFloat > percentage)
            {
                return;
            }
        }

        int randomIndex = Random.Range(0, clips.Count);
        SoundManager.Instance.PlaySFX(this.gameObject, clips[randomIndex]);
    }
}
