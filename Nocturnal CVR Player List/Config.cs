using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Nocturnal
{
    public class Json
    {
        public string Background { get; set; }

        public bool LeftSide { get; set; }
    }
    internal class Config
    {
        public Json Js { get; set; }
        public static Config Instance { get; set; }
      
        public Config()
        {
            Instance = this;
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "//Nocturnal"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "//Nocturnal");

            if (!File.Exists(Directory.GetCurrentDirectory() + "//Nocturnal//PlayerListConfigV1.1.Json"))
            {
                using (WebClient wc = new WebClient())
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + "//Nocturnal//PlayerListConfigV1.1.Json", JsonConvert.SerializeObject(new Json()
                    {
                        Background = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/Edward7s/Nocturnal-CVR-Player-List/master/Nocturnal%20CVR%20Player%20List/Images/mask.jpg.png?token=GHSAT0AAAAAABR34D7BGOY4LIKQNVV7HPRMYXG34UQ")),
                        LeftSide = true
                    }));

                 wc.Dispose();
                }
            }
            Js = JsonConvert.DeserializeObject<Json>(File.ReadAllText(Directory.GetCurrentDirectory() + "//Nocturnal//PlayerListConfigV1.1.Json"));
        }
    }
}
