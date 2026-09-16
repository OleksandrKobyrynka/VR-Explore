using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public abstract class BaseInteractableFeedback : MonoBehaviour
{
    [Header("Base Visuals")]
    [SerializeField] protected Renderer _targetRenderer;
    [SerializeField] protected Color _hoverColor = Color.yellow;
    [SerializeField] protected string _colorPropertyName = "_BaseColor";
    [SerializeField] private int _materialIndex = 0;

    [Header("Base Audio")]
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _hoverSound;
    [SerializeField] protected bool _loopHoverSound = false;

    protected XRBaseInteractable _interactable;
    protected bool _isHovered;

    private MaterialPropertyBlock _propertyBlock;
    private int _colorPropertyId;

    private Color _originalColor;
    private bool _hasColorProperty;

    protected virtual void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();

        _propertyBlock = new MaterialPropertyBlock();
        _colorPropertyId = Shader.PropertyToID(_colorPropertyName);

        CacheOriginalColor();
        ConfigureAudioSource();
    }

    protected virtual void OnEnable()
    {
        ResetFeedbackState();

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

        ResetFeedbackState();
    }

    protected virtual void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (_isHovered)
        {
            return;
        }

        _isHovered = true;

        SetHoverColor(true);
        PlaySound(_hoverSound, _loopHoverSound);
    }

    protected virtual void OnHoverExited(HoverExitEventArgs args)
    {
        if (!_isHovered)
        {
            return;
        }

        _isHovered = false;

        SetHoverColor(false);
        StopAudio();
    }

    protected void SetHoverColor(bool isHovered)
    {
        if (!_hasColorProperty)
        {
            return;
        }

        if (_targetRenderer == null)
        {
            return;
        }

        _targetRenderer.GetPropertyBlock(_propertyBlock, _materialIndex);

        Color targetColor = isHovered ? _hoverColor : _originalColor;

        _propertyBlock.SetColor(_colorPropertyId, targetColor);

        _targetRenderer.SetPropertyBlock(_propertyBlock, _materialIndex);
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

    protected void ResetFeedbackState()
    {
        _isHovered = false;
        SetHoverColor(false);
        StopAudio();
    }

    private void CacheOriginalColor()
    {
        if (_targetRenderer == null)
        {
            Debug.LogWarning($"{name}: Target Renderer is not assigned.");
            return;
        }

        Material[] materials = _targetRenderer.sharedMaterials;

        if (_materialIndex < 0 || _materialIndex >= materials.Length)
        {
            Debug.LogWarning($"{name}: Invalid material index {_materialIndex}.");
            return;
        }

        Material material = materials[_materialIndex];

        if (material == null)
        {
            Debug.LogWarning($"{name}: Material at index {_materialIndex} is null.");
            return;
        }

        if (!material.HasProperty(_colorPropertyId))
        {
            Debug.LogWarning($"{name}: Material does not contain shader property {_colorPropertyName}.");
            return;
        }

        _originalColor = material.GetColor(_colorPropertyId);

        _hasColorProperty = true;
    }

    private void ConfigureAudioSource()
    {
        if (_audioSource == null)
        {
            return;
        }

        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
    }
}