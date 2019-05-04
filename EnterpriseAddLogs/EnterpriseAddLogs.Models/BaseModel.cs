using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace EnterpriseAddLogs.Models
{
	public class BaseModel: INotifyPropertyChanged
    {
		string _id;

		[JsonProperty("id")]
		public string Id
		{
			get
			{
				return _id;
			}
			set
            {
                _id = value;
                NotifyPropertyChanged();
			}
		}

		DateTime? _updatedAt;

		public DateTime? UpdatedAt
		{
			get
			{
				return _updatedAt;
			}
			set
            {
                _updatedAt = value;
                NotifyPropertyChanged();
			}
		}

		DateTime? _createdAt;

		[CreatedAt]
		public DateTime? CreatedAt
		{
			get
			{
				return _createdAt;
			}
            set
            {
                _createdAt = value;
                NotifyPropertyChanged();
            }
		}

		string _version;

		[Version]
		public string Version
		{
			get
			{
				return _version;
			}
            set
            {
                _version = value;
                NotifyPropertyChanged();
            }
		}

		[JsonIgnore]
		public bool IsDirty
		{
			get;
			set;
		}

        [Deleted]
        public bool Deleted { get; set; }

        public virtual void LocalRefresh()
		{
		}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}