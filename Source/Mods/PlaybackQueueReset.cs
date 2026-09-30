using Multiplayer.Compat;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Clears the trigger scenario queue when the game changes.
///     <c>RimMomotalk.MomoBackgroundChat.ResetForLoadedGame</c> already clears shared chat state,
///     but the playback queue in <c>RimMomotalk.DebugChatActions</c> is static and would
///     otherwise leak pending scenarios into the newly loaded game.
/// </summary>
internal static class PlaybackQueueReset
{
    [MpCompatPostfix("RimMomotalk.MomoBackgroundChat", "ResetForLoadedGame")]
    private static void PostResetForLoadedGame()
    {
        try
        {
            var queue = MirroredReflection.PlaybackQueueField?.GetValue(null);
            if (queue != null)
                MirroredReflection.PlaybackQueueClearMethod?.Invoke(queue, null);
            MirroredReflection.PlaybackActiveField?.SetValue(null, false);
            MirroredReflection.ActiveScenarioKeyField?.SetValue(null, null);
        }
        catch (Exception exception)
        {
            Log.Error($"{TheGrandLibraryOfMirrored.LogPrefix} Failed to reset scenario playback queue: {exception}");
        }
    }
}