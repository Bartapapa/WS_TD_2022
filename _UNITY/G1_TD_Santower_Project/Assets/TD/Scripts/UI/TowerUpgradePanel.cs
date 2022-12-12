using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradePanel : MonoBehaviour
{
    [SerializeField]
    private List<UpgradeIconHolder> _upgradeIconHolders = new List<UpgradeIconHolder>();

    [SerializeField]
    private UpgradeIconHolder _upgradeIconHolderPrefab;

    public List<UpgradeIconHolder> UpgradeIconHolders => _upgradeIconHolders;

    public UpgradeIconHolder UpgradeIconHolderPrefab => _upgradeIconHolderPrefab;

    private IEnumerator _openOrClose;
    private Vector3 _originalScale = new Vector3(0.04f, 0.04f, 1);
    private float _currentScale = 1f;

    public void UpdateTowerUpgradePanel()
    {
        foreach (UpgradeIconHolder UIH in _upgradeIconHolders)
        {
            UIH.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        float angle = 360 / _upgradeIconHolders.Count;

        for (int i = 0; i < _upgradeIconHolders.Count; i++)
        {
            _upgradeIconHolders[i].transform.Rotate(new Vector3(0, 0, i * angle));
        }
    }

    public void StartOpenCircle()
    {
        if (_openOrClose != null)
        {
            ForceClose();
        }
        _openOrClose = OpenCircle();
        StartCoroutine(_openOrClose);
    }

    public void StartCloseCircle()
    {
        if (_openOrClose != null)
        {
            ForceClose();
        }
        _openOrClose = CloseCircle();
        StartCoroutine(_openOrClose);
    }

    public IEnumerator OpenCircle()
    {
        _currentScale = 0f;

        while (_currentScale < 0.99f)
        {
            float newScale = Mathf.Lerp(_currentScale, 1, 10f*Time.unscaledDeltaTime);
            _currentScale = newScale;

            transform.localScale = new Vector3(_originalScale.x*_currentScale, _originalScale.y*_currentScale, 1f);
            yield return null;
        }

        _currentScale = 1f;
        transform.localScale = new Vector3(_originalScale.x * _currentScale, _originalScale.y * _currentScale, 1f);
        _openOrClose = null;

        yield return null;
    }

    public IEnumerator CloseCircle()
    {
        _currentScale = 1f;

        while (_currentScale > 0.01f)
        {
            float newScale = Mathf.Lerp(_currentScale, 0, 10f*Time.unscaledDeltaTime);
            _currentScale = newScale;

            transform.localScale = new Vector3(_originalScale.x * _currentScale, _originalScale.y * _currentScale, 1f);
            yield return null;
        }

        _currentScale = 0f;
        transform.localScale = new Vector3(_originalScale.x * _currentScale, _originalScale.y * _currentScale, 1f);
        _openOrClose = null;

        yield return null;
    }

    public void ForceClose()
    {
        StopAllCoroutines();
        _currentScale = 0f;
        transform.localScale = new Vector3(_originalScale.x * _currentScale, _originalScale.y * _currentScale, 1f);

        _openOrClose = null;
    }
}
