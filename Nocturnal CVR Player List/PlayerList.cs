using ABI_RC.Core.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ABI_RC.Core.EventSystem;
using ABI_RC.Core.Savior;
namespace Nocturnal
{
    internal class PlayerList : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI _textComp { get; set; }
        private GameObject _textGameObject { get; set; }
        private Image _imgComp { get; set; }
        public static PlayerList Instance { get; set; }
        private static RelationsManager _instanceInfo { get; set; }

        void Start()
        {
            Instance = this;
            this.transform.localEulerAngles = Vector3.zero;
            this.transform.localScale = Vector3.one;
            this.gameObject.AddComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            this.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            _imgComp = this.gameObject.AddComponent<Image>();
            ChangeSpriteFromString(_imgComp, Config.Instance.Js.Background).color = new Color(0, 0, 0, 0.7f);
            this.transform.localPosition = new Vector3(-0.6f, 0, 0);
            this.gameObject.AddComponent<Mask>();
            _textGameObject = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<TMPro.TextMeshProUGUI>().FirstOrDefault(x => x.name == "DisplayName").gameObject, this.transform);
            _textComp = _textGameObject.GetComponent<TMPro.TextMeshProUGUI>();
            _textGameObject.transform.parent = _textGameObject.transform;
            _textComp.enableWordWrapping = false;
            _textComp.fontSize = 3.3f;
            _textComp.color = Color.white;
            _textComp.richText = true;
            _textComp.alignment = TMPro.TextAlignmentOptions.TopLeft;
            _textComp.text = "";
            _textComp.outlineWidth = 0f;
            _textComp.outlineColor = new Color(0, 0, 0, 0);
            _textGameObject.transform.localScale = new Vector3(1.4f, 0.65f, 1);
            _textGameObject.transform.localPosition = new Vector3(187.5f, 13, 0);
            InvokeRepeating(nameof(UpdatePlayerList), -1, 1f);
            this.gameObject.SetActive(false);
            _instanceInfo = GameObject.FindObjectsOfType<RelationsManager>().FirstOrDefault();
        }
        private List<CVRPlayerEntity> _players { get; set; }
        private string _pcolor { get; set; }
        private string _friend { get; set; }
        private string _playerRank { get; set; }
        private string _talking { get; set; }
        private string _flyng { get; set; } = "";
        private string _vr { get; set; } = "";
        private string _crouch { get; set; } = "";
        private string _prone { get; set; } = "";
        void UpdatePlayerList()
        {
                _players = CVRPlayerManager.Instance.NetworkPlayers;

                switch (true)
                {
                    case true when _players.Count > 30:
                        _pcolor = "<color=#09de5e>";
                        break;
                    case true when _players.Count > 14:
                        _pcolor = "<color=#ab7200>";
                        break;
                    default:
                        _pcolor = "<color=#00730f>";
                        break;
                }
                _textComp.text = $"<color=#b3b3b3>[</color>{_pcolor + _players.Count}</color><color=#b3b3b3>]</color>{MetaPort.Instance.CurrentInstanceName}\n";
            for (int i = 0; i < _players.Count; i++)
            {
                switch (_players[i].ApiUserRank)
                {
                    case "Legend":
                        _playerRank = "<color=#fbff00>Legend</color>";
                        break;
                    case "Community Guide":
                        _playerRank = "<color=#bb00ff>Community Guide</color>";
                        break;
                    case "Moderator":
                        _playerRank = "<color=#36000f>Moderator</color>";
                        break;
                    case "Developer":
                        _playerRank = "<color=#ff0033>Developer</color>";
                        break;
                    default:
                        _playerRank = "<color=#9ea1ff>User</color>";

                        break;
                }
                switch (Main.DictionaryPlayerData[_players[i].Uuid].DeviceType.ToString())
                {
                    case "OculusQuest":
                        _vr = "<color=#26ba0f>Quest</color>";
                        break;
                    case "PCVR":
                        _vr = "<color=#0012b0>VR</color>";
                        break;
                    default:
                        _vr = "<color=#808080>PC</color>";
                        break;
                }
                _talking = _players[i].TalkerAmplitude > 0 ? $" <color=#0072b0>Talking</color>" : "";
                _flyng = Main.DictionaryPlayerData[_players[i].Uuid].AnimatorFlying ? " <color=#549bff>Flying</color>" : "";
                _crouch = Main.DictionaryPlayerData[_players[i].Uuid].AnimatorCrouching ? " <color=#ff8c00>C</color>" : "";
                _prone = Main.DictionaryPlayerData[_players[i].Uuid].AnimatorProne ? " <color=#874a00>P</color>" : "";
                _friend = ABI_RC.Core.Networking.IO.Social.Friends.List.FirstOrDefault(x => x.UserId == _players[i].Uuid) != null ? " <color=#fcff5c>Friend</color>" : "";
                _textComp.text += $"<color=#6e626c>{i}</color> <color=#c4c4c4>{_players[i].Username}</color>{_friend} {_vr} {_playerRank}{_talking}{_flyng}{_crouch}{_prone}\n";
            }
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
