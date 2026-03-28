using System;
using System.Collections.Generic;
using EphemerisEngine.Interfaces;

namespace EphemerisEngine.Core;

// Sistemin çekirdek yöneticisi. Hangi eklentinin ne iş yaptığını bilmez.
// Sadece sözleşmeye (ICelestialBodyPlugin) uyan eklentileri listesine alır ve çalıştırır.
public class PluginManager
{
    // Sisteme yüklenen aktif eklentilerin tutulduğu liste
    private readonly List<ICelestialBodyPlugin> _activePlugins = new();

    // Dışarıdan sisteme eklenti (Plug-in) enjekte eden metod
    public void LoadPlugin(ICelestialBodyPlugin plugin)
    {
        _activePlugins.Add(plugin);
        Console.WriteLine($"[Sistem Boot] '{plugin.BodyName}' modülü çekirdeğe başarıyla entegre edildi.");
    }

    // Gelen uzamsal veriyi tüm aktif eklentilere gönderip sonuçları toplayan metod
    public Dictionary<string, string> ExecuteAll(SpatioTemporalData data)
    {
        var results = new Dictionary<string, string>();
        
        foreach (var plugin in _activePlugins)
        {
            // Çekirdek sistem burada eklentinin CalculatePosition metodunu tetikler
            results.Add(plugin.BodyName, plugin.CalculatePosition(data));
        }
        
        return results;
    }
}