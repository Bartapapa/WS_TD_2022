using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour
{
    public delegate void LifespanEvent(Lifespan lifespan);
    public event LifespanEvent lifespanEnded = null;

    [SerializeField]
    private Timer _lifeTimer;
    [SerializeField]
    private bool _destroyOnEnd = true;

    public Timer LifeSpanTimer => _lifeTimer;

    private void Awake()
    {
        _lifeTimer.Start();
    }

    private void Update()
    {
        _lifeTimer.Update();

        if (_lifeTimer.Progress >= 1)
        {
            if (_destroyOnEnd)
            {
                Destroy(this.gameObject);
            }
            else
            {
                lifespanEnded?.Invoke(this);
            }  
        }
    }
}
