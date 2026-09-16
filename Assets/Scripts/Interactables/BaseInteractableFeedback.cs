using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public abstract class BaseInteractableFeedback : MonoBehaviour
{
    [Header("Base Visuals")]
    [SerializeField] protected Renderer TargetRenderer;
    [SerializeField] protected Color HoverColor = Color.yellow;
    [SerializeField] protected string ColorPropertyName = "_BaseColor";

    [Header("Base Audio")]
    [SerializeField] protected AudioSource AudioSource;
    [SerializeField] protected AudioClip HoverSound;
    [SerializeField] protected bool LoopHoverSound = false;

    protected XRBaseInteractable Interactable;
    protected bool IsHovered;

    [SerializeField] private int _materialIndex = 0;

    private MaterialPropertyBlock _propertyBlock;
    private int _colorPropertyId;
    private Color _originalColor;
    private bool _hasColorProperty;

    protected virtual void Awake()
    {
        Interactable = GetComponent<XRBaseInteractable>();
        _propertyBlock = new MaterialPropertyBlock();
        _colorPropertyId = Shader.PropertyToID(ColorPropertyName);

        CacheOriginalColor();
        ConfigureAudioSource();
    }

    protected virtual void OnEnable()
    {
        ResetFeedbackState();
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
        ResetFeedbackState();
    }

    protected virtual void SubscribeEvents()
    {
        if (Interactable == null)
        {
            return;
        }

        Interactable.hoverEntered.AddListener(HandleHoverEntered);
        Interactable.hoverExited.AddListener(HandleHoverExited);
    }

    protected virtual void UnsubscribeEvents()
    {
        if (Interactable != null)
        {
            Interactable.hoverEntered.RemoveListener(HandleHoverEntered);
            Interactable.hoverExited.RemoveListener(HandleHoverExited);
        }
    }

    protected virtual void HandleHoverEntered(HoverEnterEventArgs args)
    {
        if (IsHovered)
        {
            return;
        }

        IsHovered = true;
        SetHoverColor(true);
        PlaySound(HoverSound, LoopHoverSound);
    }

    protected virtual void HandleHoverExited(HoverExitEventArgs args)
    {
        if (!IsHovered)
        {
            return;
        }

        IsHovered = false;
        SetHoverColor(false);
        StopAudio();
    }

    protected void SetHoverColor(bool isHovered)
    {
        if (!_hasColorProperty || TargetRenderer == null)
        {
            return;
        }

        TargetRenderer.GetPropertyBlock(_propertyBlock, _materialIndex);
        Color targetColor = isHovered ? HoverColor : _originalColor;
        _propertyBlock.SetColor(_colorPropertyId, targetColor);
        TargetRenderer.SetPropertyBlock(_propertyBlock, _materialIndex);
    }

    protected void PlaySound(AudioClip clip, bool loop = false)
    {
        if (AudioSource == null || clip == null)
        {
            return;
        }

        AudioSource.Stop();
        AudioSource.clip = clip;
        AudioSource.loop = loop;
        AudioSource.Play();
    }

    protected void StopAudio()
    {
        if (AudioSource == null)
        {
            return;
        }

        AudioSource.Stop();
        AudioSource.clip = null;
        AudioSource.loop = false;
    }

    protected void ResetFeedbackState()
    {
        IsHovered = false;
        SetHoverColor(false);
        StopAudio();
    }

    private void CacheOriginalColor()
    {
        if (TargetRenderer == null)
        {
            Debug.LogWarning($"{name}: Target Renderer is not assigned.");
            return;
        }

        Material[] materials = TargetRenderer.sharedMaterials;

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
            Debug.LogWarning($"{name}: Material does not contain shader property {ColorPropertyName}.");
            return;
        }

        _originalColor = material.GetColor(_colorPropertyId);
        _hasColorProperty = true;
    }

    private void ConfigureAudioSource()
    {
        if (AudioSource == null)
        {
            return;
        }

        AudioSource.playOnAwake = false;
        AudioSource.loop = false;
    }
}