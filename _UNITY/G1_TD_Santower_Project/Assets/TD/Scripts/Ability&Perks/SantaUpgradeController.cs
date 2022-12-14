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
    private PerkSlot _perkSlotPrefab;

    [SerializeField]
    private CinemachineVirtualCamera _camera;

    [SerializeField]
    private SantaUpgradePanel _santaUpgradePanel;

    private void Awake()
    {
        _santaUpgradePanel.gameObject.SetActive(false);

        //for (int i = 0; i < PlayerPerkManager.Instance.PerkList.Count; i++)
        //{
        //    PerkSlot newPerkSlot = Instantiate<PerkSlot>(_perkSlotPrefab, _upgradePanel.transform);
        //    newPerkSlot.InitializeSlot(PlayerPerkManager.Instance.PerkList[i]);
        //    newPerkSlot.UpdateSlot();
        //    _perkSlots[i] = newPerkSlot;
        //    newPerkSlot.gameObject.SetActive(false);
        //}

        _santaUpgradePanel.InitializePanel();
        UpdateCurrentAvailablePerk();
    }

    private void OnEnable()
    {
        for (int i = 0, length = _santaUpgradePanel.PerkSlots.Count; i < length; i++)
        {
            _santaUpgradePanel.PerkSlots[i].OnPerkSlotClicked -= SantaUpgradeController_OnPerkSlotClicked;
            _santaUpgradePanel.PerkSlots[i].OnPerkSlotClicked += SantaUpgradeController_OnPerkSlotClicked;
        }

        _selectable.ObjectSelected -= OnObjectSelected;
        _selectable.ObjectSelected += OnObjectSelected;
    }

    private void OnDisable()
    {
        for (int i = 0, length = _santaUpgradePanel.PerkSlots.Count; i < length; i++)
        {
            _santaUpgradePanel.PerkSlots[i].OnPerkSlotClicked -= SantaUpgradeController_OnPerkSlotClicked;
        }

        _selectable.ObjectSelected -= OnObjectSelected;
    }

    private void SantaUpgradeController_OnPerkSlotClicked(PerkSlot sender)
    {
        //When clicked, do this.
        if (ResourceManager.Instance.CanBuy(ResourceManager.ResourceType.Milk, sender.PerkDescription.MilkCost))
        {
            PlayerPerkManager.Instance.AcquirePerk(PlayerPerkManager.Instance.CurrentPerk + 1);

            sender.SetIsAcquired(true);
            sender.UpdateSlot();

            UpdateCurrentAvailablePerk();
        }
    }

    private void UpdateCurrentAvailablePerk()
    {
        for (int i = 0; i < _santaUpgradePanel.PerkSlots.Count; i++)
        {

            if (i < PlayerPerkManager.Instance.CurrentPerk + 1)
            {
                _santaUpgradePanel.PerkSlots[i].SetIsAcquired(true);
                _santaUpgradePanel.PerkSlots[i].UpdateSlot();
            }
            else if (i == PlayerPerkManager.Instance.CurrentPerk + 1)
            {
                _santaUpgradePanel.PerkSlots[i].SetIsAvailable(true);
                _santaUpgradePanel.PerkSlots[i].UpdateSlot();
            }
            else
            {
                _santaUpgradePanel.PerkSlots[i].SetIsAvailable(false);
                _santaUpgradePanel.PerkSlots[i].UpdateSlot();
            }
        }
    }

    private void OnObjectSelected(SelectableObject selectableObject, bool isSelected)
    {
        _santaUpgradePanel.gameObject.SetActive(isSelected);
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
