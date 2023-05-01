using System.ComponentModel.DataAnnotations;
using Com2usEduProject.DBSchema;

namespace Com2usEduProject.ReqRes;

public class ReceiveMailRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int MailId { get; set; }
}

public class ReceiveMailResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}