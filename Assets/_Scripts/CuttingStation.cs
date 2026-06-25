using UnityEngine;

public class CuttingStation : KitchenSurface, ISecondaryInteractable
{
    [Header("Cutting")]
    [SerializeField] private int cutsRequired = 3;
    [SerializeField] private KitchenItem cheeseSlicesPrefab;

    private int currentCuts;

    public override string GetInteractionText()
    {
        if (currentItem == null)
            return "Place Ingredient";

        if (currentItem.ItemType == KitchenItemType.CheeseBlock)
            return $"Cut Cheese ({currentCuts}/{cutsRequired})";

        return "Pick Up " + currentItem.ItemName;
    }

    public override void Interact(PlayerInteraction player)
    {
        if (currentItem == null && player.IsHoldingItem)
        {
            KitchenItem heldItem = player.GetHeldItem();

            if (heldItem.ItemType != KitchenItemType.CheeseBlock)
                return;

            PlaceItem(player);
            currentCuts = 0;
            return;
        }

        if (currentItem != null && !player.IsHoldingItem)
        {
            PickUpItem(player);
        }
    }

    public void SecondaryInteract(PlayerInteraction player)
    {
        if (currentItem == null)
            return;

        if (currentItem.ItemType != KitchenItemType.CheeseBlock)
            return;

        currentCuts++;

        if (currentCuts >= cutsRequired)
        {
            ReplaceCurrentItem(cheeseSlicesPrefab);
            currentCuts = 0;
        }
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