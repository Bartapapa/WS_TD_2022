using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialManager : MonoBehaviour
{
	[SerializeField]
	private bool _wantTuto = false;

	private void Start()
	{
		if (_wantTuto == false)
		{
			GameManager.Instance.ChangePhase(GameManager.GamePhase.Phase1	);
		}
		else
		{
			GameManager.Instance.ChangePhase(GameManager.GamePhase.Intro);
		}

	}
}
