using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IPickerGhost
{
	[System.NonSerialized]
	private List<PathFollower> _pathFollowers = new List<PathFollower>();

	[SerializeField]
	private int _numberBeforeBreaking = 10;

	private bool _canBlock = false;

	void Update()
    {
		if (_pathFollowers.Count >= _numberBeforeBreaking)
		{
			DestroyBarricade();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!_canBlock) return;

		PathFollower pathFolower = other.GetComponentInParent<PathFollower>();

		if (pathFolower != null && _pathFollowers.Contains(pathFolower) == false)
		{
			pathFolower.SetCanMove(false);
			_pathFollowers.Add(pathFolower);
		}
	}

	private void DestroyBarricade()
	{
		for (int i = 0; i < _pathFollowers.Count; i++)
		{
			_pathFollowers[i].SetCanMove(true);
		}
		Destroy(gameObject);
	}

	public void SetCanBlock(bool value)
	{
		_canBlock = value;
	}

	public Transform GetTransform()
	{
		return transform;
	}
}
