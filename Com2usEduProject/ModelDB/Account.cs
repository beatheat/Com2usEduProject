namespace Com2usEduProject.ModelDB;

public class Account
{
	public Int64 AccountId { get; set; }

	public String Id { get; set; }
	public String HashedPassword { get; set; }
	public String SaltValue { get; set; }
}
