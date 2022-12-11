using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    [SerializeField]
    private float _freezeDuration = 1f;

    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _distanceThreshold = 0.5f;

    [SerializeField]
    private AnimatorHandler _anim;

    private bool _target = false;

    private GameObject _towerTarget;

    private PathFollower _pathFollower;

    private void Awake()
    {
        _pathFollower = GetComponent<PathFollower>();
    }

    private void Update()
    {
        if (_target == true)
        {
            MoveTo(_towerTarget.transform.position);
            if (Vector3.Distance(transform.position, _towerTarget.transform.position) < _distanceThreshold)
            {
                _towerTarget.GetComponent<Freezer>().Freeze(_freezeDuration);

                _anim.Animator.SetTrigger("Attack");
            }
            if (_towerTarget.GetComponent<Freezer>().IsFrozen == false)
            {
                _target = false;
                _pathFollower.enabled = true;
            }
        }
    }

    private void MoveTo(Vector3 position)
    {
        Vector3 movement = (position - transform.position).normalized * _speed * Time.deltaTime;
        transform.position += movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Freezer>().IsFrozen == false)
        {
            _pathFollower.enabled = false;
            _towerTarget = other.gameObject;
            _target = true;
            _anim.Animator.SetFloat("Multiplier", 2f);
        }
    }
}
