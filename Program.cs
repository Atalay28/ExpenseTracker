using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker;

class Program
{
    static void Main(string[] args)
    {
        // 1. Veritabanı bağlantısını başlat
        using (var db = new AppDbContext())
        {
            // Veritabanı dosyası (expenses.db) henüz yoksa otomatik oluşturur
            db.Database.EnsureCreated();
            Console.WriteLine("✅ Veritabanı bağlantısı başarılı!\n");

            bool running = true;
            while (running)
            {
                Console.WriteLine("--- KİŞİSEL BÜTÇE TAKİBİ ---");
                Console.WriteLine("1. Harcama Ekle");
                Console.WriteLine("2. Tüm Harcamaları Listele");
                Console.WriteLine("3. Çıkış");
                Console.Write("Seçiminiz (1-3): ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddExpense(db);
                        break;
                    case "2":
                        ListExpenses(db);
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("Görüşmek üzere!");
                        break;
                    default:
                        Console.WriteLine("❌ Geçersiz seçim, tekrar deneyin.\n");
                        break;
                }
            }
        }
    }

    // Harcama Ekleme Metodu
    static void AddExpense(AppDbContext db)
    {
        Console.Write("\nHarcama Açıklaması: ");
        string description = Console.ReadLine() ?? "Bilinmeyen Harcama";

        Console.Write("Tutar (TL): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("❌ Geçersiz tutar girdiniz!\n");
            return;
        }

        Console.Write("Kategori (Yemek, Ulaşım, Eğlence vb.): ");
        string category = Console.ReadLine() ?? "Genel";

        var newExpense = new Expense
        {
            Description = description,
            Amount = amount,
            Category = category,
            Date = DateTime.Now
        };

        // Entity Framework ile veritabanına ekle
        db.Expenses.Add(newExpense);
        db.SaveChanges(); // Değişiklikleri veritabanı dosyasına kaydet

        Console.WriteLine("🎉 Harcama başarıyla kaydedildi!\n");
    }

    // Harcamaları Listeleme Metodu
    static void ListExpenses(AppDbContext db)
    {
        var expenses = db.Expenses.ToList();

        if (expenses.Count == 0)
        {
            Console.WriteLine("\n📌 Henüz kaydedilmiş bir harcama yok.\n");
            return;
        }

        Console.WriteLine("\n================ HARCAMA LİSTESİ ================");
        decimal total = 0;

        foreach (var item in expenses)
        {
            Console.WriteLine($"[{item.Id}] {item.Date:dd.MM.yyyy HH:mm} | {item.Category} | {item.Description}: {item.Amount:N2} TL");
            total += item.Amount;
        }

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine($"TOPLAM HARCAMA: {total:N2} TL");
        Console.WriteLine("=================================================\n");
    }
}