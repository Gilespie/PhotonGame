using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] float _spawnInterval = 0.3f;
    [SerializeField] GameObject _enemyPrefab;

    readonly List<Player> _players = new List<Player>();
    float _timer;
    int _randomPlayerIndex;
    int _randomSpawnPosIndex;

    void Start()
    {
        _timer = _spawnInterval;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        _timer -= Runner.DeltaTime;

        if(_timer <= 0)
        {             
            Spawn();
            _timer = _spawnInterval;
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

        NetworkObject enemy = Runner.Spawn(_enemyPrefab, _spawnPoints[_randomSpawnPosIndex].position, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetTarget(_players[_randomPlayerIndex]);
    }
}