using UnityEngine.XR.Interaction.Toolkit;

public class DisappearingInteractable : DistanceInteractable
{
    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if (Interactable != null)
        {
            Interactable.selectEntered.AddListener(HandleSelectEntered);
        }
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        if (Interactable != null)
        {
            Interactable.selectEntered.RemoveListener(HandleSelectEntered);
        }
    }

    public override void ResetInteractable()
    {
        gameObject.SetActive(true);
    }

    protected override void HandleHoverEntered(HoverEnterEventArgs args)
    {
        if (IsInRange())
        {
            base.HandleHoverEntered(args);
        }
    }

    private void HandleSelectEntered(SelectEnterEventArgs args)
    {
        if (IsInRange())
        {
            CompleteInteraction();
        }
    }
}