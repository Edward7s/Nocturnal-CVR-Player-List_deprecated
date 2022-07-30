using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace Nocturnal
{
    internal class PlayerList : MonoBehaviour
    {
        private Image _imgComp { get; set; }
        public static PlayerList Instance { get; set; }
         void Start()
         {
            Instance = this;
            this.transform.localPosition = Vector3.zero;
            this.transform.localEulerAngles = Vector3.zero;
            this.transform.localScale = Vector3.one;
            this.gameObject.AddComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            this.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            _imgComp = this.gameObject.AddComponent<Image>();
            ChangeSpriteFromString(_imgComp, Config.Instance.Js.Background);
         }

        void UpdatePlayerList()
        {

        }

        private static Texture2D _Texture2d { get; set; }
        private Image ChangeSpriteFromString(Image Image, string ImageBase64)
        {
            _Texture2d = new Texture2D(256, 256);
            ImageConversion.LoadImage(_Texture2d, Convert.FromBase64String(ImageBase64));
            Image.sprite = Sprite.Create(_Texture2d, new Rect(0, 0, _Texture2d.width, _Texture2d.height), new Vector2(0, 0), 200, 1000u, SpriteMeshType.FullRect, Vector4.zero, false);
            return Image;
        }
    }
}
