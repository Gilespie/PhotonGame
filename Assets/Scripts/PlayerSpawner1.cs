using Fusion;
using UnityEngine;

public class PlayerSpawner1 : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject _playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
