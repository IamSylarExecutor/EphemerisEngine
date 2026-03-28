using EphemerisEngine.Core;

namespace EphemerisEngine.Interfaces;

// Çekirdek sistemin eklentilerle iletişim kurmasını sağlayan ZORUNLU sözleşme.
// Bu sözleşmeyi imzalayan her eklenti, kendi hesaplama mantığını yazmak zorundadır.
public interface ICelestialBodyPlugin
{
    string BodyName { get; }
    string CalculatePosition(SpatioTemporalData data);
}