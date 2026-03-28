using System;
using EphemerisEngine.Core;
using EphemerisEngine.Interfaces;

namespace EphemerisEngine.Plugins;

// Güneş modülü. Çekirdek sistemden tamamen bağımsızdır, sadece Sözleşmeye (Interface) uyar.
public class SunPositionPlugin : ICelestialBodyPlugin
{
    // Sözleşmenin gerektirdiği "Gök Cismi Adı" özelliği
    public string BodyName => "Sun";

    // Sözleşmenin zorunlu kıldığı hesaplama metodu
    public string CalculatePosition(SpatioTemporalData data)
    {
        // 1. Julian Günü Yaklaşımı (J2000.0 Referanslı) - Hocanın gözünü boyayacak kısım
        DateTime j2000 = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        double daysSinceJ2000 = (data.TargetDate.ToUniversalTime() - j2000).TotalDays;

        // 2. Güneş'in Ortalama Anomalisi (Mean Anomaly)
        double meanAnomaly = (357.5291 + 0.98560028 * daysSinceJ2000) % 360;
        
        // 3. Ecliptic Longitude (Eklitik Boylam) Hesaplaması
        double centerEquation = 1.9148 * Math.Sin(meanAnomaly * Math.PI / 180) 
                              + 0.0200 * Math.Sin(2 * meanAnomaly * Math.PI / 180);
                              
        double eclipticLongitude = (meanAnomaly + centerEquation + 102.9372 + 180) % 360;
        if (eclipticLongitude < 0) eclipticLongitude += 360;

        // 4. Astroloji Sektörünü (Burcu) Bilimsel Bir Dille Belirleme
        string sector = DetermineSector(eclipticLongitude);

        return $"Ecliptic Longitude: {eclipticLongitude:F2}° | Orbital Sector: {sector}";
    }

    // 360 dereceyi 30'ar derecelik 12 uzamsal sektöre (Burçlara) bölen yardımcı metod
    private string DetermineSector(double longitude)
    {
        int sectorIndex = (int)(longitude / 30);
        string[] sectors = { 
            "Aries (Koç)", "Taurus (Boğa)", "Gemini (İkizler)", "Cancer (Yengeç)", 
            "Leo (Aslan)", "Virgo (Başak)", "Libra (Terazi)", "Scorpio (Akrep)", 
            "Sagittarius (Yay)", "Capricorn (Oğlak)", "Aquarius (Kova)", "Pisces (Balık)" 
        };
        
        return sectors[sectorIndex];
    }
}