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

    [Header("Throwing")]
    [SerializeField] private float minThrowForce = 3f;
    [SerializeField] private float maxThrowForce = 10f;
    [SerializeField] private float maxChargeTime = 1.25f;
    [SerializeField] private float throwUpwardForce = 1.2f;

    private bool isChargingThrow;
    private float throwChargeTimer;

    private KitchenItem heldItem;

    public bool IsHoldingItem => heldItem != null;

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckForInteractable();
        HandleThrowInput();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentInteractable?.Interact(this);
        }

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentInteractable is ISecondaryInteractable secondary)
            {
                secondary.SecondaryInteract(this);
            }
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

    private void HandleThrowInput()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (!IsHoldingItem)
            return;

        if (keyboard.qKey.wasPressedThisFrame)
        {
            isChargingThrow = true;
            throwChargeTimer = 0f;
        }

        if (keyboard.qKey.isPressed && isChargingThrow)
        {
            throwChargeTimer += Time.deltaTime;
            throwChargeTimer = Mathf.Clamp(throwChargeTimer, 0f, maxChargeTime);
        }

        if (keyboard.qKey.wasReleasedThisFrame && isChargingThrow)
        {
            ThrowHeldItem();
            isChargingThrow = false;
            throwChargeTimer = 0f;
        }
    }

    private void ThrowHeldItem()
    {
        KitchenItem item = heldItem;

        if (item == null)
            return;

        heldItem = null;

        item.transform.SetParent(null);

        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;

            float chargePercent = throwChargeTimer / maxChargeTime;
            float throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);

            Vector3 throwDirection = playerCamera.transform.forward;
            Vector3 force = throwDirection * throwForce + Vector3.up * throwUpwardForce;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}