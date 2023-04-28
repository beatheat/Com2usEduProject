using System.ComponentModel.DataAnnotations;

namespace Com2usEduProject.ModelReqRes;

public class PkCreateAccountReq
{
	[Required]
	[MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
	[StringLength(50, ErrorMessage = "ID IS TOO LONG")]
	public String Id { get; set; }

	[Required]
	[MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
	[StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
	[DataType(DataType.Password)]
	public String Password { get; set; }
}

public class PkCreateAccountRes
{
	public ErrorCode Result { get; set; } = ErrorCode.None;
}