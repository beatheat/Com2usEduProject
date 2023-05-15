using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;

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