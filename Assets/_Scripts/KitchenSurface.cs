using UnityEngine;

public class KitchenSurface : MonoBehaviour, IInteractable
{
    [Header("Surface")]
    [SerializeField] protected Transform itemPoint;

    [SerializeField] private BurgerCombiner burgerCombiner;

    protected KitchenItem currentItem;

    public bool HasItem => currentItem != null;

    public virtual string GetInteractionText()
    {
        if (currentItem == null)
            return "Place Item";

        return "Pick Up " + currentItem.ItemName;
    }

    public virtual void Interact(PlayerInteraction player)
    {
        if (currentItem == null && player.IsHoldingItem)
        {
            PlaceItem(player);
            return;
        }

        if (currentItem != null && player.IsHoldingItem)
        {
            TryCombineHeldItem(player);
            return;
        }

        if (currentItem != null && !player.IsHoldingItem)
        {
            PickUpItem(player);
        }
    }

    protected virtual void PlaceItem(PlayerInteraction player)
    {
        KitchenItem item = player.RemoveHeldItem();

        if (item == null)
            return;

        currentItem = item;

        item.transform.SetParent(itemPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    protected virtual void PickUpItem(PlayerInteraction player)
    {
        KitchenItem item = currentItem;
        currentItem = null;

        player.SetHeldItem(item);
    }

    private void TryCombineHeldItem(PlayerInteraction player)
    {
        if (burgerCombiner == null)
            return;

        KitchenItem heldItem = player.GetHeldItem();

        if (heldItem == null)
            return;

        bool combined = burgerCombiner.TryCombine(
            currentItem,
            heldItem,
            itemPoint,
            out KitchenItem result);

        if (!combined)
            return;

        player.RemoveHeldItem();
        currentItem = result;
    }

    protected bool TryCombineIntoPlayerHand(PlayerInteraction player)
    {
        if (burgerCombiner == null)
            return false;

        KitchenItem heldItem = player.GetHeldItem();

        if (heldItem == null || currentItem == null)
            return false;

        KitchenItem removedHeldItem = player.RemoveHeldItem();

        bool combined = burgerCombiner.TryCombine(
            currentItem,
            removedHeldItem,
            null,
            out KitchenItem result);

        if (!combined)
        {
            player.SetHeldItem(removedHeldItem);
            return false;
        }

        currentItem = null;

        player.SetHeldItem(result);
        return true;
    }
}