using GSGD1;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PereFouettardAir : MonoBehaviour
{
	[SerializeField]
	private float _moveSpeed = 2;

	[SerializeField]
	private float _rotateSpeed = 200;

	[SerializeField]
	private float _innerCicleRad = 5;

	[SerializeField]
	private float _outerCicleRad = 50;

	private float _threshold = 0.5f;

	private PathFollower _pathFollower;

	private GameObject _northPole;

	private Vector3 _innerCiclePos;
	
	private Vector3 _outerCiclePos;
	
	private Vector3 _exitPos;

	[SerializeField]
	private GameObject ball;

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

		LookAt(_northPole.transform.position);
		MoveTo();
    }

	private void FindNorthPole()
	{
		foreach (GameObject northPole in GameObject.FindGameObjectsWithTag("NorthPole"))
		{
			_northPole = northPole;
		}
	}

	private void MoveTo()
	{
		Distance();
		Vector3 movement = (_exitPos - transform.position).normalized * _moveSpeed * Time.deltaTime;
		transform.position += movement;
		if (Vector3.Distance(transform.position, _exitPos.normalized) < _threshold)
		{
			FindNewPos();
		}
	}

	private void Distance()
	{
		_exitPos = (_outerCiclePos + _innerCiclePos) / 2;
	}

	private void LookAt(Vector3 position)
	{
		//transform.LookAt(position, Vector3.up);

		Vector3 direction = position - transform.position;
		direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
	}

	private void FindNewPos()
	{
		_innerCiclePos = RandomPosInRadius(_innerCicleRad);
		_outerCiclePos = RandomPosInRadius(_outerCicleRad);
		transform.position = _outerCiclePos;
	}

	private Vector3 RandomPosInRadius(float radius)
	{
		var Pose = _northPole.transform.position;
		Vector3 randomPos = Random.insideUnitSphere * radius;
		randomPos += Pose;
		randomPos.y = 0f;

		Vector3 direction = randomPos - Pose;
		direction.Normalize();

		float dotProduct = Vector3.Dot(transform.forward, direction);
		float dotProductAngle = Mathf.Acos(dotProduct / transform.forward.magnitude * direction.magnitude);

		randomPos.x = Mathf.Cos(dotProductAngle) * radius + Pose.x;
		randomPos.z = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius + Pose.z;

		return randomPos;
	}
}
