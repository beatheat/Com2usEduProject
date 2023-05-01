namespace Com2usEduProject.DBSchema;

public class Account
{
	public int AccountId { get; set; }

	public string Id { get; set; }
	public string HashedPassword { get; set; }
	public string SaltValue { get; set; }
}
