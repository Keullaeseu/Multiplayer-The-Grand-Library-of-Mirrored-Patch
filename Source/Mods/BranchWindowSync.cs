using System.Collections;
using Multiplayer.API;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Mirrors single-player branch cleanup on every client.
///     <c>RimMomotalk.MomoStoryProgress.CompleteScenario</c> is synced, so blocked storylines
///     match everywhere, but each client owns its own <c>RimMomotalk.Window_MomoChat</c>
///     instances and must close windows showing eliminated branches locally
///     (mirrors <c>RimMomotalk.DebugChatActions.CloseBlockedBranchWindows</c>).
/// </summary>
internal static class BranchWindowSync
{
    [MpCompatPostfix("RimMomotalk.MomoStoryProgress", "CompleteScenario")]
    private static void PostCompleteScenario()
    {
        if (!MP.IsInMultiplayer)
            return;

        CloseWindowsWithBlockedBranches();
    }

    private static void CloseWindowsWithBlockedBranches()
    {
        try
        {
            if (MirroredReflection.ProgressInstanceField == null
                || MirroredReflection.IsScenarioBranchBlockedMethod == null
                || MirroredReflection.ChatWindowType == null
                || MirroredReflection.CurrentScenarioNameField == null)
                return;

            var progressInstance = MirroredReflection.ProgressInstanceField.GetValue(null);
            if (progressInstance == null)
                return;

            CloseForegroundBranchWindows(progressInstance);
            RemoveBackgroundBranchWindows(progressInstance);
        }
        catch (Exception exception)
        {
            Log.Error($"{TheGrandLibraryOfMirrored.LogPrefix} Failed to close blocked branch windows: {exception}");
        }
    }

    private static bool IsBranchBlockedWindow(object progressInstance, object window)
    {
        var scenarioName = MirroredReflection.CurrentScenarioNameField.GetValue(window) as string;
        if (string.IsNullOrEmpty(scenarioName))
            return false;

        return (bool)MirroredReflection.IsScenarioBranchBlockedMethod.Invoke(progressInstance,
            new object[] { scenarioName });
    }

    private static void CloseForegroundBranchWindows(object progressInstance)
    {
        var windows = new List<Window>(Find.WindowStack.Windows);
        foreach (var window in windows)
        {
            if (!MirroredReflection.ChatWindowType.IsInstanceOfType(window))
                continue;

            if (!IsBranchBlockedWindow(progressInstance, window))
                continue;

            // Prevent Close() from re-registering unfinished playback as a background window,
            // mirroring DebugChatActions FinishQueuedScenario.
            MirroredReflection.NoBackgroundOnCloseField?.SetValue(window, true);
            window.Close();
        }
    }

    private static void RemoveBackgroundBranchWindows(object progressInstance)
    {
        // Mirror MomoBackgroundChat.RemoveBackgroundWindows: blocked background windows keep
        // playing off-screen and could otherwise complete an eliminated branch.
        var backgrounds = MirroredReflection.BackgroundWindowsField?.GetValue(null) as IList;
        if (backgrounds == null || MirroredReflection.ClearSharedDialogueStateMethod == null)
            return;

        for (var index = backgrounds.Count - 1; index >= 0; index--)
        {
            var backgroundWindow = backgrounds[index];
            if (backgroundWindow == null)
            {
                backgrounds.RemoveAt(index);
                continue;
            }

            if (!IsBranchBlockedWindow(progressInstance, backgroundWindow))
                continue;

            MirroredReflection.ClearSharedDialogueStateMethod.Invoke(backgroundWindow, null);
            backgrounds.RemoveAt(index);
        }
    }
}