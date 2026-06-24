using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Throw Away Item";
    }

    public void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingItem)
            return;

        KitchenItem item = player.RemoveHeldItem();

        if (item == null)
            return;

        Destroy(item.gameObject);
    }
}