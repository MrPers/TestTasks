using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS
{
    [TestClass()]
    public class WorkDayCalculatorTests
    {

        [TestMethod()]
        public void TestEndBeforeWorkWeek()
        {
            DateTime startDate = new DateTime(2021, 4, 21);
            int count = 5;
            WeekEnd[] weekends = new WeekEnd[1]
            {
                new WeekEnd(new DateTime(2021, 4, 20), new DateTime(2021, 4, 20))
            };

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.IsTrue(result.Equals(new DateTime(2021, 4, 25)));
        }

        //[TestMethod]
        //public void TestNoWeekEnd2()
        //{
        //    DateTime startDate = new DateTime(2021, 4, 15);
        //    int count = 3;
        //    WeekEnd[] weekends = new WeekEnd[]
        //    {
        //        new WeekEnd(new DateTime(2021, 4, 13), new DateTime(2021, 4, 13)),
        //        new WeekEnd(new DateTime(2021, 4, 14), new DateTime(2021, 4, 16)),
        //        new WeekEnd(new DateTime(2021, 4, 17), new DateTime(2021, 4, 17)),
        //        new WeekEnd(new DateTime(2021, 4, 22), new DateTime(2021, 4, 22))
        //    };

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

        //    Assert.IsTrue(result.Equals(new DateTime(2021, 4, 20)));
        //}

        //[TestMethod]
        //public void TestNoWeekEnd()
        //{
        //    DateTime startDate = new DateTime(2021, 12, 1);
        //    int count = 10;

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, null);

        //    Assert.AreEqual(startDate.AddDays(count - 1), result);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void TestLessOneWorkingDays()
        //{
        //    DateTime startDate = new DateTime(2021, 12, 1);
        //    int count = -1;
        //    WeekEnd[] weekends = new WeekEnd[1]
        //    {
        //        new WeekEnd(new DateTime(2021, 4, 23), new DateTime(2021, 4, 25))
        //    };

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

        //}

        //[TestMethod]
        //public void TestNormalPath()
        //{
        //    DateTime startDate = new DateTime(2021, 4, 21);
        //    int count = 3;
        //    WeekEnd[] weekends = new WeekEnd[]
        //    {
        //        new WeekEnd(new DateTime(2021, 4, 20), new DateTime(2021, 4, 21))
        //    };

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

        //    Assert.IsTrue(result.Equals(new DateTime(2021, 4, 24)));
        //}

        //[TestMethod]
        //public void TestWeekendOutOrder()
        //{
        //    DateTime startDate = new DateTime(2021, 4, 21);
        //    int count = 5;
        //    WeekEnd[] weekends = new WeekEnd[]
        //    {
        //        new WeekEnd(new DateTime(2021, 4, 29), new DateTime(2021, 4, 29)),
        //        new WeekEnd(new DateTime(2021, 4, 23), new DateTime(2021, 4, 25))
        //    };

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

        //    Assert.IsTrue(result.Equals(new DateTime(2021, 4, 28)));
        //}

        //[TestMethod]
        //public void TestWeekendMixed()
        //{
        //    DateTime startDate = new DateTime(2021, 4, 21);
        //    int count = 5;
        //    WeekEnd[] weekends = new WeekEnd[]
        //    {
        //        new WeekEnd(new DateTime(2021, 4, 25), new DateTime(2021, 4, 23))
        //    };

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

        //    Assert.IsTrue(result.Equals(new DateTime(2021, 4, 28)));
        //}

        //[TestMethod]
        //public void TestWeekendAfterEnd()
        //{
        //    DateTime startDate = new DateTime(2021, 4, 21);
        //    int count = 5;
        //    WeekEnd[] weekends = new WeekEnd[2]
        //    {
        //        new WeekEnd(new DateTime(2021, 4, 23), new DateTime(2021, 4, 25)),
        //        new WeekEnd(new DateTime(2021, 4, 29), new DateTime(2021, 4, 29))
        //    };

        //    DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

        //    Assert.IsTrue(result.Equals(new DateTime(2021, 4, 28)));
        //}
    }
}

