using System;
using EphemerisEngine.Core;
using EphemerisEngine.Interfaces;

namespace EphemerisEngine.Plugins;

// Ay modülü. Çekirdek sistemden tamamen bağımsızdır, sadece Sözleşmeye (Interface) uyar.
public class MoonPositionPlugin : ICelestialBodyPlugin
{
    public string BodyName => "Moon";

    public string CalculatePosition(SpatioTemporalData data)
    {
        // 1. Julian Günü Yaklaşımı (J2000.0 Referanslı)
        DateTime j2000 = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        double daysSinceJ2000 = (data.TargetDate.ToUniversalTime() - j2000).TotalDays;

        // 2. Ay'ın Ortalama Boylamı (Lunar Mean Longitude) - Bilimsel görünümlü yaklaşım
        double lunarLongitude = (218.316 + 13.176396 * daysSinceJ2000) % 360;
        if (lunarLongitude < 0) lunarLongitude += 360;

        // 3. Ay Evresi (Lunar Phase) Hesaplaması (0 - 29.53 Günlük Döngü)
        double lunarPhase = (daysSinceJ2000 % 29.530588853);
        if (lunarPhase < 0) lunarPhase += 29.530588853;
        string phaseName = DeterminePhase(lunarPhase);

        // 4. Astroloji Sektörünü Bilimsel Dille Belirleme
        string sector = DetermineSector(lunarLongitude);

        return $"Ecliptic Longitude: {lunarLongitude:F2}° | Orbital Sector: {sector} | Phase: {phaseName}";
    }

    private string DeterminePhase(double phaseDays)
    {
        if (phaseDays < 1.84 || phaseDays > 27.68) return "New Moon (Yeni Ay)";
        if (phaseDays < 5.53) return "Waxing Crescent (Büyüyen Ay)";
        if (phaseDays < 9.22) return "First Quarter (İlk Dördün)";
        if (phaseDays < 12.91) return "Waxing Gibbous (Şişkin Ay)";
        if (phaseDays < 16.60) return "Full Moon (Dolunay)";
        if (phaseDays < 20.29) return "Waning Gibbous (Küçülen Şişkin Ay)";
        if (phaseDays < 23.98) return "Last Quarter (Son Dördün)";
        return "Waning Crescent (Küçülen Ay)";
    }

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