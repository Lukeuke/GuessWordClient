using System.Threading;
using Dtos.Authorize;
using Dtos.Finish;
using Dtos.Initialize;
using Dtos.JWT;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager
{
    private const string URL = "https://localhost:7071";
    private string _token = "";

    public (bool success, ResponseFinishDto) SendFinishRequest(RequestFinishDto requestFinishDto, AuthorizeManager authorizeManager)
    {
        var json = JsonUtility.ToJson(requestFinishDto);
        
        var www = new UnityWebRequest(URL + "/finish", "POST");
        
        var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        www.certificateHandler = new CertificateOverride();
        
        www.SetRequestHeader("Authorization","Bearer " + authorizeManager.GetToken());
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        Thread.Sleep(300);
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Finish response success");
            Debug.Log(www.downloadHandler.text);
            var response = JsonUtility.FromJson<ResponseFinishDto>(www.downloadHandler.text);
            return (true, response);
        }

        Debug.Log("Wrong response from Finish response");
        return (false, new ResponseFinishDto());
    }
    
    public (bool success, ResponseInitializeDto) SendInitializeRequest(RequestInitializeDto initializeRequestDto, AuthorizeManager authorizeManager)
    {
        var json = JsonUtility.ToJson(initializeRequestDto);
        
        var www = new UnityWebRequest(URL + "/initialize", "POST");
        
        var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        www.certificateHandler = new CertificateOverride();
        
        www.SetRequestHeader("Authorization","Bearer " + authorizeManager.GetToken());
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        Thread.Sleep(300);
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Initialize response success");
            Debug.Log(www.downloadHandler.text);
            var response = JsonUtility.FromJson<ResponseInitializeDto>(www.downloadHandler.text);
            return (true, response);
        }

        Debug.Log("Wrong initialize response");
        return (false, new ResponseInitializeDto());
    }

    public (bool succes, TokenDto) SendLoginRequest(AuthorizeDto authorizeDto)
    {
        var json = JsonUtility.ToJson(authorizeDto);
        
        var www = new UnityWebRequest(URL + "/authentication/login", "POST");
        
        var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        www.certificateHandler = new CertificateOverride();
        
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        Thread.Sleep(300);
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login response success");
            Debug.Log(www.downloadHandler.text);
            var response = JsonUtility.FromJson<TokenDto>(www.downloadHandler.text);
            _token = response.token;
            return (true, response);
        }

        Debug.Log("Wrong login response");
        _token = string.Empty;
        return (false, new TokenDto());
    }
    
    public (bool succes, TokenDto) SendRegisterRequest(AuthorizeDto authorizeDto)
    {
        var json = JsonUtility.ToJson(authorizeDto);
        var www = new UnityWebRequest(URL + "/authentication/register", "POST");
        
        var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        www.certificateHandler = new CertificateOverride();
        
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        Thread.Sleep(300);
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Register response success");
            Debug.Log(www.downloadHandler.text);
            var response = JsonUtility.FromJson<TokenDto>(www.downloadHandler.text);
            _token = response.token;
            return (true, response);
        }

        Debug.Log("Wrong register response");
        _token = string.Empty;
        return (false, new TokenDto());
    }
}
