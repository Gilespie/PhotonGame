using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef[] _playerPrefabs;
    [SerializeField] private Transform[] _spawnPoints;
    private bool _matchStarted;
    private LocalInputs _localInputs;
    private readonly Dictionary<PlayerRef, int> _assignedSpawnIndex = new Dictionary<PlayerRef, int>();
    private int _nextClientSpawnIndex = 1;

    private void RefreshSpawnPoints()
    {
        var points = FindObjectsByType<PlayerSpawnPoint>(FindObjectsSortMode.None);
        _spawnPoints = new Transform[points.Length];
        for (int i = 0; i < points.Length; i++)
            _spawnPoints[i] = points[i].transform;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) return;

        if (_matchStarted)
        {
            SpawnPlayer(runner, player, GetSpawnIndex(runner, player));
            return;
        }

        int count = runner.SessionInfo.PlayerCount;

        if (count >= GameManager.Instance.MinPlayersToStart)
        {
            _matchStarted = true;
            foreach (PlayerRef p in runner.ActivePlayers)
                SpawnPlayer(runner, p, GetSpawnIndex(runner, p));
        }
    }

    private int GetSpawnIndex(NetworkRunner runner, PlayerRef player)
    {
        if (_assignedSpawnIndex.TryGetValue(player, out var cachedIndex))
            return cachedIndex;

        int spawnIndex = player == runner.LocalPlayer ? 0 : _nextClientSpawnIndex++; 

        spawnIndex %= Mathf.Max(_spawnPoints.Length, 1);

        _assignedSpawnIndex[player] = spawnIndex;
        return spawnIndex;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!NetworkPlayer.Local) return;

        _localInputs ??= NetworkPlayer.Local.LocalInputs;

        input.Set(_localInputs.GetLocalInputs());
    }

    private void SpawnPlayer(NetworkRunner runner, PlayerRef player, int spawnIndex)
    {
        Vector3 pos = _spawnPoints is { Length: > 0 }
            ? _spawnPoints[spawnIndex].position
            : Vector3.zero;

        int skinIndex = GetSkinIndex(runner, player);

        NetworkObject playerObject = runner.Spawn(_playerPrefabs[skinIndex], pos, Quaternion.identity, player);
        runner.SetPlayerObject(player, playerObject);
    }

    private int GetSkinIndex(NetworkRunner runner, PlayerRef player)
    {
        byte[] token = runner.GetPlayerConnectionToken(player);

        if (token == null || token.Length < sizeof(int)) return 0;

        int skinIndex = BitConverter.ToInt32(token, 0);

        return Mathf.Clamp(skinIndex, 0, _playerPrefabs.Length - 1);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        runner.Shutdown();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        _assignedSpawnIndex.Remove(player);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { RefreshSpawnPoints(); }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
}