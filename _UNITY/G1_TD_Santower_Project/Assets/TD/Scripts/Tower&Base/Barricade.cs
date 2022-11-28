using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
	[System.NonSerialized]
	private List<PathFollower> _pathFollowers = new List<PathFollower>();

	[SerializeField]
	private int _numberBeforeBreaking = 10;

	void Update()
    {
		if (_pathFollowers.Count >= _numberBeforeBreaking)
		{
			DestroyBarricade();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
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
}
