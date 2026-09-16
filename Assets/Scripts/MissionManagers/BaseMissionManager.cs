using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseMissionManager : MonoBehaviour, IResettable
{
    [Header("Mission Elements")]
    [SerializeField] protected DistanceInteractable[] _interactables;

    [Header("Base Audio")]
    [SerializeField] protected AudioClip _progressSound;
    [SerializeField] protected AudioClip _completeSound;

    protected AudioSource _audioSource;
    protected int _currentCount;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    protected virtual void OnEnable()
    {
        foreach (var item in _interactables)
        {
            if (item != null)
            {
                item.OnPerformed += AddItem;
            }
        }
    }

    protected virtual void OnDisable()
    {
        foreach (var item in _interactables)
        {
            if (item != null)
            {
                item.OnPerformed -= AddItem;
            }
        }
    }

    protected virtual void Start()
    {
        GameResetManager.Instance?.Register(this);
        ResetState();
    }

    protected virtual void OnDestroy()
    {
        GameResetManager.Instance?.Unregister(this);
    }

    public virtual void ResetState()
    {
        _currentCount = 0;
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }

        foreach (var item in _interactables)
        {
            if (item != null)
            {
                item.ResetInteractable();
            }
        }
    }

    public virtual void AddItem()
    {
        if (_currentCount >= _interactables.Length)
        {
            return;
        }

        if (_progressSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_progressSound);
        }

        _currentCount++;

        if (_currentCount == _interactables.Length)
        {
            CompleteMission();
        }
    }

    protected virtual void CompleteMission()
    {
        if (_completeSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_completeSound);
        }
    }
}