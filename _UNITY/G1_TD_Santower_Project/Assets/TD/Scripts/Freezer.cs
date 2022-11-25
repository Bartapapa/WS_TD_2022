using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    [Header("Freeze Timer")]
    [SerializeField]
    private Timer _freezeTimer;

    [Header("Frozen objects")]
    [SerializeField]
    private MonoBehaviour[] _scriptsToFreeze;
    [SerializeField]
    private bool _isFrozen;

    [Header("Special FX")]
    [SerializeField]
    private AudioClip _freezeSound;
    [SerializeField]
    private AudioClip _unfreezeSound;
    [SerializeField]
    private GameObject _freezeFX;
    [SerializeField]
    private ParticleSystem _freezeParticles;

    public bool IsFrozen => _isFrozen;

    private void Update()
    {
        _freezeTimer.Update();
        if (_freezeTimer.Progress >= 1)
        {
            Unfreeze();
        }
    }
    public void Freeze(float freezeDuration)
    {
        if (_freezeTimer.IsRunning)
        {
            if(freezeDuration > _freezeTimer.Duration)
            {
                _freezeTimer.Stop();
                _freezeTimer.Set(freezeDuration);
                _freezeTimer.Start();
            }
        }
        else
        {
            _freezeTimer.Set(freezeDuration);
            _freezeTimer.Start();
        }

        foreach (MonoBehaviour script in _scriptsToFreeze)
        {
            script.enabled = false;
        }

        _isFrozen = true;

        //Play freeze sound, enable GFX, create particles...
    }

    public void Unfreeze()
    {
        foreach (MonoBehaviour script in _scriptsToFreeze)
        {
            script.enabled = true;
        }

        _isFrozen = false;

        //Play unfreeze sound, disable GFX, create particles...
    }
}
