using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _cookieCount;
    [SerializeField]
    private TextMeshProUGUI _milkCount;

    private void OnEnable()
    {
        ResourceManager.Instance.ResourcesUpdated -= UpdateUI;
        ResourceManager.Instance.ResourcesUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.ResourcesUpdated -= UpdateUI;
    }

    private void UpdateUI(ResourceManager.ResourceType resourceType, int quantityGained, int newQuantity)
    {
        if (resourceType == ResourceManager.ResourceType.Cookie)
        {
            _cookieCount.text = newQuantity.ToString();
        }

        if (resourceType == ResourceManager.ResourceType.Milk)
        {
            _milkCount.text = newQuantity.ToString();
        }
    }
}