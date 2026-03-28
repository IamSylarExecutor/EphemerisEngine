using System;
using System.Globalization; // Kültür bağımsız tarih işlemleri için eklendi
using EphemerisEngine.Core;
using EphemerisEngine.Plugins;

namespace EphemerisEngine;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=========================================================");
        Console.WriteLine("Spatio-Temporal Ephemeris Veri İşleme Motoru v1.0");
        Console.WriteLine("Mimari: Microkernel (Plug-in Tabanlı Dağıtık Yapı)");
        Console.WriteLine("=========================================================\n");

        PluginManager engine = new PluginManager();
        engine.LoadPlugin(new SunPositionPlugin());
        engine.LoadPlugin(new MoonPositionPlugin());

        Console.WriteLine("\n[Giriş] Lütfen hedef uzamsal-zamansal parametreleri giriniz.");
        Console.Write("Hedef Tarih ve Saat (Örn: 18-12-1812 18:12): ");
        
        // DÜZELTME 1: CS8600 uyarısını çözen '?' (Nullable) işareti
        string? inputDate = Console.ReadLine();

        
        // Hangi bilgisayarda çalışırsa çalışsın sadece bu formatları kabul edecek.
        string[] expectedFormats = { "dd-MM-yyyy HH:mm", "dd.MM.yyyy HH:mm", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm" };
        
        if (string.IsNullOrWhiteSpace(inputDate) || !DateTime.TryParseExact(inputDate, expectedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetDate))
        {
            Console.WriteLine("[Uyarı] Geçersiz format. Sistem güvenlik için anlık zamanı (Şu an) referans alıyor.");
            targetDate = DateTime.Now;
        }

        SpatioTemporalData data = new SpatioTemporalData
        {
            TargetDate = targetDate,
            Latitude = 41.0082, 
            Longitude = 28.9784
        };

        Console.WriteLine("\n[İşlem] Ephemeris vektörleri hesaplanıyor...\n");

        var results = engine.ExecuteAll(data);

        Console.WriteLine("========== HESAPLAMA SONUÇLARI ==========");
        foreach (var result in results)
        {
            Console.WriteLine($"-> Gök Cismi Referansı: {result.Key}");
            Console.WriteLine($"-> Uzamsal Vektör: {result.Value}");
        }
        Console.WriteLine("=========================================");
        
        Console.WriteLine("\nSistemi kapatmak için bir tuşa basın...");
        Console.ReadKey();
    }
}