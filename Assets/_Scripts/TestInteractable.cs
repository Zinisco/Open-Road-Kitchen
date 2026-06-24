using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionText = "Interact";

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact(PlayerInteraction player)
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }
}