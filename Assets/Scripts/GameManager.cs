using System.Collections.Generic;
using Dtos.Finish;
using Dtos.Initialize;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager GetInstance()
    {
        return _instance;
    }

    [SerializeField] private List<string> strList = new List<string>(5);
    [SerializeField] private string word;
    [SerializeField] private Text wordText;
    [SerializeField] private Text bonusText;
    [SerializeField] private int bonus;
    [SerializeField] private bool won;
    [SerializeField] private int id;
    [SerializeField] private int wonCount;
    [SerializeField] private int totalMoneyWon;
    [SerializeField] private bool canPlay;

    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject loginScreen;

    [SerializeField] private AuthorizeManager _authorizeManager;
    
    [SerializeField] private RequestFinishDto requestFinishDto = new RequestFinishDto();

    private readonly HttpManager _httpManager = new HttpManager();
    
    private void Update()
    {
        if (!gameCanvas.activeSelf) return;
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if(strList.Count > 0)
                strList.RemoveAt(strList.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            word = string.Join("", strList).ToLower();
            requestFinishDto.word = word;
            SendFinishRequest();
            bonusText.text = $"Won amount: {bonus}";
            
            strList = new List<string>(5);
        }

        wordText.text = string.Join("", strList).ToLower();
    }
    
    private void SendFinishRequest()
    { 
        var finishDto = new RequestFinishDto()
        {
            id = this.id,
            word = this.word
        };
        var (success, responseFinishDto) = _httpManager.SendFinishRequest(finishDto, _authorizeManager);
        
        if (success)
        {
            bonus = responseFinishDto.bonus;
            won = responseFinishDto.won;
            wonCount = responseFinishDto.wonCount;
            totalMoneyWon = responseFinishDto.totalMoneyWon;
        }
        else
        {
            won = false;
            bonus = 0;
        }
    }

    public void InitializeGame()
    {
        var initializeRequestDto = new RequestInitializeDto
        {
            id = id
        };
        var (success, initializeResponseDto) =
            _httpManager.SendInitializeRequest(initializeRequestDto, _authorizeManager);
        if (success)
        {
            id = initializeResponseDto.id;
            wonCount = initializeResponseDto.wonCount;
            totalMoneyWon = initializeResponseDto.totalMoneyWon;
            canPlay = initializeResponseDto.canPlay;
            
            loginScreen.SetActive(false);
            gameCanvas.SetActive(true);
        }
        else
        {
            print("Wrong response from Server");
        }
    }

    /// <summary>
    /// Gets current key pressed on keyboard
    /// </summary>
    private void OnGUI()
    {
        if (!gameCanvas.activeSelf) return;
        
        var e = Event.current;
        
        if (e.type != EventType.KeyDown || e.keyCode.ToString().Length != 1 ||
            !char.IsLetter(e.keyCode.ToString()[0])) return;

        if (strList.Count < 5)
        {
            strList.Add(e.keyCode.ToString());
        }
    }

    #region Getters

    public int GetId()
    {
        return id;
    }

    public string GetWord()
    {
        return word;
    }

    #endregion

    /// <summary>
    /// Singleton implementation
    /// </summary>
    private void Awake()
    {
        id = Random.Range(0, 1000);

        requestFinishDto.id = id;

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
