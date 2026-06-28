using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AppointmentAgent.Worker.Services
{    
    public class NotificationCacheService
    {
        private const string FileName = "notifiedSlots.json";

        private readonly HashSet<string> _cache = new();

        public NotificationCacheService()
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);

                var data = JsonSerializer.Deserialize<HashSet<string>>(json);

                if (data != null)
                    _cache = data;
            }
        }

        public bool IsNotified(string key)
        {
            return _cache.Contains(key);
        }

        public void Add(string key)
        {
            if (_cache.Add(key))
            {
                Save();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_cache, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FileName, json);
        }
    }
}
