using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] HealthSystem _health;
    [SerializeField] LifeBarItem _lifeBarItem;

    void Start()
    {
        if(_lifeBarItem != null)
        {
            _lifeBarItem = LifeBarHandler.Instance.AddLifeBar(_health);
            _health.OnHealthUpdate += UpdateBar;
            _health.OnLeft += RemoveBar;
        }
    }

    void UpdateBar(float normalized)
    {
        _lifeBarItem?.UpdateLife(normalized);
    }

    void RemoveBar()
    {
        _health.OnHealthUpdate -= UpdateBar;
        _health.OnLeft -= RemoveBar;
    }
}