using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text errorMsg;
    [SerializeField] private TMP_InputField playerNameInput;

    [Header("HighScore")]
    [SerializeField] private GameObject highScorePrefab;
    [SerializeField] private Transform highScoreParent;

    private string[] _highScoreRecord;

    [Header("Music")]
    [SerializeField]
    private AudioSource bgMusic;

    [SerializeField] private TMP_Text audioText;

    private void Start()
    {
        LoadHighScore();
    }

    public void StartGame()
    {
        // check user has entered his name before starting the game.
        if (string.IsNullOrEmpty(playerNameInput.text))
        {
            errorMsg.gameObject.SetActive(true);
            errorMsg.text = "Kindly Enter Your Name Before Start";
            Invoke(nameof(HideErrorMsg), 2);
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", playerNameInput.text);
            PlayerPrefs.Save();
            // play game
            SceneManager.LoadScene("GameScene");
        }
    }

    public void HideErrorMsg()
    {
        errorMsg.gameObject.SetActive(false);
    }

    private void LoadHighScore()
    {
        if (!PlayerPrefs.HasKey("HighScoreRecord")) return;
        var record = PlayerPrefs.GetString("HighScoreRecord");
        Debug.Log(record);
        _highScoreRecord = record.Split(",");
        
        // load record in highscore ui
        foreach (var s in _highScoreRecord)
        {
            var highScore = s.Split(":");
            var obj = Instantiate(highScorePrefab, highScoreParent);
            obj.transform.GetChild(2).GetComponent<TMP_Text>().text = highScore[0];
            obj.transform.GetChild(3).GetComponent<TMP_Text>().text = $"{highScore[1]}".PadLeft(3, '0');
        }
    }

    // private void SaveHighScore()
    // {
    //     var record = "";
    //     foreach (var s in _highScoreRecord)
    //     {
    //         record = $"{(string.IsNullOrEmpty(record) ? "" : $"{record},")}{s}";
    //     }
    //     
    //     Debug.Log("Record: "+record);
    //     PlayerPrefs.SetString("HighScoreRecord", record);
    //     PlayerPrefs.Save();
    // }

    public void BackgroundMusicToggle()
    {
        if (bgMusic.isPlaying)
        {
            bgMusic.Stop();
            audioText.text = "Play Music";
        }
        else
        {
            bgMusic.Play();
            audioText.text = "Stop Music";
        }
    }

    public void QuitGame()
    {
        // SaveHighScore();
        Application.Quit();
    }
}
