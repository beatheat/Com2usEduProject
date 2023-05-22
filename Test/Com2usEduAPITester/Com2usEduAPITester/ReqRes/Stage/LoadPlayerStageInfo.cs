using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;

public class LoadPlayerStageInfoRequest
{
    [Required]
    public int PlayerId { get; set; }
}

public class LoadPlayerStageInfoResponse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;

    public int MaxStageCode { get; set; }
    public List<int> ClearStageCodes { get; set; }
    public List<int> AccessibleStageCodes { get; set; }
}