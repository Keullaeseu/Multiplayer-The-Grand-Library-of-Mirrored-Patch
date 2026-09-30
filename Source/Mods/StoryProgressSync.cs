using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Syncs every mutation of the shared story state in
///     <c>RimMomotalk.MomoStoryProgress</c> so it executes identically on all clients.
///     MomoStoryTracker postfixes (incident/quest) and GameComponentTick already call these
///     from synced context, but dialogue option lambdas and debug actions call them from the
///     interface, hence the sync registration. MP syncs the GameComponent instance implicitly.
///     UI-only writes (chat history, unread badges, remembered contact status) are intentionally
///     left local: they are driven by per-client realtime playback and never feed trigger logic.
/// </summary>
internal static class StoryProgressSync
{
    internal static void RegisterSyncMethods()
    {
        RegisterSyncMethod("RimMomotalk.MomoStoryProgress:CompleteScenario");
        RegisterSyncMethod("RimMomotalk.MomoStoryProgress:UnlockScenario");
        RegisterSyncMethod("RimMomotalk.MomoStoryProgress:ReportIncident");
        RegisterSyncMethod("RimMomotalk.MomoStoryProgress:ReportQuestState");
        RegisterSyncMethod("RimMomotalk.MomoStoryProgress:SetMarker");
        RegisterSyncMethod("RimMomotalk.MomoStoryProgress:ActivateBranchStoryline");
    }

    private static void RegisterSyncMethod(string typeColonMethod)
    {
        var method = AccessTools.DeclaredMethod(typeColonMethod);
        if (method == null)
            Log.Error($"{TheGrandLibraryOfMirrored.LogPrefix} Could not find method {typeColonMethod} to sync.");
        else
            MP.RegisterSyncMethod(method);
    }
}