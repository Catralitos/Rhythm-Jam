using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region SingleTon

    public static ScoreManager Instance { get; private set; }
    
    [Header("Notes SFX")]public AudioSource goodHitSfx;
    public AudioSource greatHitSfx;
    public AudioSource perfectHitSfx;
    public AudioSource missSfx;

    [Header("UI")] public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public Image multiplierBar;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI notesHitText;
    public TextMeshProUGUI longestStreakText;
    public TextMeshProUGUI perfectNotesHitText;
    public TextMeshProUGUI perfectLongestStreakText;
    public GameObject hitTextPrefab;
    public RectTransform hitTextSpawn;
    
    [Header("Score Values")] public int scorePerGoodNote = 20;
    public int scorePerGreatNote = 30;
    public int scorePerPerfectNote = 40;
    public int notesToIncreaseMultiplier = 10;
    public int multiplierIncrease = 2;

    [Header("Timing Values")] public float goodTiming = 0.075f;
    public float greatTiming = 0.05f;
    public float perfectTiming = 0.025f;

    private int _score;
    private int _scoreMultiplier = 1;
    private int _currentStreak;
    private int _currentPerfectStreak;
    private int _maxStreak;
    private int _maxPerfectStreak;
    private int _notesHit;
    private int _perfectNotesHit;
    private int _totalNotes;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    private void Start()
    {
        _score = 0;
        _scoreMultiplier = 1;
        multiplierBar.fillAmount = 0;

        UpdateText();
    }

    public void Hit(int type, bool isPlayer)
    {
        //Instance.goodHitSfx.Play();

        if (!isPlayer) return;

        _notesHit++;
        _totalNotes++;

        multiplierBar.fillAmount += 1.0f / notesToIncreaseMultiplier;
        if (_currentStreak % notesToIncreaseMultiplier == 0)
        {
            _scoreMultiplier *= multiplierIncrease;
            multiplierBar.fillAmount = 0;
        }
        /*if (type == 0)
        {
            //Debug.Log("Fine hit");
            _score += scorePerFineNote * _scoreMultiplier;
            _currentPerfectStreak = 0;
            SpawnHitText("FINE");
        }*/
        else if (type == 1)
        {
            //Debug.Log("Good hit");
            _score += scorePerGoodNote * _scoreMultiplier;
            _currentPerfectStreak = 0;
            SpawnHitText("GOOD");
            goodHitSfx.Play();
        }
        else if (type == 2)
        {
            //Debug.Log("Great hit");
            _score += scorePerGreatNote * _scoreMultiplier;
            _currentPerfectStreak = 0;
            SpawnHitText("GREAT");
            greatHitSfx.Play();
        }
        else if (type == 3)
        {
            //Debug.Log("Perfect hit");
            _score += scorePerPerfectNote * _scoreMultiplier;
            _perfectNotesHit++;
            _currentPerfectStreak++;
            if (_currentPerfectStreak > _maxPerfectStreak) _maxPerfectStreak = _currentPerfectStreak;
            perfectHitSfx.Play();
            SpawnHitText("PERFECT");
        }

        _currentStreak++;
        if (_currentStreak > _maxStreak) _maxStreak = _currentStreak;

        UpdateText();
    }

    public void Miss(bool isPlayer)
    {
        if (!isPlayer) return;

        _totalNotes++;
        _currentStreak = 0;
        _currentPerfectStreak = 0;
        _scoreMultiplier = 1;
        multiplierBar.fillAmount = 0;
        UpdateText();
        missSfx.Play();
        SpawnHitText("MISS");
    }

    private void UpdateText()
    {
        scoreText.text = _score.ToString();
        multiplierText.text = _scoreMultiplier + "X";
    }

    private void SpawnHitText(string text)
    {
        Instantiate(hitTextPrefab, hitTextSpawn.position, Quaternion.identity, hitTextSpawn);
        hitTextPrefab.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetVictoryText()
    {
        finalScoreText.text = "Total Score: " + _score;
        notesHitText.text = "Notes Hit: " + _notesHit + "/" + _totalNotes + " (" +
                            Mathf.FloorToInt(100.0f * _notesHit / _totalNotes) + "%)";
        longestStreakText.text = "Longest Streak: " + _maxStreak + " notes";
        perfectNotesHitText.text = "Perfect Notes Hit: " + _perfectNotesHit + "/" + _totalNotes + " (" +
                                   Mathf.FloorToInt(100.0f * _perfectNotesHit / _totalNotes) + "%)";
        perfectLongestStreakText.text = "Longest Perfect Streak: " + _maxPerfectStreak + " notes";
    }
}