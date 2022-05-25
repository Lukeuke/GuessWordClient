using System;
using System.Collections.Generic;
using Dtos.Finish;
using Dtos.Initialize;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get; set; }
    
    [SerializeField] private List<string> strList = new List<string>(5);
    [SerializeField] private string word;
    [SerializeField] private Text wordText;
    [SerializeField] private Text bonusText;
    [SerializeField] private int bonus;
    [SerializeField] private bool won;
    [SerializeField] private int id;

    [SerializeField] private RequestFinishDto requestFinishDto = new RequestFinishDto();

    private readonly HttpManager _httpManager = new HttpManager();
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if(strList.Count > 0)
                strList.RemoveAt(strList.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            word = string.Join("", strList).ToLower();
            requestFinishDto.finish.word = word;
            SendFinishRequest();
            bonusText.text = $"Won amount: {bonus}";
            
            strList = new List<string>(5);
        }

        wordText.text = string.Join("", strList).ToLower();
    }

    private void Send()
    {
        var json = JsonUtility.ToJson(requestFinishDto);
        print(json);
    }

    private void SendFinishRequest()
    { 
        var finishDto = new RequestFinishDto()
        {
            finish = requestFinishDto.finish
        };
        var (success, responseFinishDto) = _httpManager.SendFinishRequest(finishDto);
        
        if (success)
        {
            bonus = responseFinishDto.bonus;
            won = responseFinishDto.won;
        }
        else
        {
            won = false;
            bonus = 0;
        }
    }

    private void InitializeGame()
    {
        var initializeRequestDto = new RequestInitializeDto
        {
            id = id
        };
        var (success, initializeResponseDto) =
            _httpManager.SendInitializeRequest(initializeRequestDto);
        if (success)
        {
            print("succes");
        }
        else
        {
            print("cos zepsules");
        }
    }

    /// <summary>
    /// Gets current key pressed on keyboard
    /// </summary>
    private void OnGUI()
    {
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
        return requestFinishDto.finish.id;
    }

    public string GetWord()
    {
        return requestFinishDto.finish.word;
    }

    #endregion

    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// Singleton implementation
    /// </summary>
    private void Awake()
    {
        id = Random.Range(0, 1000);

        requestFinishDto.finish.id = id;

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
