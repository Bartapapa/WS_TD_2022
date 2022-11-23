using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNorthPole : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

	private void Update()
	{
		if (_damageable.GetHealth <= 0)
		{
			// Insert Code for defeat screen
			QuitToEditor();
		}
	}

	private void QuitToEditor()
	{
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
