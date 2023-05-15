using Com2usEduAPITester.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Com2usEduAPITester;

internal class HttpRequest
{
    public static int PlayerId => s_playerId;

    private static string s_masterDataVersion = "";
    private static string s_clientVersion = "";

    private static TextBox s_tbRequest = null;
    private static TextBox s_tbResponse = null;

    private static int s_accountId = -1;
    private static int s_playerId = -1;
    private static string s_authToken = "";

    private static string s_serverUrl = "";

    private static HttpClient s_httpClient = null;

    public static readonly TextEncoderSettings encoderSettings = new TextEncoderSettings();

    public class CommonResponse
    {
        public ErrorCode Result { get; set; }
    }

    public static void Init(string masterDataVersion, string clientVersion, TextBox tbRequest, TextBox tbResponse)
    {
        s_masterDataVersion = masterDataVersion;
        s_clientVersion = clientVersion;
        s_tbRequest = tbRequest;
        s_tbResponse = tbResponse;
        s_httpClient = new HttpClient();
        encoderSettings.AllowRange(UnicodeRanges.All);
    }

    public static void SetAuth(int accountId, int playerId, string authToken)
    {
        s_accountId = accountId;
        s_playerId = playerId;
        s_authToken = authToken;
    }

    public static void SetServerURL(string serverURL)
    {
        s_serverUrl = serverURL;
    }


    public static async Task<string?> Post(string apiName, string jsonString)
    {
        if (s_httpClient == null)
            throw new Exception("HttpRequest Not Initialized");
        if (s_serverUrl == "")
            throw new Exception("Server URL is Missing");

        var targetURL = s_serverUrl + "/" + apiName;

        var jsonObject = JsonNode.Parse(jsonString);

        jsonObject["MasterDataVersion"] = s_masterDataVersion;
        jsonObject["ClientVersion"] = s_clientVersion;

        jsonString = jsonObject.ToJsonString(new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(encoderSettings),
            WriteIndented = true,
        });

        s_tbResponse.Text = "";
        s_tbRequest.Text = "";
        s_tbRequest.Text += "POST " + targetURL + "\r\n";
        s_tbRequest.Text += "Content-Type: application/json\r\n";
        s_tbRequest.Text += jsonString;

        using (var response = await s_httpClient.PostAsync(s_serverUrl + "/" + apiName, new StringContent(jsonString, Encoding.UTF8, "application/json")))
        {
            if (HttpStatusCode.OK == response.StatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var prettyJsonString = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(responseString), new JsonSerializerOptions { WriteIndented = true });

                s_tbResponse.Text = prettyJsonString;

                var commonResponse = JsonSerializer.Deserialize<CommonResponse>(responseString);
                if (commonResponse.Result != ErrorCode.None)
                {
                    s_tbResponse.Text = "ErrorCode : " + commonResponse.Result.ToString();
                    return responseString;
                }

                return responseString;
            }
            else
            {
                s_tbResponse.Text = "Http Fail Status : "+ response.StatusCode.ToString();
                return null;
            }
        }


    }
    public static async Task<T?> PostAuth<T>(string apiName, object jsonBody)
    {

        var jsonString = JsonSerializer.Serialize(jsonBody);

        var jsonObject = JsonNode.Parse(jsonString);

        if (s_authToken == "" || s_playerId == -1 || s_accountId == -1)
            throw new Exception("Auth Not Initialized");

        jsonObject["AccountId"] = s_accountId;
        jsonObject["AuthToken"] = s_authToken;
        jsonObject["PlayerId"] = s_playerId;

        jsonString = jsonObject.ToJsonString(new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(encoderSettings),
            WriteIndented = true,
        });
        jsonString = await Post(apiName, jsonString);
        if (jsonString == null)
            return default(T);
        
        var response = JsonSerializer.Deserialize<T>(jsonString);
        return response;
    }

}