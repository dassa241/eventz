using System;
using System.Collections.Generic;
using System.Text;

namespace Eventz.BLL.DataObject
{
   public  class EventEnroll
    {
        public int EventEnrollId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime EntrollmentDate { get; set; }
        public bool IsPaid { get; set; }
        public decimal Amount { get; set; }

    }
}
