using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private NetworkRunnerHandler _networkRunnerHandler;

    [Header("Panels")]
    [SerializeField] private GameObject _initialPanel;
    [SerializeField] private GameObject _joinPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _statusPanel;
    [SerializeField] private GameObject _sessionBrowserPanel;
    [SerializeField] private GameObject _hostGamePanel;

    [Header("Buttons")]
    [SerializeField] private Button _joinLobbyBTN;
    [SerializeField] private Button _hostPanelBTN;
    [SerializeField] private Button _hostGameBTN;
    [SerializeField] private Button _creditsBTN;
    [SerializeField] private Button _backBTN;
    [SerializeField] private Button _playBTN;
    [SerializeField] private Button _exitBTN;

    [Header("InputFields")]
    [SerializeField] private TMP_InputField _sessionName;
    [SerializeField] private TMP_InputField _nicknameField;

    [Header("Texts")]
    [SerializeField] private TMP_Text _statusText;

    void Start()
    {
        _joinLobbyBTN.onClick.AddListener(Btn_JoinLobby);
        _hostPanelBTN.onClick.AddListener(Btn_ShowHostPanel);
        _hostGameBTN.onClick.AddListener(Btn_CreateGameSession);
        _creditsBTN.onClick.AddListener(Btn_CreditsPanel);
        _backBTN.onClick.AddListener(Btn_Back);
        _playBTN.onClick.AddListener(Btn_Play);
        _exitBTN.onClick.AddListener(Btn_Quit);

        _networkRunnerHandler.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            _sessionBrowserPanel.SetActive(true);
        };
    }

    void Btn_JoinLobby()
    {
        _networkRunnerHandler.JoinLobby();

        PlayerPrefs.SetString("Nickname", _nicknameField.text);

        _initialPanel.SetActive(false);
        _statusPanel.SetActive(true);

        _statusText.text = "Joining Lobby...";
    }

    void Btn_ShowHostPanel()
    {
        _sessionBrowserPanel.SetActive(false);
        _hostGamePanel.SetActive(true);
    }

    void Btn_CreateGameSession()
    {
        _hostGameBTN.interactable = false;

        _networkRunnerHandler.CreateGame(_sessionName.text, "Game");
    }

    void Btn_CreditsPanel()
    {
        _initialPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    void Btn_Back()
    {
        _initialPanel.SetActive(true);
        _creditsPanel.SetActive(false);
    }

    void Btn_Play()
    {
        _initialPanel.SetActive(false);
        _joinPanel.SetActive(true);
    }

    void Btn_Quit()
    {
        Application.Quit();
    }
}