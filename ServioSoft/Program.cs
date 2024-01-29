using Microsoft.EntityFrameworkCore;
using ServioSoft;
using System.ComponentModel.DataAnnotations;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var context = new MyDbContext())
        {
            List<Company> companys = new List<Company>()
                {
                    new Company(){CompanyName = "ExpertSolution"},
                    new Company(){CompanyName = "ServioSoft" },
                    new Company(){CompanyName = "Apple" }
                };

            await context.GetCompanies.AddRangeAsync(companys);
            await context.SaveChangesAsync();

            List<Guest> guests = new List<Guest>()
                {
                    new Guest(){CompanyID = 1, Name = "Іван Василів"},
                    new Guest(){CompanyID = 1, Name = "Петро Симонович"},
                    new Guest(){CompanyID = 2, Name = "Василь Пупкін"},
                    new Guest(){CompanyID = 3, Name = "Сергій Іванов"},
                    new Guest(){CompanyID = 2, Name = "Роман Пушко"},
                    new Guest(){CompanyID = 1, Name = "Станіслав Мотужко"},
                    new Guest(){CompanyID = 1, Name = "Рустам Алекжі"},
                };

            await context.GetGuests.AddRangeAsync(guests);
            await context.SaveChangesAsync();

            /// <summary>
            /// All guests who live with the ExpertSolution company
            /// </summary>
            //var guestExpertSolutionAllAsync = await context.GetGuests
            //    .Join(context.GetCompanies.Where(c => c.CompanyName == "ExpertSolution"),
            //        p => p.CompanyID,
            //        t => t.CompanyID,
            //        (p, t) => new GuestDTO1
            //        {
            //            Name = p.Name,
            //            GuestID = p.CompanyID,
            //            CompanyName = t.CompanyName
            //        })
            //   .ToListAsync();

            /// < summary >
            /// All companies, based on the number of people who live with them, are sorted after a change in the number of guests
            /// </ summary >
            ///                 not work
            //var сompaniesAllAsync = await context.GetCompanies
            //    .Select(x => new CompanyDTO2
            //    {
            //        CompanyID = x.CompanyID,
            //        CompanyName = x.CompanyName,
            //        GuestCount = context.GetGuests.Where(t => t.CompanyID == x.CompanyID).Count()
            //    })
            //    .OrderByDescending(n => n.GuestCount)
            //    .ToListAsync();

            /// < summary >
            /// All companies, for which there are guests with a nickname that ends in “ko”. The result will display a list of such companies, without guest data.
            /// </ summary >
            //var сompaniesAllWithKoAsync = await context.GetCompanies
            //    .Join(context.GetGuests.Where(p => p.Name.Substring(p.Name.Length - 2) == "ко"),
            //        p => p.CompanyID,
            //        t => t.CompanyID,
            //        (p, t) => new
            //        {
            //            Name = p.CompanyName
            //        })
            //    .ToListAsync();

            /// < summary >
            /// All companies and guests who live with them. The result may display the name of the company, a list of names of guests via whom. To view the results of the vikory class CompanyDTO3
            /// </ summary >
            ///                 not work
            //var сompaniesAllAsync = await context.GetCompanies
            //.GroupJoin(context.GetGuests,
            //    p => p.CompanyID,
            //    t => t.CompanyID,
            //    (p, t) => new CompanyDTO3
            //    {
            //        CompanyID = p.CompanyID,
            //        CompanyName = p.CompanyName,
            //        GuestNames = String.Join(", ", t.Select(r => r.Name).ToList())
            //    }).ToListAsync();

        }
    }
}


public class GuestDTO1
{
    public int GuestID;
    public string Name;
    public string CompanyName;
}
public class CompanyDTO2
{
    public int CompanyID;
    public string CompanyName;
    public int GuestCount;
}
public class CompanyDTO3
{
    public int CompanyID;
    public string CompanyName;
    public string GuestNames;
}

public class Guest
{
    [Key]
    public int GuestID { get; set; }
    public int CompanyID { get; set; }
    public Company Company { get; set; }
    public string Name { get; set; }
}
public class Company
{
    [Key]
    public int CompanyID { get; set; }
    public string CompanyName { get; set; }
    public ICollection<Guest> Guests { get; set; } = new List<Guest>();
}