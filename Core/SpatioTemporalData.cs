using System;

namespace EphemerisEngine.Core;

// Kullanıcıdan alınan verileri eklentilere güvenle taşımak için oluşturulan veri sınıfı.
public class SpatioTemporalData
{
    public DateTime TargetDate { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}