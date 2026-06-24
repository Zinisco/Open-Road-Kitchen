using UnityEngine;

public class BurgerCombiner : MonoBehaviour
{
    [SerializeField] private KitchenItem rawBurgerPrefab;
    [SerializeField] private KitchenItem cookedBurgerPrefab;
    [SerializeField] private KitchenItem burntBurgerPrefab;

    public bool TryCombine(KitchenItem itemA, KitchenItem itemB, Transform parentPoint, out KitchenItem result)
    {
        result = null;

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

        result = Instantiate(prefabToSpawn);

        if (parentPoint != null)
        {
            result.transform.SetParent(parentPoint);
            result.transform.localPosition = Vector3.zero;
            result.transform.localRotation = Quaternion.identity;
        }

        Rigidbody rb = result.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider col = result.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

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
}