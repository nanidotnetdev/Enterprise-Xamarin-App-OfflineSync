using Xamarin.Forms;

namespace EnterpriseAddLogs.Messaging
{
	public sealed class ShowDetailPageMessage
    {
		public ShowDetailPageMessage(Page page)
		{
			Page = page;
		}

		public Page Page { get; private set; }
    }
}
