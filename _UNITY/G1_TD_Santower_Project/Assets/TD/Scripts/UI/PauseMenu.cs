using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
	[Header("Options")]
	[SerializeField]
	private Slider _volume;

	private void Update()
	{
		SoundManager.Instance.SetVolume(_volume.value);
	}
}
