using System.Collections;
using UnityEngine;

public class Grill : KitchenSurface
{
    [Header("Grill")]
    [SerializeField] private float cookTime = 3f;
    [SerializeField] private float burnTime = 4f;

    [SerializeField] private KitchenItem cookedPattyPrefab;
    [SerializeField] private KitchenItem burntPattyPrefab;

    private Coroutine cookingRoutine;
    private bool isCooking;

    public override string GetInteractionText()
    {
        if (currentItem == null)
            return "Place Raw Patty";

        if (isCooking)
            return "Cooking...";

        if (currentItem.ItemType == KitchenItemType.BurgerPattyCooked)
            return "Pick Up Cooked Patty";

        if (currentItem.ItemType == KitchenItemType.BurgerPattyBurnt)
            return "Pick Up Burnt Patty";

        return "Pick Up " + currentItem.ItemName;
    }

    public override void Interact(PlayerInteraction player)
    {
        if (currentItem == null && player.IsHoldingItem)
        {
            KitchenItem heldItem = player.GetHeldItem();

            if (heldItem.ItemType == KitchenItemType.BurgerPattyRaw)
            {
                PlaceItem(player);
                cookingRoutine = StartCoroutine(CookRoutine());
                return;
            }

            if (heldItem.ItemType == KitchenItemType.BurgerPattyCooked)
            {
                PlaceItem(player);
                cookingRoutine = StartCoroutine(BurnRoutine());
                return;
            }

            return;
        }

        if (currentItem != null && player.IsHoldingItem)
        {
            if (TryCombineIntoPlayerHand(player))
            {
                if (cookingRoutine != null)
                {
                    StopCoroutine(cookingRoutine);
                    cookingRoutine = null;
                }

                isCooking = false;
            }

            return;
        }

        if (currentItem != null && !player.IsHoldingItem)
        {
            if (cookingRoutine != null)
            {
                StopCoroutine(cookingRoutine);
                cookingRoutine = null;
            }

            isCooking = false;
            PickUpItem(player);
        }
    }

    private IEnumerator CookRoutine()
    {
        isCooking = true;

        yield return new WaitForSeconds(cookTime);

        ReplaceCurrentItem(cookedPattyPrefab);

        isCooking = false;

        yield return new WaitForSeconds(burnTime);

        if (currentItem != null &&
            currentItem.ItemType == KitchenItemType.BurgerPattyCooked)
        {
            ReplaceCurrentItem(burntPattyPrefab);
        }

        cookingRoutine = null;
    }

    private IEnumerator BurnRoutine()
    {
        isCooking = true;

        yield return new WaitForSeconds(burnTime);

        if (currentItem != null &&
            currentItem.ItemType == KitchenItemType.BurgerPattyCooked)
        {
            ReplaceCurrentItem(burntPattyPrefab);
        }

        isCooking = false;
        cookingRoutine = null;
    }

    private void ReplaceCurrentItem(KitchenItem newPrefab)
    {
        if (currentItem != null)
            Destroy(currentItem.gameObject);

        currentItem = Instantiate(newPrefab, itemPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider col = currentItem.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }
}