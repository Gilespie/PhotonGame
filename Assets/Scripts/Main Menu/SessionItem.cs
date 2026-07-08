using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _playerCount;
    [SerializeField] private TMP_Text _friendlyFire;
    [SerializeField] private Button _joinBtn;

    private SessionInfo _sessionInfo;

    public event Action<SessionInfo> OnJoinSession;

    private void Awake()
    {
        _joinBtn.onClick.AddListener(OnClick);
    }

    public void Initialize(SessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;

        _name.text = _sessionInfo.Name;
        _playerCount.text = $"{_sessionInfo.PlayerCount}/{_sessionInfo.MaxPlayers}";

        bool friendlyFire = _sessionInfo.Properties.TryGetValue("FriendlyFire", out var value) && (bool)value;
        _friendlyFire.text = friendlyFire ? "Friendly Fire: On" : "Friendly Fire: Off";

        _joinBtn.enabled = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
    }
    
    void OnClick()
    {
        OnJoinSession(_sessionInfo);
    }
}