using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CampfireManager : MonoBehaviour, IResettable
{
    [Header("Interactables To Collect")]
    [SerializeField] private DisappearingInteractable[] _woodCollectibles;

    [Header("Campfire Elements")]
    [SerializeField] private GameObject[] _woodenBricks;

    [SerializeField] private GameObject _pointLight;
    [SerializeField] private ParticleSystem _fireParticles;

    [Header("Audio")]
    [SerializeField] private AudioClip _placeWoodSound;
    [SerializeField] private AudioClip _completeSound;

    private AudioSource _audioSource;
    private int _currentWoodCount;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        foreach (var wood in _woodCollectibles)
        {
            if (wood != null)
            {
                wood.OnCollected += AddWood;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var wood in _woodCollectibles)
        {
            if (wood != null)
            {
                wood.OnCollected -= AddWood;
            }
        }
    }

    private void Start()
    {
        GameResetManager.Instance?.Register(this);
        ResetState();
    }

    private void OnDestroy()
    {
        GameResetManager.Instance?.Unregister(this);
    }

    public void ResetState()
    {
        _currentWoodCount = 0;

        _audioSource.Stop();

        foreach (var brick in _woodenBricks)
        {
            if (brick != null)
            {
                brick.SetActive(false);
            }
        }

        if (_pointLight != null)
        {
            _pointLight.SetActive(false);
        }

        if (_fireParticles != null)
        {
            _fireParticles.Stop();
            _fireParticles.Clear();
            _fireParticles.gameObject.SetActive(false);
        }

        foreach (var wood in _woodCollectibles)
        {
            if (wood != null)
            {
                wood.ResetState();
            }
        }
    }

    public void AddWood()
    {
        if (_currentWoodCount >= _woodenBricks.Length)
        {
            return;
        }

        GameObject brick = _woodenBricks[_currentWoodCount];

        if (brick != null)
        {
            brick.SetActive(true);
        }

        if (_placeWoodSound != null)
        {
            _audioSource.PlayOneShot(_placeWoodSound);
        }

        _currentWoodCount++;

        if (_currentWoodCount == _woodenBricks.Length)
        {
            IgniteFire();
        }
    }

    private void IgniteFire()
    {
        if (_pointLight != null)
        {
            _pointLight.SetActive(true);
        }

        if (_fireParticles != null)
        {
            _fireParticles.gameObject.SetActive(true);
            _fireParticles.Play();
        }

        if (_completeSound != null)
        {
            _audioSource.PlayOneShot(_completeSound);
        }
    }
}