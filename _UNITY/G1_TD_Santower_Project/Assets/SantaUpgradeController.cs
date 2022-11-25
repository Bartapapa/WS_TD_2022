using Cinemachine;
using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaUpgradeController : MonoBehaviour
{
    [SerializeField]
    private SelectableObject _selectable;

    [SerializeField]
    private GameObject _upgradePanel;

    [SerializeField]
    private PerkSlot[] _perkSlots;

    [SerializeField]
    private PerkSlot _perkSlotPrefab;

    [SerializeField]
    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _upgradePanel.SetActive(false);

        _perkSlots = new PerkSlot[PlayerPerkManager.Instance.PerkList.Count];

        for (int i = 0; i < PlayerPerkManager.Instance.PerkList.Count; i++)
        {
            PerkSlot newPerkSlot = Instantiate<PerkSlot>(_perkSlotPrefab, _upgradePanel.transform);
            newPerkSlot.InitializeSlot(PlayerPerkManager.Instance.PerkList[i]);
            newPerkSlot.UpdateSlot();
            _perkSlots[i] = newPerkSlot;
            newPerkSlot.gameObject.SetActive(false);
            //Instantiate, then setactive(false).
            //Start listening to the 
        }

        UpdateCurrentShownPerk();
    }

    private void OnEnable()
    {
        for (int i = 0, length = _perkSlots.Length; i < length; i++)
        {
            _perkSlots[i].OnPerkSlotClicked -= SantaUpgradeController_OnPerkSlotClicked;
            _perkSlots[i].OnPerkSlotClicked += SantaUpgradeController_OnPerkSlotClicked;
        }

        _selectable.ObjectSelected -= OnObjectSelected;
        _selectable.ObjectSelected += OnObjectSelected;
    }

    private void OnDisable()
    {
        for (int i = 0, length = _perkSlots.Length; i < length; i++)
        {
            _perkSlots[i].OnPerkSlotClicked -= SantaUpgradeController_OnPerkSlotClicked;
        }

        _selectable.ObjectSelected -= OnObjectSelected;
    }

    private void SantaUpgradeController_OnPerkSlotClicked(PerkSlot sender)
    {
        //When clicked, do this.
        if (ResourceManager.Instance.CanBuy(ResourceManager.ResourceType.Milk, sender.PerkDescription.MilkCost))
        {
            PlayerPerkManager.Instance.AcquirePerk(PlayerPerkManager.Instance.CurrentPerk + 1);

            UpdateCurrentShownPerk();
        }
    }

    private void UpdateCurrentShownPerk()
    {
        for (int i = 0; i < _perkSlots.Length; i++)
        {
            _perkSlots[i].gameObject.SetActive(false);
            if (i == PlayerPerkManager.Instance.CurrentPerk+1)
            {
                _perkSlots[i].gameObject.SetActive(true);
            }
        }
    }

    private void OnObjectSelected(SelectableObject selectableObject, bool isSelected)
    {
        _upgradePanel.SetActive(isSelected);
        _camera.gameObject.SetActive(isSelected);

        if (isSelected)
        {
            Time.timeScale = 0.1f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
