using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryDefeat : MonoBehaviour
{
	[SerializeField]
	private GameObject _victoryScreen;

	[SerializeField]
	private GameObject _defeatScreen;

	private GameManager _gameManager;


	private void Awake()
	{
		_gameManager = GameManager.Instance;
	}

	private void OnEnable()
	{
		_gameManager.GamePhaseChangeEvent_UE.RemoveListener(VictoryDefeat);
		_gameManager.GamePhaseChangeEvent_UE.AddListener(VictoryDefeat);
	}

	private void OnDisable()
	{
		_gameManager.GamePhaseChangeEvent_UE.RemoveListener(VictoryDefeat);
	}

	private void VictoryDefeat(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
	{
		if (toPhase == GameManager.GamePhase.GameWin)
		{
			_victoryScreen.SetActive(true);
		}
		if (toPhase == GameManager.GamePhase.GameLose)
		{
			_defeatScreen.SetActive(true);
		}
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Menu()
	{
		GameManager.Instance.ReturnToMainMenu();
	}
}
