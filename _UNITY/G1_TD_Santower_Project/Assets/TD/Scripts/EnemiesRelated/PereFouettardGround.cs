using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PereFouettardGround : MonoBehaviour
{
	[SerializeField]
	private float _fireRadius = 10f;

	[SerializeField]
	private WeaponController _weaponController;

	private List<GameObject> _waypoint = new List<GameObject>();

	private List<GameObject> _tower = new List<GameObject>();

	private GameObject _waypointIndex;

	private PathFollower _pathFollower;

	private Path _path;

	private void Start()
	{
		_pathFollower = GetComponent<PathFollower>();
		GetAllWaypoint();
		GetPath();
		_pathFollower.SetPath(_path, true);

		GetAllTurret();
	}

	private void Update()
	{
		var tower = GetNearestTower();
		if (Vector3.Distance(tower.transform.position, transform.position) <= _fireRadius )
		{
			_pathFollower.SetCanMove(false);
			_weaponController.LookAtAndFire(tower.transform.position);
			if (tower == null)
			{
				RemoveNullItemsFromList();
			}
		}
		else
		{ 
			_pathFollower.SetCanMove(true);
		}
	}

	public void RemoveNullItemsFromList()
	{
		for (var i = _tower.Count - 1; i > -1; i--)
		{
			if (_tower[i] == null)
			{
				_tower.RemoveAt(i);
			}
		}
	}

	private GameObject GetNearestTower()
	{
		float shortestDistance = 0;
		int shortestDistanceIndex = 0;
		for (int i = 0; i < _tower.Count; i++)
		{
			var distance = (_tower[i].transform.position - transform.position).sqrMagnitude;
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				shortestDistanceIndex = i;
			}
		}
		return _tower[shortestDistanceIndex];
	}

	private void GetAllTurret()
	{
		foreach (GameObject Tower in GameObject.FindGameObjectsWithTag("Tower"))
		{
			_tower.Add(Tower);
		}
	}

	private void GetAllWaypoint()
	{
		_waypoint.Clear();
		foreach (GameObject Waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			if (Waypoint.GetComponentInParent<MerryPath>() == false)
			{
				_waypoint.Add(Waypoint);
			}
		}
	}

	private void GetPath()
	{
		if (_waypoint != null)
		{
			var tempGet = _waypoint[0];
			for (int i = 0, length = _waypoint.Count; i < length; i++)
			{
				float distance = Vector3.Distance(_waypoint[i].transform.position, transform.position);
				float targetDistance = Vector3.Distance(tempGet.transform.position, transform.position);

				if (distance < targetDistance)
				{
					tempGet = _waypoint[i];
				}
			}
			_waypointIndex = tempGet;
			_path = _waypointIndex.GetComponentInParent<Path>();
		}
	}
}
