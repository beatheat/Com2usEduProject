namespace Com2usEduProject.DBSchema;

public class Account
{
	public int Id { get; set; }

	public string LoginId { get; set; }
	public string HashedPassword { get; set; }
	public string SaltValue { get; set; }
	public DateTime CreateAt { get; set; }
}
