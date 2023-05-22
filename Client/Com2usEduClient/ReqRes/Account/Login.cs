using System.ComponentModel.DataAnnotations;
using Com2usEduClient.Game;

namespace Com2usEduClient.ReqRes;

public class LoginRequest
{
	[Required]
	[MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
	[StringLength(50, ErrorMessage = "ID IS TOO LONG")]
	public string LoginId { get; set; }

	[Required]
	[MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
	[StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
	[DataType(DataType.Password)]
	public string Password { get; set; }
	
}

public class LoginResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
	
	public int AccountId { get; set; }
	public string AuthToken { get; set; }
	public string Notice { get; set; }
	public int PlayerId { get; set; }
}