using Com2usEduProject.DBSchema;
using Com2usEduProject.Authorization;
using Com2usEduProject.Chatting;

namespace Com2usEduProject.Databases;

public interface IMemoryDb
{
	public void Init(string address);
    
	public AuthManager AuthManager { get; }
	public NoticeManager NoticeManager { get; }
	public ChatManager ChatManager { get; }
}

