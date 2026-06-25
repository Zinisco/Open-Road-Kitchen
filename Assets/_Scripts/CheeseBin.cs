using UnityEngine;

public class CheeseBin : MonoBehaviour, IInteractable
{
    [SerializeField] private KitchenItem cheesePrefab;
    [SerializeField] private Transform spawnPoint;

    public string GetInteractionText()
    {
        return "Take Cheese";
    }

    public void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingItem)
        {
            KitchenItem cheese = Instantiate(
                cheesePrefab,
                spawnPoint.position,
                Quaternion.identity);

            player.SetHeldItem(cheese);
        }
    }
}