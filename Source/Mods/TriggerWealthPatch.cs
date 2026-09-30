using Multiplayer.API;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Makes the wealth trigger condition deterministic in multiplayer.
///     <c>RimMomotalk.MomoStoryTriggerSystem.GetColonyWealth</c> reads
///     <c>Find.CurrentMap</c>, which is the locally selected map and may differ between clients,
///     so wealth triggers could unlock scenarios on one client but not another.
/// </summary>
internal static class TriggerWealthPatch
{
    [MpCompatPrefix("RimMomotalk.MomoStoryTriggerSystem", "GetColonyWealth")]
    private static bool PreGetColonyWealth(ref double __result)
    {
        if (!MP.IsInMultiplayer)
            return true;

        // Use the first player home map so every client computes the same wealth.
        var homeMap = Find.AnyPlayerHomeMap;
        if (homeMap == null || homeMap.wealthWatcher == null)
            __result = 0d;
        else
            __result = homeMap.wealthWatcher.WealthTotal;

        return false;
    }
}