using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[RequireComponent(typeof(TeleportationAnchor))]
public class TeleportHotspotFeedback : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private Material _idleMaterial;
    [SerializeField] private Material _hoverMaterial;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _teleportSound;
    [SerializeField] private bool _loopHoverSound = false;

    private TeleportationAnchor _teleportAnchor;
    private bool _isHovered;
    private bool _teleportStarted;

    private void Awake()
    {
        _teleportAnchor = GetComponent<TeleportationAnchor>();

        SetMaterial(_idleMaterial);

        if (_audioSource != null)
        {
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }
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
        if (_teleportStarted || _isHovered)
        {
            return;
        }

        _isHovered = true;

        SetMaterial(_hoverMaterial);
        PlayHoverSound();
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (!_isHovered && !_teleportStarted)
        {
            return;
        }

        _isHovered = false;

        SetMaterial(_idleMaterial);

        if (!_teleportStarted)
        {
            StopAudio();
        }

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

        StopAudio();
        PlayTeleportSound();
    }

    private void PlayHoverSound()
    {
        if (_audioSource == null || _hoverSound == null)
        {
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = _hoverSound;
        _audioSource.loop = _loopHoverSound;
        _audioSource.Play();
    }

    private void PlayTeleportSound()
    {
        if (_audioSource == null || _teleportSound == null)
        {
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = _teleportSound;
        _audioSource.loop = false;
        _audioSource.Play();
    }

    private void StopAudio()
    {
        if (_audioSource == null)
        {
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = null;
        _audioSource.loop = false;
    }

    private void SetMaterial(Material material)
    {
        if (_targetRenderer == null || material == null)
        {
            return;
        }

        _targetRenderer.material = material;
    }
}