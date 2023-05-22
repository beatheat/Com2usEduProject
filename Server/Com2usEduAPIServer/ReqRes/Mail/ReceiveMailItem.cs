using System.ComponentModel.DataAnnotations;
using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.ReqRes;

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