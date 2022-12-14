using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetKinematic : MonoBehaviour
{
    [SerializeField]
    private Timer _timer;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _timer.Start();
    }

    private void Update()
    {
        _timer.Update();

        if(_timer.Progress >= 1f)
        {
            Kinematic();
        }
    }

    private void Kinematic()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
    }
}
