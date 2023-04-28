
using System.Text;
using System.Text.Json;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Services;

namespace Com2usEduProject.Middleware;

public class CheckUserAuth
{
    readonly IMemoryDb _memoryDb;
    readonly RequestDelegate _next;

    public CheckUserAuth(RequestDelegate next, IMemoryDb memoryDb)
    {
        _memoryDb = memoryDb;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var formString = context.Request.Path.Value;
        if (string.Compare(formString, "/Login", StringComparison.OrdinalIgnoreCase) == 0 ||
            string.Compare(formString, "/CreateAccount", StringComparison.OrdinalIgnoreCase) == 0)
        {
            await _next(context);

            return;
        }
        
        context.Request.EnableBuffering();

        string authToken;
        string id;
        string clientAppVersion;
        string masterDataVersion;
        
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 4096, true))
        {
            var bodyStr = await reader.ReadToEndAsync();

            // body String에 어떤 문자열도 없을때
            if (await IsNullBodyDataThenSendError(context, bodyStr))
            {
                return;
            }
            
            var document = JsonDocument.Parse(bodyStr);
            
            // id와 authToken이 존재하는지 검증
            if (IsInvalidJsonFormatThenSendError(context, document, out id, out authToken, out clientAppVersion, out masterDataVersion))
            {
                return;
            }

            // 마스터 데이터 버전 확인
            if (await IsInvalidMasterDataVersionThenSendError(context, masterDataVersion))
            {
                return;
            }
            
            // 클라이언트 데이터 버전 확인
            if (await IsInvalidClientVersionThenSendError(context, clientAppVersion))
            {
                return;
            }

            //redis에서 AuthUser 획득
            var (isOk, userInfo) = await _memoryDb.GetUserAsync(id);
            if (isOk != ErrorCode.None)
            {
                return;
            }

            // 사용자가 보낸 authToken과 저장된 authToken비교
            if (await IsInvalidUserAuthTokenThenSendError(context, userInfo, authToken))
            {
                return;
            }

            // 트랜잭션 설정
            if (await SetLockAndIsFailThenSendError(context, id))
            {
                return;
            }

            context.Items[nameof(AuthUser)] = userInfo;            
        }

        context.Request.Body.Position = 0;

        // Call the next delegate/middleware in the pipeline
        await _next(context);

        // 트랜잭션 해제(Redis 동기화 해제)
        await _memoryDb.DelUserReqLockAsync(id);
    }

    private async Task<bool> SetLockAndIsFailThenSendError(HttpContext context, string id)
    {
        if (await _memoryDb.SetUserReqLockAsync(id))
        {
            return false;
        }
        
        
        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.AuthTokenFailSetNx
        });
        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }

    private static async Task<bool> IsInvalidUserAuthTokenThenSendError(HttpContext context, AuthUser userInfo, string authToken)
    {
        if (string.CompareOrdinal(userInfo.AuthToken, authToken) == 0)
        {
            return false;
        }
        

        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.AuthTokenFailWrongAuthToken
        });
        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);

        return true;
    }
    
    async Task<bool> IsInvalidMasterDataVersionThenSendError(HttpContext context, string userVersion)
    {
        var (errorCode, masterVersion) = await _memoryDb.GetLatestMasterDataVersionAsync();

        string errorJsonResponse;
        byte[] bytes;
        
        if (errorCode != ErrorCode.None)
        {
            errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
            {
                result = errorCode
            });
            bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            return true;
        }
        
        if (string.CompareOrdinal(masterVersion, userVersion) == 0)
        {
            return false;
        }
        
        errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.InvalidMasterDataVersion
        });
        bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }
    
    async Task<bool> IsInvalidClientVersionThenSendError(HttpContext context, string userVersion)
    {
        var (errorCode, clientVersion) = await _memoryDb.GetLatestClientVersionAsync();

        string errorJsonResponse;
        byte[] bytes;
        
        if (errorCode != ErrorCode.None)
        {
            errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
            {
                result = errorCode
            });
            bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            return true;
        }
        
        if (string.CompareOrdinal(clientVersion, userVersion) == 0)
        {
            return false;
        }
        
        errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.InvalidClientVersion
        });
        bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }

    
    private bool IsInvalidJsonFormatThenSendError(HttpContext context, JsonDocument document, out string id, out string authToken, out string clientAppVersion, out string masterDataVersion)
    {
        try
        {
            id = document.RootElement.GetProperty("Id").GetString();
            authToken = document.RootElement.GetProperty("AuthToken").GetString();
            clientAppVersion = document.RootElement.GetProperty("clientAppVersion").GetString();
            masterDataVersion = document.RootElement.GetProperty("masterDataVersion").GetString();
            return false;
        }
        catch
        {
            id = authToken = clientAppVersion = masterDataVersion = "";

            var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
            {
                result = ErrorCode.AuthTokenFailWrongKeyword
            });

            var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
            context.Response.Body.Write(bytes, 0, bytes.Length);

            return true;
        }        
    }

    async Task<bool> IsNullBodyDataThenSendError(HttpContext context, string bodyStr)
    {
        if (string.IsNullOrEmpty(bodyStr) == false)
        {
            return false;
        }

        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.InValidRequestHttpBody
        });
        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        
        return true;
    }


    public class MiddlewareResponse
    {
        public ErrorCode result { get; set; }
    }
}
