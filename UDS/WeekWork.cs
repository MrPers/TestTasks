namespace UDS
{
    internal class WeekWork
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public WeekWork(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}