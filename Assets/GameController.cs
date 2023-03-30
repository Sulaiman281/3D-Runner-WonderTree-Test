using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private TMP_Text countDownTMP;

    [Header("PickUp Spawner")] [SerializeField]
    private Transform moneyParent;

    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private int totalPickUp;
    [SerializeField] private float yAxis;
    private int[] _xAxis = new[] { -2, 0, 2 };

    [Header("Money Collect")] [SerializeField]
    private TMP_Text moneyCollectTMP;

    [SerializeField] private AudioClip pickUpSfx;
    private int _moneyValue;

    [Header("GameOver")] [SerializeField] private TMP_Text finalScore;
    [SerializeField] private GameObject finishGate;

    public int MoneyPicked
    {
        set
        {
            _moneyValue = value;
            moneyCollectTMP.text = $"{_moneyValue}".PadLeft(3, '0');
            // add sound effect
            AudioSource.PlayClipAtPoint(pickUpSfx, playerTrans.position, .7f);
        }
        get => _moneyValue;
    }

    [Header("Time")] [SerializeField] private TMP_Text counterText;
    [SerializeField] private float timeCounter;
    [SerializeField] private AudioClip victorySfx;

    public bool hasGameStarted;

    private void Start()
    {
        // add a countdown
        hasGameStarted = false;
        StartCoroutine(CountDown());
        timeCounter = Random.Range(30, 50);
        counterText.text = $"Time Left: {timeCounter}".PadLeft(2, '0');
        var zAxis = timeCounter * 8.92f;
        Instantiate(finishGate, new Vector3(1.5f, 0, zAxis - 1), Quaternion.identity);
    }

    private IEnumerator CountDown()
    {
        countDownTMP.gameObject.SetActive(true);
        SpawnMoney();
        for (var i = 5; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            countDownTMP.text = $"{i}".PadLeft(2, '0');
        }

        countDownTMP.text = "GO GO!";
        StartCoroutine(GameTime());
        yield return new WaitForSeconds(2);
        countDownTMP.gameObject.SetActive(false);
    }

    private IEnumerator GameTime()
    {
        hasGameStarted = true;
        while (timeCounter > 0)
        {
            yield return new WaitForSeconds(1);
            timeCounter -= 1;
            counterText.text = $"Time Left: {timeCounter}".PadLeft(2, '0');
        }

        // end the game play sound fx.
        Debug.Log("Game Has ended");
        hasGameStarted = false;
        playerTrans.GetComponent<PlayerController>().GameOver();
        AudioSource.PlayClipAtPoint(victorySfx, playerTrans.position, .7f);
        finalScore.text = $"Your Score ({_moneyValue})";
        SaveScore();
    }

    public void GoToMenu()
    {
        instance = null;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void SaveScore()
    {
        var record = PlayerPrefs.GetString("HighScoreRecord");
        var playerName = PlayerPrefs.GetString("PlayerName");
        var rec = $"{playerName}:{_moneyValue}";
        record = $"{(string.IsNullOrEmpty(record) ? "" : $"{record},")}{rec}";
        PlayerPrefs.SetString("HighScoreRecord", record);
        PlayerPrefs.Save();
    }

    private void SpawnMoney()
    {
        for (var i = 1; i <= totalPickUp; i++)
        {
            var obj = Instantiate(moneyPrefab,
                new Vector3(_xAxis[Random.Range(0, _xAxis.Length)], yAxis, i * 15),
                Quaternion.identity);
            obj.transform.SetParent(moneyParent);
        }
    }
}