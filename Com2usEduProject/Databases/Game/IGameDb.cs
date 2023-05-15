using Com2usEduProject.Databases.Schema;
using Com2usEduProject.ModelDB;

namespace Com2usEduProject.Databases;

public interface IGameDb
{
	public PlayerTable PlayerTable { get; }
	public PlayerAttendanceTable PlayerAttendanceTable { get; }
	public PlayerCompletedStageTable PlayerCompletedStageTable { get; }
	public PlayerItemTable PlayerItemTable { get; }
	public MailTable MailTable { get; }
	public BillTable BillTable { get; }
}