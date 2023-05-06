using System.Text;
using System.Text.Json;
using Com2usEduProject.Databases;

namespace Com2usEduProject.Middlewares;

public class CheckPlayerIdAuth
{
	readonly IGameDb _gameDb;
	readonly RequestDelegate _next;

	public CheckPlayerIdAuth(RequestDelegate next, IGameDb gameDb)
	{
		_gameDb = gameDb;
		_next = next;
	}
	
	public async Task Invoke(HttpContext context)
	{
		var formString = context.Request.Path.Value;
		if (string.Compare(formString, "/CreateAccount", StringComparison.OrdinalIgnoreCase) == 0 || 
		    string.Compare(formString, "/Login", StringComparison.OrdinalIgnoreCase) == 0)
		{
			await _next(context);
			return;
		}

		context.Request.EnableBuffering();
		
		using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 4096, true))
		{
			var bodyStr = await reader.ReadToEndAsync();
			var document = JsonDocument.Parse(bodyStr);

			int accountId, playerId;
			// 요청에서 PlayerId와 AccountId를 추출
			try
			{
				accountId = document.RootElement.GetProperty("AccountId").GetInt32();
				playerId = document.RootElement.GetProperty("PlayerId").GetInt32();
			}
			catch
			{
				var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
				{
					result = ErrorCode.InValidRequestHttpBody
				});
				await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));
				return;
			}

			//gameDB에서 플레이어 데이터를 로드
			var (errorCode, player) = await _gameDb.PlayerTable.SelectByAccountIdAsync(accountId);
			if (errorCode != ErrorCode.None)
			{
				var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
				{
					result = errorCode
				});
				await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));
				return;
			}
			
			// AccountId를 통해 찾은 PlayerId와 요청한 데이터의 PlayerId가 같은지 검증
			if (player.Id != playerId)
			{
				var errorJsonResponse = JsonSerializer.Serialize(new MiddlewareResponse
				{
					result = ErrorCode.UnAuthorizedPlayerId
				});
				await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorJsonResponse));
				return;
			}
		}
		
		context.Request.Body.Position = 0;

		await _next(context);
	}
	
	public class MiddlewareResponse
	{
		public ErrorCode result { get; set; }
	}
}