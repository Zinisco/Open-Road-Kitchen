using UnityEngine;

public class Fridge : MonoBehaviour, IInteractable
{
    [SerializeField] private KitchenItem burgerPattyPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Accepted Items")]
    [SerializeField] private string acceptedItemName = "Burger Patty";

    public string GetInteractionText()
    {
        return "Use Fridge";
    }

    public void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingItem)
        {
            TakeBurgerPatty(player);
            return;
        }

        KitchenItem heldItem = player.GetHeldItem();

        if (heldItem.ItemName == acceptedItemName)
        {
            PutItemBack(player);
        }
    }

    private void TakeBurgerPatty(PlayerInteraction player)
    {
        KitchenItem patty = Instantiate(
            burgerPattyPrefab,
            spawnPoint.position,
            Quaternion.identity);

        player.SetHeldItem(patty);
    }

    private void PutItemBack(PlayerInteraction player)
    {
        KitchenItem item = player.RemoveHeldItem();

        if (item == null)
            return;

        Destroy(item.gameObject);
    }
}