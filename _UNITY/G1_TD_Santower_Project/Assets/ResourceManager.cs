using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public enum ResourceType
    {
        Cookie,
        Milk,
        None,
    }

    public delegate void ResourceEvent(ResourceManager.ResourceType resourceType, int quantityGained, int newQuantity);
    public event ResourceEvent ResourcesUpdated = null;

    [Header("Resources")]
    [SerializeField] private int _cookie = 0;
    [SerializeField] private int _milk = 0;
    public int Cookie => _cookie;
    public int Milk => _milk;

    protected override void Start()
    {
        base.Start();
        if (ResourcesUpdated != null)
        {
            ResourcesUpdated.Invoke(ResourceManager.ResourceType.Cookie, _cookie, _cookie);
            ResourcesUpdated.Invoke(ResourceManager.ResourceType.Milk, _milk, _milk);
        }
    }

    public void AcquireResource(ResourceManager.ResourceType resourceType, int quantityGained)
    {
        if (resourceType == ResourceManager.ResourceType.Cookie)
        {
            _cookie += quantityGained;

            if (ResourcesUpdated != null)
            {
                ResourcesUpdated.Invoke(resourceType, quantityGained, _cookie);
            }

        }

        if (resourceType == ResourceManager.ResourceType.Milk)
        {
            _milk += quantityGained;

            if (ResourcesUpdated != null)
            {
                ResourcesUpdated.Invoke(resourceType, quantityGained, _milk);
            }

        }
    }

    public bool CanBuy(ResourceManager.ResourceType resourceType, int quantityToUse)
    {
        if (resourceType == ResourceManager.ResourceType.Cookie)
        {
            if (quantityToUse > _cookie)
            {
                Debug.Log("You don't have enough cookies for that!");
                return false;
            }
        }

        if (resourceType == ResourceManager.ResourceType.Milk)
        {
            if (quantityToUse > _milk)
            {
                Debug.Log("You don't have enough milk for that!");
                return false;
            }
        }

        return true;
    }
}
