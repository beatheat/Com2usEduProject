using System.ComponentModel.DataAnnotations;
using Com2usEduProject.DBSchema;

namespace Com2usEduProject.ReqRes;

public class OpenMailboxRequest
{
	[Required]
	public int PlayerId { get; set; }
	public int PageNo { get; set; } = 1;
}

public class OpenMailboxResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;

	public int MailBoxPageCount { get; set; }
	public IList<Mail> MailboxPage { get; set; }
}