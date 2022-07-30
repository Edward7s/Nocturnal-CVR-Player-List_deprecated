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
        public int[] Color { get; set; }
        public string Background { get; set; }
        public string PlayerList { get; set; }

    }
    internal class Config
    {
        public Json Js { get; set; }
        public Color DefaultColor { get; set; }
        public static Config Instance { get; set; }
      
        public Config()
        {
            Instance = this;
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "//Nocturnal"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "//Nocturnal");

            if (!File.Exists(Directory.GetCurrentDirectory() + "//Nocturnal//PlayerListConfig.Json"))
            {
                using (WebClient wc = new WebClient())
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + "//Nocturnal//PlayerListConfig.Json", JsonConvert.SerializeObject(new Json()
                    {
                        Color = new int[] { 145, 255, 158, 200 },
                      //  Background = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/Edward7s/Nocturnal-CVR-Player-List/master/Nocturnal%20CVR%20Player%20List/Images/BackgroundMenu.png?token=GHSAT0AAAAAABR34D7BZIJJDORMV7YNSNTMYXEMDOQ")),
                      //  PlayerList = Convert.ToBase64String(wc.DownloadData("https://raw.githubusercontent.com/Edward7s/Nocturnal-CVR-Player-List/master/Nocturnal%20CVR%20Player%20List/Images/PlayerListMask.png?token=GHSAT0AAAAAABR34D7BUI2B3YZJ7TVL2AIIYXEMFHQ"))
                        Background = Convert.ToBase64String(wc.DownloadData("https://nocturnal-client.xyz/Resources/BackgroundMenu.png")),
                        PlayerList = Convert.ToBase64String(wc.DownloadData("https://nocturnal-client.xyz/Resources/PlayerListMask.png"))
                    }));
                    wc.Dispose();
                }
            }
            Js = JsonConvert.DeserializeObject<Json>(File.ReadAllText(Directory.GetCurrentDirectory() + "//Nocturnal//PlayerListConfig.Json"));
            DefaultColor = new Color32(byte.Parse(Js.Color[0].ToString()), byte.Parse(Js.Color[1].ToString()), byte.Parse(Js.Color[2].ToString()), byte.Parse(Js.Color[3].ToString()));
        }
    }
}
