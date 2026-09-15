using UnityEngine;

public class CampfireManager : BaseCollectionManager
{
    [Header("Campfire Specific")]
    [SerializeField] private GameObject _pointLight;
    [SerializeField] private ParticleSystem _fireParticles;

    public override void ResetState()
    {
        base.ResetState();

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
    }

    protected override void CompleteMission()
    {
        base.CompleteMission();

        if (_pointLight != null)
        {
            _pointLight.SetActive(true);
        }

        if (_fireParticles != null)
        {
            _fireParticles.gameObject.SetActive(true);
            _fireParticles.Play();
        }
    }
}