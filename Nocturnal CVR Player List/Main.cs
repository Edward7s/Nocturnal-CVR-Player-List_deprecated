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

namespace Nocturnal
{
    public class Main : MelonMod
    {
        private HarmonyInstance _instance = new HarmonyInstance(Guid.NewGuid().ToString());
        public override void OnApplicationStart()
        {
            Patch();
            MelonCoroutines.Start(WaitForMenu());
            new Config();
        }

        private IEnumerator WaitForMenu()
        {
            while (GameObject.Find("/Cohtml") == null) yield return null;
            s_menu = new GameObject("Nocturnal Player List");
            s_menu.AddComponent<PlayerList>().transform.parent = GameObject.Find("/Cohtml/QuickMenu").transform;
            yield break;
        }

        private void Patch()
        {
            _instance.Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.ToggleQuickMenu)), null, typeof(Main).GetMethod(nameof(ToggleMenu), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).ToNewHarmonyMethod());
            _instance.Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.SetScale)), null, typeof(Main).GetMethod(nameof(SetScale), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).ToNewHarmonyMethod());

        }

        private static void ToggleMenu(bool __0) =>
            s_menu.SetActive(__0);


        private static void SetScale(float __0)
        {
            s_scaleFactor = __0 / 1.8f;
            if (MetaPort.Instance.isUsingVr)
                s_scaleFactor *= 0.5f;
            s_menu.transform.localScale = new Vector3(s_scaleFactor / 1500 + 0.0071f, s_scaleFactor / 1500 + 0.0071f, 1);
        }



    }
}
