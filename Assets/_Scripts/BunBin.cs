using UnityEngine;

public class BunBin : MonoBehaviour, IInteractable
{
    [SerializeField] private KitchenItem bunPrefab;
    [SerializeField] private Transform spawnPoint;

    public string GetInteractionText()
    {
        return "Take Bun";
    }

    public void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingItem)
        {
            KitchenItem bun = Instantiate(
                bunPrefab,
                spawnPoint.position,
                Quaternion.identity);

            player.SetHeldItem(bun);
            return;
        }

        KitchenItem heldItem = player.GetHeldItem();

        if (heldItem.ItemType == KitchenItemType.BurgerBun)
        {
            Destroy(player.RemoveHeldItem().gameObject);
        }
    }
}