using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Push.DTO
{
    public class NotificationPushDTO
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string PlayerId { get; set; } //Para adicionar ao log
    }
}