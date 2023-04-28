using System.ComponentModel.DataAnnotations;

namespace Com2usEduProject.ModelReqRes;

public class PkLoginRequest
{
	[Required]
	[MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
	[StringLength(50, ErrorMessage = "ID IS TOO LONG")]
	public String Id { get; set; }

	[Required]
	[MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
	[StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
	[DataType(DataType.Password)]
	public String Password { get; set; }
}

public class PkLoginResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
	[Required] public String AuthToken { get; set; } = "";
}