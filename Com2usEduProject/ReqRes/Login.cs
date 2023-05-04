using System.ComponentModel.DataAnnotations;
using Com2usEduProject.DBSchema;

namespace Com2usEduProject.ReqRes;

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
	[Required] public string AuthToken { get; set; } = "";
	
	public Player PlayerData { get; set; }
	public IList<PlayerItem> PlayerItems { get; set; }
	public string Notice { get; set; }
}