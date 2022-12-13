using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour		
{
    [SerializeField]
    private GameObject _pauseUI;

    private float _timeScale;

    void Update()
    { 
        if (Input.GetKeyDown("t"))
        {
			SetPause();
        }
    }

    private void SetPause()
    {
		if (_pauseUI.activeSelf == false)
		{
			_timeScale = Time.timeScale;
			_pauseUI.SetActive(true);
			Time.timeScale = 0;
		}
		else if (_pauseUI.activeSelf == true)
		{
			_pauseUI.SetActive(false);
			Time.timeScale = _timeScale;
		}
	}
}
