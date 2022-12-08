using GSGD1;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class PereFouettardAir : MonoBehaviour
{
	[SerializeField]
	private float _moveSpeed = 2;

	[SerializeField]
	private float _rotateSpeed = 200;

	[SerializeField]
	private float _fireRadius = 15;

	[SerializeField]
	private float _innerCicleRad = 5;

	[SerializeField]
	private float _outerCicleRad = 50;
    
	[SerializeField]
	private GameObject _pereFouettardGround;

	private WeaponController _weaponController;

    private Damageable _damageable;

	private float _threshold = 0.5f;

	private PathFollower _pathFollower;

	private GameObject _northPole;

	private Vector3 _innerCiclePos;
	
	private Vector3 _outerCiclePos;
	
	private Vector3 _exitPos;

	private Vector3 _northPoleAimPos;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _weaponController = GetComponentInChildren<WeaponController>();
		
		_pathFollower = GetComponent<PathFollower>();
		_pathFollower.enabled = false;
	
		FindNorthPole();
        FindNewPos();
    }

    private void OnEnable()
    {
        _damageable.CallerDied -= Die;
        _damageable.CallerDied += Die;
    }

    private void OnDisable()
    {
        _damageable.CallerDied -= Die;
    }

    void Update()
    {
		MoveTo(_innerCiclePos);
		LookAt(_northPoleAimPos);
		
		if (Vector3.Distance(_northPoleAimPos, transform.position) <= _fireRadius)
		{
			_weaponController.LookAtAndFire(_northPoleAimPos);
		}
	}

	private void FindNorthPole()
	{
		foreach (GameObject northPole in GameObject.FindGameObjectsWithTag("NorthPole"))
		{
			_northPole = northPole;
			_northPoleAimPos= northPole.GetComponentInChildren<Damageable>().GetAimPosition();
		}
	}

	private void MoveTo(Vector3 position)
	{
		ExitPosition();
		var exitPos = position + -_exitPos;

        Vector3 movement = (exitPos - transform.position).normalized * _moveSpeed * Time.deltaTime;
		transform.position += movement;
		if (Vector3.Distance(transform.position, exitPos) < _threshold)
		{
			FindNewPos();
		}
	}

	private void ExitPosition()
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

	private void Die(Damageable damageable, int currentHealth, int damage)
	{
		Instantiate(_pereFouettardGround, transform.position, Quaternion.identity);
		_damageable.DestroyIfKilled = true;
		_damageable.DoDestroy();
	}
}
