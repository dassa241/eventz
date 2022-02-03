using Dapper;
using Eventz.BLL.DataObject;
using Eventz.BLL.ViewModels;
using Eventz.DAL.Contracts;
using Eventz.DAL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Eventz.DAL.DataAccess
{
    public class EventDAL : DbAccess, IEvent
    {
        public Event GetEventById(int evntId)
        {

        try
        {
            var para = new DynamicParameters();
            para.Add("@EventId", evntId);
            return ReturnObject<Event>("spGetEventsById", para);
        }
        catch (Exception)
        {

            throw new Exception("unable to get event");
        }
    }

        public List<Event> GetEvents()
        {
            
                try
                {
                    var para = new DynamicParameters();
                    return ReturnList<Event>("spGetEvents", para);
                }
                catch (Exception ex)
                {

                    throw new Exception("unable to get events");
                }
            }

        public void SaveEventEnrollment(EventEnroll eventEnroll)
        {
            try
            {
                BeginTranaction();
                DoSaveEventEnrollment(eventEnroll);
                CommitTranaction();

            }
            catch (Exception ex)
            {
                RollbackTranaction();
                throw new Exception(ex.Message);
            }
        }
        public void DoSaveEventEnrollment(EventEnroll eventEnroll)
        {
           
               
                int userId = 0;
                var user = GetUserByEmail(eventEnroll.Email);
                if (user!=null)
                {
                    userId = user.UserIdx;
                    CheckUserHasEvent(userId, eventEnroll.EventId);
                }
                else
                {
                    userId = CreateUser(eventEnroll.FirstName, eventEnroll.LastName, eventEnroll.Email, eventEnroll.MobileNo);
                }
              
                var para = new DynamicParameters();
                para.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                para.Add("@UserId", userId);
                para.Add("@EventId", eventEnroll.EventId);
                para.Add("@IsPaid", eventEnroll.IsPaid);
                var result = ExecuteTranaction("spSaveEventEnroll", para);
                int eventEnrollId = para.Get<int>("@result");
            
        }
        private User GetUserByEmail(string email)
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@Email", email);
                return ReturnObject<User>("spGetUserByEmail", para);
            }
            catch (Exception ex)
            {

                throw new Exception("unable to get my events");
            }
        }
        private void CheckUserHasEvent(int userId,int eventId)
        {
           
            var para = new DynamicParameters();
            para.Add("@UserId", userId);
            para.Add("@EventId", eventId);
            ExecuteWithoutReturn("spCheckUserHasEvent", para);
           
        }
        private int CreateUser(string firstName,string lastName,string email,string mobileNo)
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var password = CryptoEncrypt.Encrypt("a", true);
                para.Add("@FirstName", firstName);
                para.Add("@LastName", lastName);
                para.Add("@Email", email);
                para.Add("@Username", email);
                para.Add("@Password", password);
                para.Add("@MobileNo", mobileNo);
                var result = ExecuteTranaction("spCreateUser", para);
                return para.Get<int>("@result");
            }
            catch (Exception ex)
            {

                throw new Exception("unable to create user");
            }
        }

        public List<Event> GetMyEvents(int userId,bool allEvent)
        {
            try
            {
                var para = new DynamicParameters();
                para.Add("@UserId", allEvent?-1:userId);
                return ReturnList<Event>("spGetEventsByUserId", para);
            }
            catch (Exception ex)
            {

                throw new Exception("unable to get my events");
            }
        }
    }
}
