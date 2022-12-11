using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField]
    private AudioClip _mainMenuMusic;

    [Header("Options")]
    [SerializeField]
    private Slider _volume;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(_mainMenuMusic);
    }

    private void Update()
    {
        SoundManager.Instance.SetVolume(_volume.value);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
