using System;
using System.Collections.Generic;
using System.Text;

namespace Eventz.BLL.ViewModels
{
   public class Event
    {
        public int EventIdx { get; set; }
        public string EventName { get; set; }
        public string EventVenue{ get; set; }
        public DateTime EventDate { get; set; }
        public string HostName { get; set; }
        public string Speakers { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        
    }
}
