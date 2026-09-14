using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Slider _volumeSlider;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _volumeParameterName = "MasterVolume";

    private void Awake()
    {
        _restartButton.onClick.AddListener(RestartGame);

        _volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void Start()
    {
        if (_audioMixer.GetFloat(_volumeParameterName, out float currentDb))
        {
            _volumeSlider.value = Mathf.Pow(10, currentDb / 20);
        }
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(RestartGame);
        _volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetVolume(float linearValue)
    {
        float clampedValue = Mathf.Clamp(linearValue, 0.0001f, 1f);

        float volumeDb = Mathf.Log10(clampedValue) * 20f;

        _audioMixer.SetFloat(_volumeParameterName, volumeDb);
    }
}