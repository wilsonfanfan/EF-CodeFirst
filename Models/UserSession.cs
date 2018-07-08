using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace X.Models
{
    public class UserSession
    {
        //public Dictionary<long, string> AuthList { get; set; }
        public List<string> RoleList { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
      
    }
}
