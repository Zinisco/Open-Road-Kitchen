using UnityEngine;

public class BurgerCombiner : MonoBehaviour
{
    [SerializeField] private KitchenItem rawBurgerPrefab;
    [SerializeField] private KitchenItem cookedBurgerPrefab;
    [SerializeField] private KitchenItem burntBurgerPrefab;

    [SerializeField] private KitchenItem cookedCheeseburgerPrefab;

    public bool TryCombine(KitchenItem itemA, KitchenItem itemB, Transform parentPoint, out KitchenItem result)
    {
        result = null;

        if (CanMakeCookedCheeseburger(itemA, itemB))
        {
            Destroy(itemA.gameObject);
            Destroy(itemB.gameObject);

            result = SpawnResult(cookedCheeseburgerPrefab, parentPoint);
            return true;
        }

        KitchenItem patty = null;
        KitchenItem bun = null;

        if (itemA.ItemType == KitchenItemType.BurgerBun)
            bun = itemA;

        if (itemB.ItemType == KitchenItemType.BurgerBun)
            bun = itemB;

        if (IsPatty(itemA))
            patty = itemA;

        if (IsPatty(itemB))
            patty = itemB;

        if (bun == null || patty == null)
            return false;

        KitchenItem prefabToSpawn = GetBurgerPrefab(patty.ItemType);

        if (prefabToSpawn == null)
            return false;

        Destroy(itemA.gameObject);
        Destroy(itemB.gameObject);

        result = SpawnResult(prefabToSpawn, parentPoint);
        return true;
    }

    private bool IsPatty(KitchenItem item)
    {
        return item.ItemType == KitchenItemType.BurgerPattyRaw ||
               item.ItemType == KitchenItemType.BurgerPattyCooked ||
               item.ItemType == KitchenItemType.BurgerPattyBurnt;
    }

    private KitchenItem GetBurgerPrefab(KitchenItemType pattyType)
    {
        switch (pattyType)
        {
            case KitchenItemType.BurgerPattyRaw:
                return rawBurgerPrefab;

            case KitchenItemType.BurgerPattyCooked:
                return cookedBurgerPrefab;

            case KitchenItemType.BurgerPattyBurnt:
                return burntBurgerPrefab;

            default:
                return null;
        }
    }

    private bool CanMakeCookedCheeseburger(KitchenItem itemA, KitchenItem itemB)
    {
        bool hasCookedBurger =
            itemA.ItemType == KitchenItemType.BurgerCooked ||
            itemB.ItemType == KitchenItemType.BurgerCooked;

        bool hasCheeseSlices =
            itemA.ItemType == KitchenItemType.CheeseSlices ||
            itemB.ItemType == KitchenItemType.CheeseSlices;

        return hasCookedBurger && hasCheeseSlices;
    }

    private KitchenItem SpawnResult(KitchenItem prefab, Transform parentPoint)
    {
        KitchenItem item = Instantiate(prefab);

        if (parentPoint != null)
        {
            item.transform.SetParent(parentPoint);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        return item;
    }
}