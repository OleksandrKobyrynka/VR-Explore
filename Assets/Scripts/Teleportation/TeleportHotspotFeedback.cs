using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportHotspotFeedback : MonoBehaviour
{
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private Color _idleColor = Color.cyan;
    [SerializeField] private Color _hoverColor = Color.yellow;

    private TeleportationAnchor _teleportAnchor;
    private GazeFeedback _gazeAudio;
    private Material _runtimeMaterial;
    private bool _isHovered;

    private void Awake()
    {
        _teleportAnchor = GetComponent<TeleportationAnchor>();
        _gazeAudio = FindFirstObjectByType<GazeFeedback>();

        if (_targetRenderer != null)
        {
            _runtimeMaterial = _targetRenderer.material;
        }

        SetVisualState(false);
    }

    private void OnEnable()
    {
        if (_teleportAnchor == null)
        {
            return;
        }

        _teleportAnchor.hoverEntered.AddListener(OnHoverEntered);
        _teleportAnchor.hoverExited.AddListener(OnHoverExited);
        _teleportAnchor.teleporting.AddListener(OnTeleporting);
    }

    private void OnDisable()
    {
        if (_teleportAnchor == null)
        {
            return;
        }

        _teleportAnchor.hoverEntered.RemoveListener(OnHoverEntered);
        _teleportAnchor.hoverExited.RemoveListener(OnHoverExited);
        _teleportAnchor.teleporting.RemoveListener(OnTeleporting);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (_isHovered)
        {
            return;
        }

        _isHovered = true;
        SetVisualState(true);

        if (_gazeAudio != null)
        {
            _gazeAudio.StartHover(gameObject);
        }
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (!_isHovered)
        {
            return;
        }

        _isHovered = false;
        SetVisualState(false);

        if (_gazeAudio != null)
        {
            _gazeAudio.StopHover(gameObject);
        }
    }

    private void OnTeleporting(TeleportingEventArgs args)
    {
        if (_gazeAudio != null)
        {
            _gazeAudio.PlayTeleport();
        }
    }

    private void SetVisualState(bool hovered)
    {
        Color color = hovered ? _hoverColor : _idleColor;

        if (_runtimeMaterial == null)
        {
            return;
        }

        if (_runtimeMaterial.HasProperty("_BaseColor"))
        {
            _runtimeMaterial.SetColor("_BaseColor", color);
        }
        else if (_runtimeMaterial.HasProperty("_Color"))
        {
            _runtimeMaterial.SetColor("_Color", color);
        }
    }
}