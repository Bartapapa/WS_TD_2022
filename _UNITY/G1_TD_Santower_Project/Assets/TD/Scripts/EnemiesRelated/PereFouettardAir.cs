using GSGD1;
using UnityEditor;
using UnityEngine;

public class PereFouettardAir : MonoBehaviour
{
	[SerializeField]
	private float _moveSpeed = 2;

	[SerializeField]
	private float _rotateSpeed = 200;

	[SerializeField]
	private float _fireRadius = 7;

	[SerializeField]
	private float _innerCicleRad = 5;

	[SerializeField]
	private float _outerCicleRad = 50;

	[SerializeField]
	private WeaponController _weaponController;

	private float _threshold = 0.5f;

	private PathFollower _pathFollower;

	private GameObject _northPole;

	private Vector3 _innerCiclePos;
	
	private Vector3 _outerCiclePos;
	
	private Vector3 _exitPos;

	private void DoOnEnable()
	{
		_pathFollower = GetComponent<PathFollower>();
		_pathFollower.enabled = false;
		FindNorthPole();
	}

	void Update()
    {
		if (_pathFollower == null)
		{
			DoOnEnable();
			FindNewPos();
		}

		//MoveTo(_innerCiclePos);
		LookAt(_northPole.transform.position);
		//if (Vector3.Distance(_northPole.transform.position, transform.position) <= _fireRadius )
		{
			//Debug.Log("ssssssssss");
			_weaponController.LookAtAndFire(_northPole.transform.position);
		}
		//Debug.Log(Vector3.Distance(_northPole.transform.position, transform.position));
    }

	private void FindNorthPole()
	{
		foreach (GameObject northPole in GameObject.FindGameObjectsWithTag("NorthPole"))
		{
			_northPole = northPole;
		}
	}

	private void MoveTo(Vector3 position)
	{
		Distance();
		var oui = position + -_exitPos;

        Vector3 movement = (oui - transform.position).normalized * _moveSpeed * Time.deltaTime;
		transform.position += movement;
		if (Vector3.Distance(transform.position, oui) < _threshold)
		{
			FindNewPos();
		}
	}

	private void Distance()
	{
		_exitPos = _outerCiclePos - _innerCiclePos;
	}

	private void LookAt(Vector3 position)
	{
		Vector3 direction = position - transform.position;
		direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
	}

	private void FindNewPos()
	{
		_outerCiclePos = RandomPosInRadius(_outerCicleRad);
		_innerCiclePos = RandomPosInRadius(_innerCicleRad);
		transform.position = _outerCiclePos;
	}

	private Vector3 RandomPosInRadius(float radius)
	{
		var Pose = _northPole.transform.position;
		Vector3 randomPos = Random.insideUnitSphere * radius;
		randomPos += Pose;
		randomPos.y = 12f;

		Vector3 direction = randomPos - Pose;
		direction.Normalize();

		float dotProduct = Vector3.Dot(transform.forward, direction);
		float dotProductAngle = Mathf.Acos(dotProduct / transform.forward.magnitude * direction.magnitude);

		randomPos.x = Mathf.Cos(dotProductAngle) * radius + Pose.x;
		randomPos.z = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius + Pose.z;

		return randomPos;
	}
}
