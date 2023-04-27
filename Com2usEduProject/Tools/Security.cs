using System.Security.Cryptography;
using System.Text;

namespace Com2usEduProject.Tools;

public class Security
{
	private const string ALLOWABLE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz0123456789";

	public static string MakeHashPassWord(string saltValue, String password)
	{
		var sha = SHA256.Create();
		var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(saltValue + password));
		var stringBuilder = new StringBuilder();
		foreach (var b in hash)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}

		return stringBuilder.ToString();
	}

	public static string MakeSaltString()
	{
		var bytes = new byte[64];
		using (var random = RandomNumberGenerator.Create())
		{
			random.GetBytes(bytes);
		}

		return new string(bytes.Select(x => ALLOWABLE_CHARACTERS[x % ALLOWABLE_CHARACTERS.Length]).ToArray());
	}

	public static string CreateAuthToken()
	{
		var bytes = new byte[25];
		using (var random = RandomNumberGenerator.Create())
		{
			random.GetBytes(bytes);
		}

		return new string(bytes.Select(x => ALLOWABLE_CHARACTERS[x % ALLOWABLE_CHARACTERS.Length]).ToArray());
	}
}