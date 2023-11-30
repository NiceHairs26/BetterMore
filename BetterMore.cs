using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetterMore
{
    [BepInPlugin(GUID, Name, "1.0.0")]
    public class BetterMore : BaseUnityPlugin
    {
        public const string GUID = "niceh.BetterMore";
        public const string Name = "BetterMore";

        public static BetterMore Instance { get; private set; }
        public static ManualLogSource Log { get; private set; }
        private void Awake()
        {

            Instance = this;
            Log = Logger;


            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll();
        }
    }
}
