namespace EnterpriseAddLogs.Messaging
{
	public class ShowMenuMessage
    {
		public ShowMenuMessage(bool showMenu)
		{
			ShowMenu = showMenu;
		}

		public bool ShowMenu { get; private set; }
    }
}
