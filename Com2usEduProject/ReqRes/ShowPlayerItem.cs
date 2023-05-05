using System.ComponentModel.DataAnnotations;
using Com2usEduProject.DBSchema;

namespace Com2usEduProject.ReqRes;

public class ShowPlayerItemRequest
{
	[Required]
	public int PlayerId { get; set; }

}

public class ShowPlayerItemResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;

	public IList<PlayerItem> PlayerItems { get; set; }
}