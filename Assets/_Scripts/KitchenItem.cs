using UnityEngine;

public enum KitchenItemType
{
    BurgerPattyRaw,
    BurgerPattyCooked,
    BurgerPattyBurnt,

    BurgerBun,

    BurgerRaw,
    BurgerCooked,
    BurgerBurnt,

    CheeseBlock,
    CheeseSlices,

    BurgerCookedWithCheese,
}

public class KitchenItem : MonoBehaviour, IInteractable
{
    public string ItemName;
    public KitchenItemType ItemType;

    public string GetInteractionText()
    {
        return "Pick Up " + ItemName;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingItem)
            return;

        player.SetHeldItem(this);
    }
}