using System.ComponentModel.DataAnnotations;

namespace Com2usEduClient.ReqRes;


public class ReceiveInAppPurchaseItemRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public long BillToken { get; set; }
	[Required]
	public int ShopCode { get;set; }
}

public class ReceiveInAppPurchaseItemResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;
	
}