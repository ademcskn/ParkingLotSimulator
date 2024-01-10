using Newtonsoft.Json;
using System;

namespace ParkingLotSimulator.Business
{
    public class Vehicle
    {
        #region Internal Json Members
        //private TimeSpan ticketIssueTime;
        [JsonProperty]
        internal TimeSpan TicketIssueTime
        {
            set
            {
                //this.ticketIssueTime = value;
                this.ticketIssueDate = DateTime.Now.Date + value;
                if (DateTime.Now.TimeOfDay < value)
                {
                    this.ticketIssueDate = this.ticketIssueDate.AddDays(-1);
                }
            }
        }
        #endregion

        private string licencePlate;
        public string LicencePlate
        {
            get { return licencePlate; }
            set { licencePlate = value; }
        }

        private DateTime ticketIssueDate;
        public DateTime TicketIssueDate
        {
            get { return ticketIssueDate; }
            set { ticketIssueDate = value; }
        }

    }
}
