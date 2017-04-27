using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Gamific.Model.Account.DTO
{
    public class AccountDevicesDTO
    {
        public string Id { get; set; }
        public string UUID { get; set; }
        public string Notification_Token { get; set; }
        public string Plataform { get; set; }
        public string External_User_Id { get; set; }
        public DateTime Last_Update { get; set; }
        public string PlayerName { get; set; }
    }
}
