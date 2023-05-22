using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.Databases;

public interface IMemoryDb
{
	public void Init(string address);
    
	public AuthManager AuthManager { get; }
	public NoticeManager NoticeManager { get; }
	public ChatManager ChatManager { get; }
	public StageManager StageManager { get; }
}

