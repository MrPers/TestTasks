using System;
using System.Runtime.InteropServices.ComTypes;
using UDS;

namespace UDS
{
    public class WorkDayCalculator : IWorkDayCalculator
    {
        public DateTime Calculate(DateTime startDate, int dayCount, WeekEnd[] weekEnds)
        {
            int firstDayNotAddition;
            DateTime sumWorkingDays;
            WeekWork weekWork;

            if (dayCount < 1)
            {
                throw new ArgumentException("0 or less business days");
            }

            firstDayNotAddition = dayCount - 1;
            weekWork = new WeekWork(startDate, startDate.AddDays(firstDayNotAddition));
            sumWorkingDays = AddWorkEndDays(weekEnds, weekWork);

            return sumWorkingDays;
        }

        private static DateTime AddWorkEndDays(WeekEnd[] weekEnds, WeekWork weekWork)
        {
            WeekEnd weekEndSort;

            if (weekEnds?.Length > 0)
            {
                foreach (var weekEnd in weekEnds)
                {
                    weekEndSort = weekEnd.EndDate < weekEnd.StartDate ?
                        new WeekEnd(weekEnd.EndDate, weekEnd.StartDate) :
                        weekEnd;

                    if (weekWork.EndDate >= weekEndSort.StartDate && weekWork.StartDate <= weekEndSort.EndDate)
                    {
                        for (var day = weekEndSort.StartDate; day <= weekEndSort.EndDate; day = day.AddDays(1))
                        {
                            if (weekWork.StartDate <= day && day <= weekWork.EndDate)
                            {
                                weekWork.EndDate = weekWork.EndDate.AddDays(1);
                            }
                        }
                    }
                }
            }

            return weekWork.EndDate;
        }
    }
}
