using Fusion;
using UnityEngine;

public class LightFlickering : NetworkBehaviour
{
    [SerializeField] AnimationCurve _intensityCurve;
    [SerializeField] float _flickerSpeed = 1f;
    [SerializeField] float _maxIntensity = 5f;
    [SerializeField] Light _light;
    float _time;

    public override void FixedUpdateNetwork()
    {
        _time += Runner.DeltaTime * _flickerSpeed;
        _light.intensity = _intensityCurve.Evaluate(_time) * _maxIntensity;
    }
}