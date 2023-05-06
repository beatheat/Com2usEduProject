using System.ComponentModel.DataAnnotations;
using Com2usEduProject.DBSchema;

namespace Com2usEduProject.ReqRes;

public class LoadMailRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int MailId { get; set; }
}

public class LoadMailResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;

	public Mail Mail { get; set; }
	public IList<MailItem> MailItems { get; set; }
}