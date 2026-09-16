using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public abstract class BaseInteractableFeedback : MonoBehaviour
{
    [Header("Base Visuals")]
    [SerializeField] protected Renderer _targetRenderer;
    [SerializeField] protected Material _idleMaterial;
    [SerializeField] protected Material _hoverMaterial;

    [Header("Base Audio")]
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _hoverSound;
    [SerializeField] protected bool _loopHoverSound = false;

    protected XRBaseInteractable _interactable;
    protected bool _isHovered;

    protected virtual void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        SetMaterial(_idleMaterial);

        if (_audioSource != null)
        {
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }
    }

    protected virtual void OnEnable()
    {
        if (_interactable == null)
        {
            return;
        }

        _interactable.hoverEntered.AddListener(OnHoverEntered);
        _interactable.hoverExited.AddListener(OnHoverExited);
    }

    protected virtual void OnDisable()
    {
        if (_interactable != null)
        {
            _interactable.hoverEntered.RemoveListener(OnHoverEntered);
            _interactable.hoverExited.RemoveListener(OnHoverExited);
        }

        _isHovered = false;
        SetMaterial(_idleMaterial);
        StopAudio();
    }

    protected virtual void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (_isHovered)
        {
            return;
        }

        _isHovered = true;
        SetMaterial(_hoverMaterial);
        PlaySound(_hoverSound, _loopHoverSound);
    }

    protected virtual void OnHoverExited(HoverExitEventArgs args)
    {
        if (!_isHovered)
        {
            return;
        }

        _isHovered = false;
        SetMaterial(_idleMaterial);
        StopAudio();
    }

    protected void PlaySound(AudioClip clip, bool loop = false)
    {
        if (_audioSource == null || clip == null)
        {
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    protected void StopAudio()
    {
        if (_audioSource == null)
        {
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = null;
        _audioSource.loop = false;
    }

    protected void SetMaterial(Material material)
    {
        if (_targetRenderer == null || material == null)
        {
            return;
        }

        _targetRenderer.sharedMaterial = material;
    }
}