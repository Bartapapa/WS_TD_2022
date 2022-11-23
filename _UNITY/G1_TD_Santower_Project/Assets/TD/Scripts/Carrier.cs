using Cinemachine.Utility;
using GSGD1;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carrier : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> _pathFollowers= new List<GameObject>();

	[SerializeField]
	private float _randomSpawnAmplitude = 1.0f;
	private int a;
	private void Start()
	{
		Spawn();
	}

	private void Spawn()
	{
		for (int i = 0; i < _pathFollowers.Count; i++)
		{
			Vector3 spawnPos = new Vector3(
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.x,
								transform.position.y,
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.z);

			//PathFollower aaaa = _pathFollowers[i].GetComponents<PathFollower>;
			if ( _randomSpawnAmplitude == 1)
			{
				Instantiate(_pathFollowers[i], spawnPos, Quaternion.identity);
			}
		}
	}
}
