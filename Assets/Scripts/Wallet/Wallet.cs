using Fusion;
using System;
using UnityEngine;

public class Wallet : NetworkBehaviour
{
    [SerializeField] int _maxScore = 9999;

    [Networked, OnChangedRender(nameof(OnScoreChangedRender))]
    public int _currentScore { get; private set; }

    public event Action<int> OnScoreChanged;

    private ScoreItem _myScoreItem;

    public override void Spawned()
    {
        _myScoreItem = WalletsHandler.Instance.AddScoreItem(this);
        _myScoreItem.UpdateScore(_currentScore);

        OnScoreChanged?.Invoke(_currentScore);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (_myScoreItem != null)
            WalletsHandler.Instance.RemoveScoreItem(_myScoreItem);
    }

    void OnScoreChangedRender()
    {
        _myScoreItem?.UpdateScore(_currentScore);
        OnScoreChanged?.Invoke(_currentScore);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddScore(int score)
    {
        if (score <= 0) return;

        _currentScore = Mathf.Min(_currentScore + score, _maxScore);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SubtractScore(int score)
    {
        if (score <= 0) return;

        _currentScore = Mathf.Max(_currentScore - score, 0);
    }
}