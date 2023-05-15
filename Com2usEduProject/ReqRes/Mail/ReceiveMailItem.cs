using System.ComponentModel.DataAnnotations;
using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.ReqRes;

public class ReceiveMailItemRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int MailId { get; set; }
}

public class ReceiveMailItemResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}