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

    [SerializeField]
    private GameObject _credit;

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

    public void OpenCredit()
    {
        _credit.SetActive(true);
    }

    public void CloseCredit()
    {
        _credit.SetActive(false);
    }
}
