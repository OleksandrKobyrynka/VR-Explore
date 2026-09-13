using UnityEngine;

public class GazeFeedback : MonoBehaviour
{
    [SerializeField] private AudioSource _hoverAudio;
    [SerializeField] private AudioSource _teleportAudio;

    private GameObject _currentHotspot;

    public void StartHover(GameObject hotspot)
    {
        if (hotspot == null)
        {
            return;
        }

        if (_currentHotspot == hotspot)
        {
            return;
        }

        StopHover();

        _currentHotspot = hotspot;

        if (_hoverAudio != null && !_hoverAudio.isPlaying)
        {
            _hoverAudio.Play();
        }
    }

    public void StopHover(GameObject hotspot)
    {
        if (_currentHotspot != hotspot)
        {
            return;
        }

        StopHover();
    }

    public void StopHover()
    {
        _currentHotspot = null;

        if (_hoverAudio != null && _hoverAudio.isPlaying)
        {
            _hoverAudio.Stop();
        }
    }

    public void PlayTeleport()
    {
        StopHover();

        if (_teleportAudio != null)
        {
            _teleportAudio.Play();
        }
    }
}
