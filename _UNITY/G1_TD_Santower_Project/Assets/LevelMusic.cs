using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip _music;

    private void Start()
    {
        if (_music != null)
        {
            SoundManager.Instance.PlayMusic(_music);
        }
    }
}
