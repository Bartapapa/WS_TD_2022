using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFog : MonoBehaviour
{
	[SerializeField]
	private List<float> _fogRadius = new List<float>();

	private ParticleSystem _particleSystem;
	
	private ParticleSystem.ShapeModule _shapeModule;

	private void Awake()
	{ 
		_particleSystem = GetComponent<ParticleSystem>();
		_shapeModule= _particleSystem.shape;
	}

	private void OnEnable()
	{		
		GameManager.Instance.GamePhaseChangeEvent_UE.RemoveListener(ChangeFogRadius);
		GameManager.Instance.GamePhaseChangeEvent_UE.AddListener(ChangeFogRadius);
	}

	private void OnDisable()
	{
		GameManager.Instance.GamePhaseChangeEvent_UE.RemoveListener(ChangeFogRadius);
	}

	private void ChangeFogRadius(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
	{
		if (toPhase == GameManager.GamePhase.Phase1)
		{
			_shapeModule.radius = _fogRadius[0];
		}		
		if (toPhase == GameManager.GamePhase.Phase2)
		{
			_shapeModule.radius = _fogRadius[1];
		}		
		if (toPhase == GameManager.GamePhase.Phase3)
		{
			_shapeModule.radius = _fogRadius[2];
		}		
		if (toPhase == GameManager.GamePhase.Phase4)
		{
			_shapeModule.radius = _fogRadius[3];
		}
	}
}
