using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudolphSleigh : MonoBehaviour
{
    [SerializeField]
    ProjectileExplosive _bomb;

    [SerializeField]
    Transform _bombSpawner;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private Timer _bombingInterval;

    [SerializeField]
    private Vector3 _targetPosition;
    private bool _targetPositionSet = false;
    private bool _hasDroppedBomb = false;

    void Update()
    {
        _bombingInterval.Update();
        MoveSleigh();

        if (_bombingInterval.Progress >= 1f)
        {
            DropBomb();
        }

        if (_targetPositionSet)
        {
            Vector3 currentPositionIgnoreY = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 ignoreSetTargetPosition = new Vector3(_targetPosition.x, 0, _targetPosition.z);

            if (Vector3.Distance(currentPositionIgnoreY, ignoreSetTargetPosition) <= 1f && !_hasDroppedBomb)
            {
                DropBomb();
                _hasDroppedBomb = true;
            }
            
        }
    }

    public void DropBomb()
    {
        ProjectileExplosive newBomb = Instantiate(_bomb, _bombSpawner.position, _bombSpawner.rotation);
    }

    private void MoveSleigh()
    {
        transform.position = transform.position + _speed * Time.deltaTime * transform.forward;
    }

    public void StartBombing()
    {
        _bombingInterval.Start();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _targetPositionSet = true;
    }
}
