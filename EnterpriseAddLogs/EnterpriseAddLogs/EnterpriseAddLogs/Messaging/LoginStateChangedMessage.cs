namespace EnterpriseAddLogs.Messaging
{
	public class LoginStateChangedMessage
	{
		public LoginStateChangedMessage(bool isLoggedIn)
		{
			IsLoggedIn = isLoggedIn;
		}

		public bool IsLoggedIn { get; private set; }
    }
}