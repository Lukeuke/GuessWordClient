using System.Threading;
using Dtos.Finish;
using Dtos.Initialize;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager
{
    private const string URL = "http://localhost:8000";

    public (bool success, ResponseFinishDto) SendFinishRequest(RequestFinishDto requestFinishDto)
    {
        var jsonString = JsonUtility.ToJson(requestFinishDto);
        var www = UnityWebRequest.Put(URL + "/finish", jsonString);
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
    
    public (bool success, ResponseInitializeDto) SendInitializeRequest(RequestInitializeDto initializeRequestDTO)
    {
        var jsonString = JsonUtility.ToJson(initializeRequestDTO);
        var www = UnityWebRequest.Put(URL + "/initialize", jsonString);
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
    
    
}
