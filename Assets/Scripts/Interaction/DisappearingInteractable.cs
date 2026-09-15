using UnityEngine.XR.Interaction.Toolkit;

public class DisappearingInteractable : DistanceInteractable
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if (_interactable != null)
        {
            _interactable.selectEntered.AddListener(OnSelectEntered);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_interactable != null)
        {
            _interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (IsInRange())
        {
            base.OnHoverEntered(args);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (IsInRange())
        {
            CompleteInteraction();
        }
    }

    public override void ResetInteractable()
    {
        gameObject.SetActive(true);
    }
}