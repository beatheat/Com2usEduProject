﻿using System.ComponentModel.DataAnnotations;
using Com2usEduClient.Game;

namespace Com2usEduClient.ReqRes;

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