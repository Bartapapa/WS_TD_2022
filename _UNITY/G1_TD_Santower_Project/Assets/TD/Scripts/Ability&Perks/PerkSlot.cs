using GSGD1;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkSlot : MonoBehaviour
{
    [SerializeField]
    private PerkDescription _perkDescription;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Image _ornamentDefault;

    [SerializeField]
    private Image _ornamentAcquired;

    [SerializeField]
    private Image _ornamentUnavailable;

    [SerializeField]
    protected TextMeshProUGUI _milkCostNumber;

    [SerializeField]
    private GameObject _milkIcon;

    private bool _isAcquired = false;
    private bool _isAvailable = false;

    public PerkDescription PerkDescription => _perkDescription;

    public delegate void PerkSlotEvent(PerkSlot sender);
    public event PerkSlotEvent OnPerkSlotClicked = null;

    private void Awake()
    {
        //UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (_perkDescription == null)
        {
            Debug.LogErrorFormat("{0}.PerkSlot() Missing _perkDescription reference in {1}.", GetType().Name, name);
            return;
        }

        _icon.sprite = _perkDescription.Icon;
        _milkCostNumber.text = _perkDescription.MilkCost.ToString();

        if (_isAvailable)
        {
            if (_isAcquired)
            {
                _button.enabled = false;

                _ornamentUnavailable.gameObject.SetActive(false);
                _ornamentDefault.gameObject.SetActive(false);
                _ornamentAcquired.gameObject.SetActive(true);

                _milkIcon.SetActive(false);
            }
            else
            {
                _button.enabled = true;

                _ornamentUnavailable.gameObject.SetActive(false);
                _ornamentDefault.gameObject.SetActive(true);
                _ornamentAcquired.gameObject.SetActive(false);

                _milkIcon.SetActive(true);
            }
        }
        else
        {
            _button.enabled = false;

            _ornamentUnavailable.gameObject.SetActive(true);
            _ornamentDefault.gameObject.SetActive(false);
            _ornamentAcquired.gameObject.SetActive(false);

            _milkIcon.SetActive(false);
        }

    }

    public void InitializeSlot(PerkDescription perkDescription)
    {
        _perkDescription = perkDescription;
    }

    private void OnEnable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        OnPerkSlotClicked?.Invoke(this);
    }

    public void SetIsAcquired(bool value)
    {
        _isAcquired = value;
    }

    public void SetIsAvailable(bool value)
    {
        _isAvailable = value;
    }

}
