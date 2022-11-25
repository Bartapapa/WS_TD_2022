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
    protected TextMeshProUGUI _milkCostNumber;

    public PerkDescription PerkDescription => _perkDescription;

    public delegate void PerkSlotEvent(PerkSlot sender);
    public event PerkSlotEvent OnPerkSlotClicked = null;

    private void Awake()
    {
        UpdateSlot();
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

}
