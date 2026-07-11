using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _scoreText;
    private Wallet _wallet;

    private void Start()
    {
        _wallet = NetworkPlayer.Local.GetComponent<Wallet>();

        if (_wallet != null)
        {
            _wallet.OnScoreChanged += UpdateScoreText;
            UpdateScoreText(_wallet._currentScore);
        }
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    private void OnDestroy()
    {
        if (_wallet != null)
            _wallet.OnScoreChanged -= UpdateScoreText;
    }
}