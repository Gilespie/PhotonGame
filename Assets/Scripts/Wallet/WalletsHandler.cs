using System;
using System.Collections.Generic;
using UnityEngine;

public class WalletsHandler : MonoBehaviour
{
    public static WalletsHandler Instance { get; private set; }

    [SerializeField] private ScoreItem _scoreItemPrefab;

    private readonly List<ScoreItem> _scoreItems = new List<ScoreItem>();

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public ScoreItem AddScoreItem(Wallet owner)
    {
        var scoreItem = Instantiate(_scoreItemPrefab, transform)
            .SetOwner(owner);

        _scoreItems.Add(scoreItem);

        return scoreItem;
    }

    public void RemoveScoreItem(ScoreItem scoreItem)
    {
        if (_scoreItems.Remove(scoreItem))
            Destroy(scoreItem.gameObject);
    }

    void LateUpdate()
    {
        foreach (var scoreItem in _scoreItems)
        {
            scoreItem.UpdatePosition();
        }
    }
}