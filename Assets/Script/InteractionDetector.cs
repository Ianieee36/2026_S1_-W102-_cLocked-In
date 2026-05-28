using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    private IInteractable activeLocker = null; // Track locker player is hiding in
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Interact pressed");
            if (PlayerHiding.Instance != null && PlayerHiding.Instance.IsHiding())
            {
                // Exit locker
                activeLocker?.Interact();
                activeLocker = null;
            }
            else if (interactableInRange != null)
            {
                // Check if its a locker so we can track it
                if (interactableInRange is Locker)
                    activeLocker = interactableInRange;

                interactableInRange.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    IInteractable interactable =
        collision.GetComponentInParent<IInteractable>();

    if (interactable != null && interactable.CanInteract())
    {
        interactableInRange = interactable;
        interactionIcon.SetActive(true);
    }
}

    private void OnTriggerExit2D(Collider2D collision)
{
    IInteractable interactable =
        collision.GetComponentInParent<IInteractable>();

    if (interactable != null && interactable == interactableInRange)
    {
        interactableInRange = null;
        interactionIcon.SetActive(false);
    }
}
}