using System.ComponentModel.DataAnnotations;

using Com2usEduAPITester.Game;

namespace Com2usEduAPITester.ReqRes;

public class LoadMailListRequest
{
	[Required]
	public int PlayerId { get; set; }
	public int PageNo { get; set; } = 1;
}

public class LoadMailListResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;

	public int TotalPageCount { get; set; }
	public IList<Mail> MailList { get; set; }
}