using System.Text;
using System.Text.Json;
using Com2usEduProject.Authorization;
using Com2usEduProject.Databases;

namespace Com2usEduProject.Middlewares;

public class CheckUserAuth
{
    readonly IMemoryDb _memoryDb;
    readonly IMasterDb _masterDb;
    readonly RequestDelegate _next;

    public CheckUserAuth(RequestDelegate next, IMemoryDb memoryDb, IMasterDb masterDb)
    {
        _masterDb = masterDb;
        _memoryDb = memoryDb;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var formString = context.Request.Path.Value;
        if (string.Compare(formString, "/CreateAccount", StringComparison.OrdinalIgnoreCase) == 0)
        {
            await _next(context);
            return;
        }
        
        context.Request.EnableBuffering();

        int accountId;
        int playerId;
        string authToken;

        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 4096, true))
        {
            var bodyStr = await reader.ReadToEndAsync();

            // body String에 어떤 문자열도 없을때
            if (await IsNullBodyDataThenSendError(context, bodyStr))
            {
                return;
            }
            
            var document = JsonDocument.Parse(bodyStr);
            
            // 마스터 데이터 버전 확인
            if (await IsInvalidMasterDataVersionThenSendError(context, document))
            {
                return;
            }
            
            // 클라이언트 데이터 버전 확인
            if (await IsInvalidClientVersionThenSendError(context, document))
            {
                return;
            }
            
            if (string.Compare(formString, "/Login", StringComparison.OrdinalIgnoreCase) == 0)
            {
                context.Request.Body.Position = 0;
                await _next(context);
                return;
            }
                        
            // id와 authToken이 존재하는지 검증
            if (IsInvalidJsonFormatThenSendError(context, document, out accountId, out authToken, out playerId))
            {
                return;
            }

            //redis에서 AuthUser 획득
            var (isOk, userInfo) = await _memoryDb.AuthManager.GetUserAsync(accountId);
            if (isOk != ErrorCode.None)
            {
                return;
            }

            // 사용자가 보낸 authToken과 저장된 authToken비교
            if (await IsInvalidUserAuthTokenThenSendError(context, userInfo, authToken, playerId))
            {
                return;
            }

            // 트랜잭션 설정
            if (await SetLockAndIsFailThenSendError(context, accountId))
            {
                return;
            }

            context.Items[nameof(AuthUser)] = userInfo;            
        }

        context.Request.Body.Position = 0;

        await _next(context);

        // 트랜잭션 해제(Redis 동기화 해제)
        await _memoryDb.AuthManager.DelUserRequestLockAsync(accountId.ToString());
    }

    async Task<bool> SetLockAndIsFailThenSendError(HttpContext context, int accountId)
    {
        if (await _memoryDb.AuthManager.SetUserRequestLockAsync(accountId.ToString()))
        {
            return false;
        }
        
        
        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.AuthTokenFailSetLock
        });
        var bytes = Encoding.UTF8.GetBytes(errorJsonResponse);
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        return true;
    }

    async Task<bool> IsInvalidUserAuthTokenThenSendError(HttpContext context, AuthUser userInfo, string authToken, int playerId)
    {
        if (string.CompareOrdinal(userInfo.AuthToken, authToken) == 0 && userInfo.PlayerId == playerId)
        {
            return false;
        }

        var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.AuthTokenFailWrongAuthToken
        });
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));

        return true;
    }
    
    async Task<bool> IsInvalidMasterDataVersionThenSendError(HttpContext context, JsonDocument document)
    {
        string masterDataVersion;
        string errorJsonResponse;
        
        try
        {
            masterDataVersion = document.RootElement.GetProperty("MasterDataVersion").GetString();
        }
        catch
        {
            errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
            {
                result = ErrorCode.InValidRequestHttpBody
            });
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));

            return true;
        }
        
        if (string.CompareOrdinal(_masterDb.GetVersion(), masterDataVersion) == 0)
        {
            return false;
        }
        
        errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.InvalidMasterDataVersion
        });
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));

        return true;
    }
    
    async Task<bool> IsInvalidClientVersionThenSendError(HttpContext context, JsonDocument document)
    {

        string clientAppVersion;
        string errorJsonResponse;
        
        try
        {
            clientAppVersion = document.RootElement.GetProperty("ClientVersion").GetString();
        }
        catch
        {
            errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
            {
                result = ErrorCode.InValidRequestHttpBody
            });
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));

            return true;
        }
        
        if (string.CompareOrdinal(_masterDb.GetClientVersion(), clientAppVersion) == 0)
        {
            return false;
        }
        
        errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
        {
            result = ErrorCode.InvalidClientVersion
        });
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));

        return true;
    }
    
    bool IsInvalidJsonFormatThenSendError(HttpContext context, JsonDocument document, out int accountId, out string authToken, out int playerId)
    {
        try
        {
            accountId = document.RootElement.GetProperty("AccountId").GetInt32();
            authToken = document.RootElement.GetProperty("AuthToken").GetString();
            playerId = document.RootElement.GetProperty("PlayerId").GetInt32();

            return false;
        }
        catch
        {
            accountId = -1;
            authToken = "";
            playerId = -1;

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
