using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _winImage;
    [SerializeField] private GameObject _loseImage;

    [Header("Waiting Room")]
    [SerializeField] private GameObject _waitingPanel;
    [SerializeField] private int _minPlayersToStart = 2;
    public int MinPlayersToStart => _minPlayersToStart;

    [SerializeField] Button _rtmButton1;
    [SerializeField] Button _rtmButton2;
    [SerializeField] Button _rtmButton3;

    private List<PlayerRef> _players;
    private readonly HashSet<PlayerRef> _playersAtFinish = new HashSet<PlayerRef>();
    private bool _gameEnded;

    private void Awake()
    {
        Instance = this;

        _players = new List<PlayerRef>();

        _rtmButton1.onClick.AddListener(ReturnToMenu);
        _rtmButton2.onClick.AddListener(ReturnToMenu);
        _rtmButton3.onClick.AddListener(ReturnToMenu);
    }

    public override void Render()
    {
        bool waiting = Runner.SessionInfo.PlayerCount < _minPlayersToStart;
        _waitingPanel.SetActive(waiting);
    }


    public void AddToList(Player player)
    {
        var playerRef = player.Object.InputAuthority;

        if (!_players.Contains(playerRef))
        {
            _players.Add(playerRef);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Defeat(PlayerRef loser)
    {
        if (_gameEnded) return;
        _gameEnded = true;

        ShowDefeatPanel();
    }

    public void PlayerEnteredFinish(PlayerRef player)
    {
        if (!HasStateAuthority) return;
        if (_gameEnded) return;

        _playersAtFinish.Add(player);
        CheckFinishCondition();
    }

    public void PlayerExitedFinish(PlayerRef player)
    {
        if (!HasStateAuthority) return;

        _playersAtFinish.Remove(player);
    }

    void CheckFinishCondition()
    {
        if (_players.Count == 0) return; 

        foreach (var p in _players)
        {
            if (!_playersAtFinish.Contains(p)) return;
        }

        RPC_WinAll();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_WinAll()
    {
        if (_gameEnded) return;
        _gameEnded = true;

        ShowWinPanel();
    }

    void ShowWinPanel()
    {
        _winImage.SetActive(true);
    }

    void ShowDefeatPanel()
    {
        _loseImage.SetActive(true);
    }

    async void ReturnToMenu()
    {
        _rtmButton1.interactable = false;
        _rtmButton2.interactable = false;

        var runner = Runner;

        if (runner != null)
            await runner.Shutdown();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}