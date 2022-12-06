using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMinimap : MonoBehaviour
{
	[SerializeField]
	private float _cameraZoom = 15;

	private Camera _minimapCamera;

	private void Awake()
	{
		_minimapCamera = GetComponent<Camera>();
	}

	private void Update()
	{
		_minimapCamera.orthographicSize = _cameraZoom;
	}

}
