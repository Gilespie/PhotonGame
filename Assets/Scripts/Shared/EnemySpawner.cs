using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] float _spawnInterval = 0.3f;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] int _enemyMaxCount = 50;
    [Networked] TickTimer _tickTimer { get; set; }
    readonly List<Player> _players = new List<Player>();
    int _randomPlayerIndex;
    int _randomSpawnPosIndex;
    int _enemyCount;

    public override void Spawned()
    {
        _tickTimer = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (Runner.SessionInfo.PlayerCount < GameManager.Instance.MinPlayersToStart) return;

        if (_tickTimer.Expired(Runner) && _enemyCount < _enemyMaxCount)
        {
            Spawn();
            _enemyCount++;
            _tickTimer = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
        }
    }

    private void Spawn()
    {
        _players.Clear();

        var playerRefs = Runner.ActivePlayers;

        foreach (var playerRef in playerRefs)
        {
            var obj = Runner.GetPlayerObject(playerRef);

            if (obj == null) continue;

            var player = obj.GetComponent<Player>();

            if (player != null)
                _players.Add(player);
        }

        int playersCount = _players.Count;

        if (playersCount == 0) return;
        if (_spawnPoints == null || _spawnPoints.Length == 0) return;

        _randomPlayerIndex = Random.Range(0, playersCount);
        _randomSpawnPosIndex = Random.Range(0, _spawnPoints.Length);

        NetworkObject enemyObj = Runner.Spawn(_enemyPrefab, _spawnPoints[_randomSpawnPosIndex].position, Quaternion.identity);
        var enemy = enemyObj.GetComponent<Enemy>();

        enemy.SetTarget(_players[_randomPlayerIndex]);
        enemy.OnEnemyDead += () => _enemyCount--;
    }
}