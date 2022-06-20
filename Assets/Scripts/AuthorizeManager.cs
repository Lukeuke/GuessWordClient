using Dtos.Authorize;
using TMPro;
using UnityEngine;

public class AuthorizeManager : MonoBehaviour
{
    [SerializeField] private string token;
    
    [SerializeField] private TMP_InputField loginUsernameInput;
    [SerializeField] private TMP_InputField loginPasswordInput;
    
    [SerializeField] private TMP_InputField registerUsernameInput;
    [SerializeField] private TMP_InputField registerPasswordInput;

    private readonly HttpManager _httpManager = new HttpManager();
    
    public void SendLogin()
    {
        var username = loginUsernameInput.text;
        var password = loginPasswordInput.text;
        
        var credentials = new AuthorizeDto()
        {
            username = username,
            password = password
        };
        
        var (success, tokenDto) = _httpManager.SendLoginRequest(credentials);
        
        token = success ? tokenDto.token : string.Empty;
        
        GameManager.GetInstance().InitializeGame();
    }

    public void SendRegister()
    {
        var username = registerUsernameInput.text;
        var password = registerPasswordInput.text;

        var credentials = new AuthorizeDto()
        {
            username = username,
            password = password
        };

        var (success, tokenDto) = _httpManager.SendRegisterRequest(credentials);

        token = success ? tokenDto.token : string.Empty;
    }

    public string GetToken()
    {
        return token;
    }
}
