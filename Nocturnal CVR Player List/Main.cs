using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using Harmony;
using ABI_RC.Core.InteractionSystem;
using UnityEngine;
using System.Collections;
using ABI_RC.Core.Savior;
using ABI_RC.Core.Player;
using System.Reflection;
using ABI_RC.Core;

namespace Nocturnal
{
    public class Main : MelonMod
    {
        private HarmonyInstance _instance = new HarmonyInstance(Guid.NewGuid().ToString());

        public static Dictionary<string,PlayerAvatarMovementData> DictionaryPlayerData = new Dictionary<string,PlayerAvatarMovementData>();
        public override void OnApplicationStart()
        {
            Patch();
            MelonCoroutines.Start(WaitForMenu());
            new Config();
        }

        private IEnumerator WaitForMenu()
        {
            while (GameObject.Find("/Cohtml") == null) yield return null;
            new GameObject("Nocturnal Player List").AddComponent<PlayerList>().transform.parent = GameObject.Find("/Cohtml/QuickMenu").transform;
            while (RootLogic.Instance == null) yield return null;
            RootLogic.Instance.comms.OnPlayerExitedRoom += UserLeft;
            yield break;
        }

        private void UserLeft(Dissonance.VoicePlayerState arg1, string arg2) =>
            DictionaryPlayerData.Remove(arg1.Name);

        private void Patch()
        {
            _instance.Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.ToggleQuickMenu)), null, typeof(Main).GetMethod(nameof(ToggleMenu), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).ToNewHarmonyMethod());
            _instance.Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.SetScale)), null, typeof(Main).GetMethod(nameof(SetScale), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).ToNewHarmonyMethod());
            _instance.Patch(typeof(PuppetMaster).GetMethod(nameof(PuppetMaster.CycleData)), null, typeof(Main).GetMethod(nameof(PuppetMasterPostFix), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).ToNewHarmonyMethod());
        }

        private static void PuppetMasterPostFix(PuppetMaster __instance)
        {
            if (DictionaryPlayerData.ContainsKey(__instance.gameObject.name)) return;
            DictionaryPlayerData.Add(__instance.gameObject.name, (PlayerAvatarMovementData)typeof(PuppetMaster).GetField("_playerAvatarMovementDataCurrent", BindingFlags.NonPublic | BindingFlags.Instance
             ).GetValue(__instance));
        }

        private static void ToggleMenu(bool __0) =>
            PlayerList.Instance.gameObject.SetActive(__0);

        private static float s_scaleFactor { get;set; }
        private static void SetScale(float __0)
        {
            s_scaleFactor = __0 / 1.8f;
            if (MetaPort.Instance.isUsingVr)
                s_scaleFactor *= 0.5f;
            PlayerList.Instance.transform.localScale = new Vector3(s_scaleFactor / 1550 + 0.0035f, s_scaleFactor / 1500 + 0.0071f, 1);
        }



    }
}
