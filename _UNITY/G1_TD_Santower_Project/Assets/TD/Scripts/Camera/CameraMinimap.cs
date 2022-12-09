using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CameraMinimap : MonoBehaviour
{
	[SerializeField]
	private List<float> _cameraZoom = new List<float>();

	private Camera _minimapCamera;

	private void OnEnable()
	{
		GameManager.Instance.GamePhaseChangeEvent_UE.RemoveListener(ChangeZoom);
		GameManager.Instance.GamePhaseChangeEvent_UE.AddListener(ChangeZoom);
	}

	private void OnDisable()
	{
		GameManager.Instance.GamePhaseChangeEvent_UE.RemoveListener(ChangeZoom);
	}

	private void Awake()
	{
		_minimapCamera = GetComponent<Camera>();
	}

	private void ChangeZoom(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
	{
		if (toPhase == GameManager.GamePhase.Phase1)
		{
			_minimapCamera.orthographicSize = _cameraZoom[0];
		}
		if (toPhase == GameManager.GamePhase.Phase2)
		{ 
			_minimapCamera.orthographicSize = _cameraZoom[1];
		}
		if (toPhase == GameManager.GamePhase.Phase3)
		{
			_minimapCamera.orthographicSize = _cameraZoom[2];
		}
		if (toPhase == GameManager.GamePhase.Phase4)
		{
			_minimapCamera.orthographicSize = _cameraZoom[3];
		}
	}

}
