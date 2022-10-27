using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    private EventData _eventData;

    [SerializeField] int levelCount;
    [SerializeField] int randomLevelLowerLimit;

    GameState _gameState = GameState.Play;

    public GameState GameState
    {
        get => _gameState;
        set => _gameState = value;
    }

    public bool Playability()
    {
        return _gameState == GameState.Play;
    }

    public int Level
    {
        get
        {
            if (PlayerPrefs.GetInt("Level") == 0)
            {
                PlayerPrefs.SetInt("Level", 1);
                PlayerPrefs.SetInt("EndlessLevel", 1);
                return PlayerPrefs.GetInt("Level");
            }
            else if (PlayerPrefs.GetInt("Level") > levelCount)
            {
                return Random.Range(randomLevelLowerLimit, levelCount);
            }
            else
            {
                return PlayerPrefs.GetInt("Level", 1);
            }
        }
        set
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("EndlessLevel"));
        }
    }

    public int EndlessLevel
    {
        get
        {
            return PlayerPrefs.GetInt("EndlessLevel", 1);
        }
        set
        {
            PlayerPrefs.SetInt("EndlessLevel", value);
        }
    }


    public int Gold
    {
        get => PlayerPrefs.GetInt("Gold");
        set
        {
            PlayerPrefs.SetInt("Gold", value);
            UIController.Instance.UpdateGoldText();
        }
    }

    public int Score
    {
        get => PlayerPrefs.GetInt("Score");
        set => PlayerPrefs.SetInt("Score", value);
    }

    private void Awake()
    {
        Singleton(true);
        _eventData = Resources.Load("EventData") as EventData;
    }

    private void OnEnable()
    {
        _eventData.OnFinish += LevelFinish;
        _eventData.OnRestart += LoadScene;
    }

    private void LevelFinish()
    {
        Invoke("NextLevel", 1.5f);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(Level);
    }

    public void NextLevel()
    {
        _gameState = GameState.Play;
        EndlessLevel++;
        Level++;
        if (EndlessLevel % 3 == 0)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(Level);
        }
    }
}
