using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;

namespace Com2usEduProject.Databases;

public interface IGameDb
{
	public PlayerTable PlayerTable { get; }
	public PlayerItemTable PlayerItemTable { get; }
	public MailTable MailTable { get; }
	public MailItemTable MailItemTable { get; }
}