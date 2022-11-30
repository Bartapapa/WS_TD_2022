using GSGD1;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AirPereFouettard : MonoBehaviour
{
	[SerializeField]
	private float _moveSpeed = 2;

	[SerializeField]
	private float _rotateSpeed = 200;

	[SerializeField]
	private float _innerCicle = 5;

	[SerializeField]
	private float _outerCicle = 50;

	private float _threshold = 0.5f;

	private PathFollower _pathFollower;

	private GameObject _northPole;

	private Vector3 _northPoleInnerCircle;

	private Vector3 _northPoleOuterCircle;

	private void DoOnEnable()
	{
		_pathFollower= GetComponent<PathFollower>();
		_pathFollower.enabled= false;
		FindNorthPole();
	}

	void Update()
    {
		if (_pathFollower == null)
		{
			DoOnEnable();
			NewPoint();
		}

		LookAt(_northPole.transform.position);
		MoveTo(_northPoleOuterCircle);


    }

	private void FindNorthPole()
	{
		foreach (GameObject northPole in GameObject.FindGameObjectsWithTag("NorthPole"))
		{
			_northPole = northPole;
		}
	}

	private Vector3 NorthPoleCircle(float offset)
	{
		var pole = new Vector3(
						_northPole.transform.position.x + Random.Range(-offset, offset),
						10,
						_northPole.transform.position.z + Random.Range(-offset, offset));
		return pole;
	}

	private void NewPoint()
	{
		_northPoleInnerCircle = NorthPoleCircle(_innerCicle);
		_northPoleOuterCircle = NorthPoleCircle(_outerCicle);
		transform.position = new Vector3 (-_northPoleOuterCircle.x, transform.position.y, -_northPoleOuterCircle.z);
	}

	private void MoveTo(Vector3 position)
	{
		Vector3 movement = (position - transform.position).normalized * _moveSpeed * Time.deltaTime;
		transform.position += movement;
		if (Vector3.Distance(transform.position, position) < _threshold)
		{
			NewPoint();
		}
	}

	private void LookAt(Vector3 position)
	{
		//transform.LookAt(position, Vector3.up);

		Vector3 direction = position - transform.position;
		direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
	}
}
