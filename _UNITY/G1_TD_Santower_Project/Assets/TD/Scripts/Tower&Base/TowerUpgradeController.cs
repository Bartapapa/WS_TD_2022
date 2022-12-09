using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeController : MonoBehaviour
{
    [SerializeField]
    private Tower _tower;

    [SerializeField]
    private SelectableObject _selectable;

    [SerializeField]
    private GameObject _upgradePanel;

    [SerializeField]
    private TowerSlot[] _towerSlots;

    [SerializeField]
    private TowerSlot _towerSlotPrefab;

    private void Awake()
    {
        _upgradePanel.SetActive(false);

        _tower = GetComponentInParent<Tower>();
        _selectable = _tower.gameObject.GetComponentInChildren<SelectableObject>();

        _towerSlots = new TowerSlot[_tower.TowerDescription.UpgradeList.Count];

        for (int i = 0; i < _tower.TowerDescription.UpgradeList.Count; i++)
        {
            TowerSlot newTowerSlot = Instantiate<TowerSlot>(_towerSlotPrefab, _upgradePanel.transform);
            newTowerSlot.InitializeSlot(_tower.TowerDescription.UpgradeList[i]);
            newTowerSlot.UpdateSlot();
            _towerSlots[i] = newTowerSlot;
        }
    }

    private void OnEnable()
    {
        for (int i = 0, length = _towerSlots.Length; i < length; i++)
        {
            _towerSlots[i].OnTowerSlotClicked -= TowerUpgradeController_OnTowerSlotClicked;
            _towerSlots[i].OnTowerSlotClicked += TowerUpgradeController_OnTowerSlotClicked;
        }
        _selectable.ObjectSelected -= OnObjectSelected;
        _selectable.ObjectSelected += OnObjectSelected;
    }

    private void OnDisable()
    {
        for (int i = 0, length = _towerSlots.Length; i < length; i++)
        {
            _towerSlots[i].OnTowerSlotClicked -= TowerUpgradeController_OnTowerSlotClicked;
        }
        _selectable.ObjectSelected -= OnObjectSelected;
    }

    private void TowerUpgradeController_OnTowerSlotClicked(TowerSlot sender)
    {
        //When clicked, do this.
        if (ResourceManager.Instance.CanBuy(ResourceManager.ResourceType.Cookie, sender.TowerDescription.CookieCost))
        {
            Tower upgradedTower = sender.TowerDescription.Instantiate(sender.TowerDescription.Prefab);
            upgradedTower.transform.position = _tower.transform.position;
            //upgradedTower.GetTransform().position = _tower.GetParent().position;
            //upgradedTower.transform.parent = _tower.GetParent();
            upgradedTower.Enable(true);

            if (sender.TowerDescription.CookieCost >= 0)
            {
                ResourceManager.Instance.AcquireResource(ResourceManager.ResourceType.Cookie, -sender.TowerDescription.CookieCost);
                upgradedTower.SetTotalCookieCost(_tower.GetTotalCookieCost + sender.TowerDescription.CookieCost);
            }
            else
            {
                ResourceManager.Instance.AcquireResource(ResourceManager.ResourceType.Cookie, _tower.GetTotalCookieCost);
                //TODO Make a formula here to only acquire a % of totalCookieCost
                upgradedTower.SetTotalCookieCost(0);
            }


            _upgradePanel.SetActive(false);
            _tower.KillTower();
        }
    }

    private void OnObjectSelected(SelectableObject selectableObject, bool isSelected)
    {
        _upgradePanel.SetActive(isSelected);
    }
}
