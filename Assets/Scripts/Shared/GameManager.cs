using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _winImage;
    [SerializeField] private GameObject _loseImage;
    private List<PlayerRef> _players;   //PlayerRef sirve como identificacion de cada cliente conectado

    private void Awake()
    {
        Instance = this;

        _players = new List<PlayerRef>();
    }

    public void AddToList(Player player)
    {
        var playerRef = player.Object.InputAuthority;
        //Consigo el objecto con state autority
        //lo agrego a _players si no lo contiene.

        if (!_players.Contains(playerRef))
        {
            _players.Add(playerRef);
            Debug.Log($"[GameManager] Added {playerRef}, total: {_players.Count}");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Defeat(PlayerRef loser)
    {
        bool removed = _players.Remove(loser);

        if (loser == Runner.LocalPlayer)
            ShowDefeatPanel();

        if (!HasStateAuthority) return;

        switch (_players.Count)
        {
            case 1:
                RPC_Win(_players[0]);
                break;
            case 0:
                RPC_Draw();
                break;
        }
    }

    //[RpcTarget] El llamado del RPC va a ir dirigido a ese jugador
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Win(PlayerRef winner)
    {
        if (winner == Runner.LocalPlayer)
            ShowWinPanel();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Draw()
    {
        ShowDefeatPanel();
        //Debug.Log("[GameManager] Match ended in a draw.");
    }

    void ShowWinPanel()
    {
        _winImage.SetActive(true);
    }

    void ShowDefeatPanel()
    {
        _loseImage.SetActive(true);
    }
}