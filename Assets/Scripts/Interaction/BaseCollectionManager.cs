using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseCollectionManager : MonoBehaviour, IResettable
{
    [Header("Base Collection Elements")]
    [SerializeField] protected DistanceInteractable[] _interactables;
    [SerializeField] protected GameObject[] _visualObjects;

    [Header("Base Audio")]
    [SerializeField] protected AudioClip _placeSound;
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

        foreach (var obj in _visualObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
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
        if (_currentCount >= _visualObjects.Length)
        {
            return;
        }

        _visualObjects[_currentCount]?.SetActive(true);

        if (_placeSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_placeSound);
        }

        _currentCount++;

        if (_currentCount == _visualObjects.Length)
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