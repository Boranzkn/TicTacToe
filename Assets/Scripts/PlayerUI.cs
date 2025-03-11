using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject crossArrow;
    [SerializeField] private GameObject circleArrow;
    [SerializeField] private GameObject crossYouText;
    [SerializeField] private GameObject circleYouText;
    [SerializeField] private TextMeshProUGUI playerCrossScoreText;
    [SerializeField] private TextMeshProUGUI playerCircleScoreText;

    private void Awake()
    {
        crossArrow.SetActive(false);
        circleArrow.SetActive(false);
        crossYouText.SetActive(false);
        circleYouText.SetActive(false);

        playerCrossScoreText.text = "";
        playerCircleScoreText.text = "";
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnCurrentPlayablePlayerTypeChanged += GameManager_OnCurrentPlayablePlayerTypeChanged;
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
    }

    private void GameManager_OnScoreChanged(object sender, System.EventArgs e)
    {
        GameManager.Instance.GetScores(out int playerCrossScore, out int playerCircleScore);

        playerCrossScoreText.text = playerCrossScore.ToString();
        playerCircleScoreText.text = playerCircleScore.ToString();
    }

    private void GameManager_OnCurrentPlayablePlayerTypeChanged(object sender, System.EventArgs e)
    {
        UpdateCurrentArrow();
    }

    private void GameManager_OnGameStarted(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.GetLocalPlayerType() == GameManager.PlayerType.Cross)
        {
            crossYouText.SetActive(true);
        }
        else
        {
            circleYouText.SetActive(true);
        }

        playerCrossScoreText.text = "0";
        playerCircleScoreText.text = "0";

        UpdateCurrentArrow();
    }

    private void UpdateCurrentArrow()
    {
        if (GameManager.Instance.GetCurrentPlayablePlayerType() == GameManager.PlayerType.Cross)
        {
            crossArrow.SetActive(true);
            circleArrow.SetActive(false);
        }
        else
        {
            crossArrow.SetActive(false);
            circleArrow.SetActive(true);
        }
    }
}
