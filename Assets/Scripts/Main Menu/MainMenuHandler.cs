using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private NetworkRunnerHandler _networkRunnerHandler;

    [Header("Host Settings")]
    [SerializeField] private Toggle _friendlyFireToggle;

    [Header("Panels")]
    [SerializeField] private GameObject _initialPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _profilePanel;
    [SerializeField] private GameObject _statusPanel;
    [SerializeField] private GameObject _sessionBrowserPanel;
    [SerializeField] private GameObject _hostGamePanel;

    [Header("Buttons")]
    [SerializeField] private Button _hostPanelBTN;
    [SerializeField] private Button _hostGameBTN;
    [SerializeField] private Button _creditsBTN;
    [SerializeField] private Button _profileBTN;
    [SerializeField] private Button _saveSkinBTN1;
    [SerializeField] private Button _saveSkinBTN2;
    [SerializeField] private Button _saveNicknameBTN;
    [SerializeField] private Button _backFromCreditsBTN;
    [SerializeField] private Button _backFromProfileBTN;
    [SerializeField] private Button _playBTN;
    [SerializeField] private Button _exitBTN;

    [Header("InputFields")]
    [SerializeField] private TMP_InputField _sessionName;
    [SerializeField] private TMP_InputField _nicknameField;

    [Header("Texts")]
    [SerializeField] private TMP_Text _statusText;

    [SerializeField] int[] _skins;

    void Start()
    {
        _playBTN.onClick.AddListener(Btn_JoinLobby);
        _hostPanelBTN.onClick.AddListener(Btn_ShowHostPanel);
        _hostGameBTN.onClick.AddListener(Btn_CreateGameSession);
        _creditsBTN.onClick.AddListener(Btn_CreditsPanel);
        _backFromCreditsBTN.onClick.AddListener(Btn_CreditsBack);
        _backFromProfileBTN.onClick.AddListener(Btn_ProfileBack);
        _exitBTN.onClick.AddListener(Btn_Quit);
        _profileBTN.onClick.AddListener(Btn_ProfilePanel);

        _saveSkinBTN1.onClick.AddListener(Btn_SaveSkinPlayer);
        _saveSkinBTN2.onClick.AddListener(Btn_SaveSkinPlayer2);
        _saveNicknameBTN.onClick.AddListener(Btn_SaveNickName);

        _networkRunnerHandler.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            _sessionBrowserPanel.SetActive(true);
        };
    }

    void Btn_JoinLobby()
    {
        _networkRunnerHandler.JoinLobby();

        _initialPanel.SetActive(false);
        _statusPanel.SetActive(true);

        _statusText.text = "Joining Lobby...";
    }

    void Btn_SaveSkinPlayer()
    {
        PlayerPrefs.SetInt("PlayerSkin", _skins[0]);
        
    }

    void Btn_SaveSkinPlayer2()
    {
        PlayerPrefs.SetInt("PlayerSkin", _skins[1]);
    }

    void Btn_SaveNickName()
    {
        PlayerPrefs.SetString("Nickname", _nicknameField.text);
    }

    void Btn_ShowHostPanel()
    {
        _sessionBrowserPanel.SetActive(false);
        _hostGamePanel.SetActive(true);
    }

    void Btn_CreateGameSession()
    {
        _hostGameBTN.interactable = false;

        _networkRunnerHandler.CreateGame(_sessionName.text, "Game", _friendlyFireToggle.isOn);
    }

    void Btn_CreditsPanel()
    {
        _initialPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    void Btn_CreditsBack()
    {
        _initialPanel.SetActive(true);
        _creditsPanel.SetActive(false);
    }

    void Btn_ProfilePanel()
    {
        _initialPanel.SetActive(false);
        _profilePanel.SetActive(true);
    }

    void Btn_ProfileBack()
    {
        _initialPanel.SetActive(true);
        _profilePanel.SetActive(false);
    }

    void Btn_Quit()
    {
        Application.Quit();
    }
}