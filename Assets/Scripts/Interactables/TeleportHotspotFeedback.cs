using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[RequireComponent(typeof(TeleportationAnchor))]
public class TeleportHotspotFeedback : BaseInteractableFeedback
{
    [Header("Teleport Specific")]
    [SerializeField] private AudioClip _teleportSound;

    private TeleportationAnchor _teleportAnchor;
    private bool _teleportStarted;

    protected override void Awake()
    {
        base.Awake();

        _teleportAnchor = Interactable as TeleportationAnchor;

        if (_teleportAnchor == null)
        {
            Debug.LogError($"{name}: TeleportationAnchor not found.", this);
        }
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if (_teleportAnchor != null)
        {
            _teleportAnchor.teleporting.AddListener(HandleTeleporting);
        }
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        if (_teleportAnchor != null)
        {
            _teleportAnchor.teleporting.RemoveListener(HandleTeleporting);
        }
    }

    protected override void HandleHoverEntered(HoverEnterEventArgs args)
    {
        if (_teleportStarted)
        {
            return;
        }

        base.HandleHoverEntered(args);
    }

    protected override void HandleHoverExited(HoverExitEventArgs args)
    {
        base.HandleHoverExited(args);
        _teleportStarted = false;
    }

    private void HandleTeleporting(TeleportingEventArgs args)
    {
        if (_teleportStarted)
        {
            return;
        }

        _teleportStarted = true;
        IsHovered = false;

        SetHoverColor(false);
        PlaySound(_teleportSound, false);
    }
}