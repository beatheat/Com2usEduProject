using Com2usEduProject.GameLogic;

namespace Com2usEduProject.Databases.Schema.Extension;


public static class MailExtension
{
	public static void AddItem(this Mail mail, int itemCode, int itemCount)
	{
		if (mail.ItemCode1 == -1)
		{
			mail.ItemCode1 = itemCode;
			mail.ItemCount1 = itemCount;
			return;
		}

		if (mail.ItemCode2 == -1)
		{
			mail.ItemCode2 = itemCode;
			mail.ItemCount2 = itemCount;
			return;
		}

		if (mail.ItemCode3 == -1)
		{
			mail.ItemCode3 = itemCode;
			mail.ItemCount3 = itemCount;
			return;
		}

		if (mail.ItemCode4 == -1)
		{
			mail.ItemCode4 = itemCode;
			mail.ItemCount4 = itemCount;
			return;
		}
	}

	public static IEnumerable<ItemBundle> GetItemList(this Mail mail)
	{
		yield return new ItemBundle{ItemCode = mail.ItemCode1, ItemCount = mail.ItemCount1};
		yield return new ItemBundle{ItemCode = mail.ItemCode2, ItemCount = mail.ItemCount2};
		yield return new ItemBundle{ItemCode = mail.ItemCode3, ItemCount = mail.ItemCount3};
		yield return new ItemBundle{ItemCode = mail.ItemCode4, ItemCount = mail.ItemCount4};
	}
	
}

