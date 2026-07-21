using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data;

public class AppDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Veritabanı dosyamız proje klasöründe "expenses.db" adıyla oluşacak
        optionsBuilder.UseSqlite("Data Source=expenses.db");
    }
}