using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MarketBackend
{
    public class DataDbContext : DbContext
    {

        public DbSet<locations> locations { get; set; }
        public DbSet<users> users { get; set; }
        public DbSet<stocks> stocks { get; set; }
        public DbSet<orders> orders { get; set; }

        public DbSet<companys> companys { get; set; }
        public DbSet<product_details> product_details { get; set; }
        public DbSet<order_details> order_details { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseNpgsql("Host=192.168.1.61;Username=postgres;Password=23054q;Database=TaxiDB");
    }

    public class locations
    {
        public int id { get; set; }
        public string? longitude { get; set; }
        public string? latitude { get; set; }
        public string? accuracy { get; set; }
        public string ? heading { get; set; }
        public string? altitude { get; set; }
        public string? altitudeaccuracy { get; set; }
        public string? floor { get; set; }
        public string? timestamp { get; set; }
        public string? frommockprovider { get; set; }
        public DateTime? created_date { get; set; }
        public string? created_by { get; set; }
        public string? speedaccuracy { get; set; }
        public string? speed { get; set; }
        public int? userid { get; set; }

    }

    public class users
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime? created_date { get; set; }
        public string? created_by { get; set; }

       

    }

    public class stocks
    {
        public int id { get; set; }
        public int piece { get; set; }
        public int product_id { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public string ?created_by { get; set; }
        public string ?updated_by { get; set; }

    }

    public class orders
    {
        public int id { get; set; }
        public float price { get; set; }
        public int piece { get; set; }
        public DateTime? updated_date { get; set; }
        public DateTime? created_date { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public int? company_id { get; set; }
    }
    public class order_details
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public float price { get; set; }
        public int? company_id { get; set; }
        public DateTime? updated_date { get; set; }
        public DateTime? created_date { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }

    }
    public class companys
    {
        public int id { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string adress { get; set; }

        public int product_id { get; set; }

        public int order_id { get; set; }
        public DateTime created_date { get; set; }
        public DateTime ?updated_date { get; set; }
        public string created_by { get; set; }
        public string ?updated_by { get; set; }
    }

    public class product_details
    {
        public int id { get; set; }

        public DateTime? expiration_date { get; set; }

        public string ?supplier { get; set; }

        public DateTime ?production_date { get; set; }

        public string ?picture { get; set; }

        public int ?product_id { get; set; }

        public DateTime ?created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public string ?created_by { get; set; }
        public string? updated_by { get; set; }
    }
}
