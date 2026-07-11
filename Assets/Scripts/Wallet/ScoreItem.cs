using TMPro;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    private Transform _owner;

    private const float HEAD_OFFSET = 2F;

    [SerializeField] private TextMeshProUGUI _myText;

    public ScoreItem SetOwner(Wallet owner)
    {
        _owner = owner.transform;

        return this;
    }

    public void UpdateScore(int score)
    {
        _myText.text = $"Score: {score.ToString()}";
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * HEAD_OFFSET;
    }
}