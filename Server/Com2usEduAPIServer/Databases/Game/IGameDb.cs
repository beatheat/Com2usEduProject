using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.Databases;

public interface IGameDb : IDisposable
{
	public PlayerTable PlayerTable { get; }
	public PlayerAttendanceTable PlayerAttendanceTable { get; }
	public PlayerItemTable PlayerItemTable { get; }
	public MailTable MailTable { get; }
	public BillTable BillTable { get; }
}