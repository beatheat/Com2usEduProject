﻿using System.ComponentModel.DataAnnotations;
using Com2usEduAPITester;
using Com2usEduAPITester.Game;

namespace Com2usEduAPITester.ReqRes;

public enum EnforceState
{
	Success,
	Fail,
	Disable,
	Error
}

public class EnforcePlayerItemRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int PlayerItemId { get; set; }
	
}

public class EnforcePlayerItemResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None; 
	public EnforceState EnforceState { get; set; }
	public PlayerItem EnforcedItem { get; set; }

}