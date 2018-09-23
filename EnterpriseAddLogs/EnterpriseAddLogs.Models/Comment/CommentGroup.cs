using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EnterpriseAddLogs.Models
{
    public class CommentGroup
    {
        public string GroupTitle { get; set; }

        public ObservableCollection<CommentEntity> Comments { get; set; }
    }
}
