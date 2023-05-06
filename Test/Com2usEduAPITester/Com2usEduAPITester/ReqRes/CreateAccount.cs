using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;

public class CreateAccountRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(50, ErrorMessage = "ID IS TOO LONG")]
    public string LoginId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class CreateAccountResponse
{
	public ErrorCode Result { get; set; } = ErrorCode.None;
}