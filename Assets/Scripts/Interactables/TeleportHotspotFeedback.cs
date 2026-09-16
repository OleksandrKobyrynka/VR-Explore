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

        _teleportAnchor = _interactable as TeleportationAnchor;

        if (_teleportAnchor == null)
        {
            Debug.LogError($"{name}: TeleportationAnchor not found.", this);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_teleportAnchor != null)
        {
            _teleportAnchor.teleporting.AddListener(OnTeleporting);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_teleportAnchor != null)
        {
            _teleportAnchor.teleporting.RemoveListener(OnTeleporting);
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (_teleportStarted)
        {
            return;
        }

        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        _teleportStarted = false;
    }

    private void OnTeleporting(TeleportingEventArgs args)
    {
        if (_teleportStarted)
        {
            return;
        }

        _teleportStarted = true;
        _isHovered = false;

        SetMaterial(_idleMaterial);
        PlaySound(_teleportSound, false);
    }
}