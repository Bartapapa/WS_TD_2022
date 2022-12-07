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
		if (true)
		{
			GameManager.Instance.ChangePhase(GameManager.GamePhase.Phase2);
		}
	}
}
