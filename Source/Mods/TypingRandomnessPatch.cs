using HarmonyLib;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Isolates interface randomness from shared RNG.
///     <c>RimMomotalk.Window_MomoChat.StartTyping</c> draws the typing-indicator duration from
///     Verse.Rand while running in interface/background-update context.
///     The WindowOnGUI shake uses UnityEngine.Random, which does not touch sim RNG,
///     so it is intentionally left alone.
/// </summary>
internal static class TypingRandomnessPatch
{
    internal static void Apply()
    {
        var startTyping = AccessTools.DeclaredMethod("RimMomotalk.Window_MomoChat:StartTyping");
        if (startTyping == null)
            Log.Error(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not find method RimMomotalk.Window_MomoChat:StartTyping for RNG patch.");
        else
            PatchingUtilities.PatchPushPopRand(startTyping);
    }
}