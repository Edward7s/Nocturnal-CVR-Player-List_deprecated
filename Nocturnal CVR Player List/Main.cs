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
        }

        private IEnumerator WaitForMenu()
        {
            while (GameObject.Find("/Cohtml") == null) yield return null;
            new GameObject("Nocturnal Player List").AddComponent<PlayerList>().transform.parent = GameObject.Find("/Cohtml").transform;
            yield break;
        }

        private void Patch() =>
            _instance.Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.ToggleQuickMenu)), null, typeof(Main).GetMethod(nameof(ToggleMenu), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).ToNewHarmonyMethod());

        


        private static void ToggleMenu(bool __0)
        {
           MelonLogger.Msg(__0);

        }
    }
}
