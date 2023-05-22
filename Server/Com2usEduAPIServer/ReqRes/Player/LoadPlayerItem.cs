using System.ComponentModel.DataAnnotations;
using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.ReqRes;

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