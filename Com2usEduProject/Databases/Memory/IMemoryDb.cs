using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.Databases;

public interface IMemoryDb
{
	public void Init(string address);
    
	public AuthManager AuthManager { get; }
	public NoticeManager NoticeManager { get; }
	public ChatManager ChatManager { get; }
	public StageManager StageManager { get; }
}

