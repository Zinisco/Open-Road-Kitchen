using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayers;

    [Header("Holding")]
    [SerializeField] private Transform holdPoint;

    private KitchenItem heldItem;

    public bool IsHoldingItem => heldItem != null;

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckForInteractable();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentInteractable?.Interact(this);
        }
    }

    private void CheckForInteractable()
    {
        currentInteractable = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers))
        {
            currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
        }
    }

    public bool HasInteractable()
    {
        return currentInteractable != null;
    }

    public string GetCurrentInteractionText()
    {
        if (currentInteractable == null)
            return string.Empty;

        return currentInteractable.GetInteractionText();
    }

    public void SetHeldItem(KitchenItem item)
    {
        if (heldItem != null)
            return;

        heldItem = item;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;
    }

    public KitchenItem GetHeldItem()
    {
        return heldItem;
    }

    public KitchenItem RemoveHeldItem()
    {
        KitchenItem item = heldItem;

        if (item == null)
            return null;

        heldItem = null;

        item.transform.SetParent(null);

        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        return item;
    }
}