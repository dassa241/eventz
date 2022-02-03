using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventz.BLL.DataObject;
using Eventz.DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventz.API.Controllers
{
    [ApiController]
    [Route("api/Event")]
    public class EventController : ControllerBase
    {
        private readonly IEvent _event;
        public EventController(IEvent uEvent)
        {
            _event = uEvent;
        }
        [HttpGet]
        public IActionResult GetEvents()
        {
            return Ok(_event.GetEvents());
        }
        [HttpGet("{id}")]
        public IActionResult GetEvents(int id)
        {
            return Ok(_event.GetEventById(id));
        }

        [HttpGet("events/{allEvent}")]
        public IActionResult GetMyEvents(bool allEvent)
        {
            var userId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserId").Value);
            return Ok(_event.GetMyEvents(userId, allEvent));
        }


        [HttpPost]
        public IActionResult SaveEvntRegistraion(EventEnroll eventEnroll)
        {
            //eventEnroll.UserId = Convert.ToInt32(User.Claims.First(c => c.Type == "UserId").Value);
            eventEnroll.EntrollmentDate = DateTime.Now;
            _event.SaveEventEnrollment(eventEnroll);
            return Ok(eventEnroll);
        }
    }
}
