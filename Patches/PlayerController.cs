using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Linq;

namespace BetterMore.Patches
{
    [HarmonyPatch(typeof(StartOfRound), "WritePlayerNotes")]
    internal static class WritePlayerNotes
    {
        static void Prefix(StartOfRound __instance)
        {
            foreach (var player in __instance.allPlayerObjects.Select(x => x.GetComponent<PlayerControllerB>()))
            {
                try
                {
                    if (!player.isPlayerDead) continue;

                    CauseOfDeathRandom = new Random((int)player.playerClientId + __instance.randomMapSeed);
                    string deathNote = GetDeathNote(player.causeOfDeath);

                    if (deathNote.IsNullOrWhiteSpace()) continue;

                    __instance.gameStats
                        .allPlayerStats[player.playerClientId]
                        .playerNotes
                        .Add(deathNote);
                }
                catch (Exception e)
                {
                    BetterMore.Log.LogError(e);
                }
            }

        }
        private static string GetDeathNote(CauseOfDeath causeOfDeath)
        {

            switch (causeOfDeath)
            {
                case CauseOfDeath.Bludgeoning:
                    return RandomT(
                        "Beaten up to a pulp.",
                        "Pummeled into oblivion.",
                        "Battered beyond recognition."
                        );
                case CauseOfDeath.Gravity:
                    return RandomT(
                        "Forgot gravity existed."
                        );
                case CauseOfDeath.Blast:
                    return RandomT(
                        "Reduced to smithereens.",
                        "Evaporated in the boom.",
                        "Went out with a bang."
                        );
                case CauseOfDeath.Strangulation:
                    return RandomT(
                        "Ambushed from behind.",
                        "Caught off guard."
                        );
                case CauseOfDeath.Suffocation:
                    return RandomT(
                        "Forgot how to breathe.",
                        "Ran out of airtime."
                        );
                case CauseOfDeath.Mauling:
                    return RandomT(
                        "Turned into ground beef.",
                        "Ripped to shreds.",
                        "Mangled beyond repair.",
                        "Shredded to bits.",
                        "Disassembled violently."
                        );
                case CauseOfDeath.Gunshots:
                    return RandomT(
                        "Took one for the team.",
                        "Bullet's new best friend.",
                        "Caught the deadly rain."
                        );
                case CauseOfDeath.Crushing:
                    return RandomT(
                        "Squashed flat involuntary.",
                        "Compressed beyond limits."
                        );
                case CauseOfDeath.Drowning:
                    return RandomT(
                        "Turned into fish food.",
                        "Forgot to bring a snorkel.",
                        "Swam with the fishes."
                        );
                case CauseOfDeath.Abandoned:
                    {
                        Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
                        string levelName = terminal.moonsCatalogueList[RoundManager.Instance.currentLevel.levelID].PlanetName;
                        return $"Still left on {levelName}";
                    }
                case CauseOfDeath.Electrocution:
                    return RandomT(
                        "The most conductive employee."
                        );
                default:
                    return string.Empty;
            }

        }
        private static Random CauseOfDeathRandom { get; set; }
        private static T RandomT<T>(params T[] tees)
        {
            return tees[CauseOfDeathRandom.Next(tees.Length)];
        }

    }
}
