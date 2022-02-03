using Eventz.BLL.DataObject;
using Eventz.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventz.DAL.Contracts
{
   public interface IEvent
    {

        List<Event> GetEvents();
        Event GetEventById(int evntId);
        void SaveEventEnrollment(EventEnroll eventEnroll);
        List<Event> GetMyEvents(int userId, bool allEvent);


    }
}
