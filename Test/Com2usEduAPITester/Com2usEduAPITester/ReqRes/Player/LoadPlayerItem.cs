using System.ComponentModel.DataAnnotations;
using Com2usEduAPITester.Game;

namespace Com2usEduAPITester.ReqRes;

public class LoadPlayerItemRequest
{
	[Required]
	public int PlayerId { get; set; }

}

public class LoadPlayerItemResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;

	public IList<PlayerItem> PlayerItems { get; set; }
}