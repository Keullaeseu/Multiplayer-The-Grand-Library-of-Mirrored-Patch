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

            var windows = new List<Window>(Find.WindowStack.Windows);
            foreach (var window in windows)
            {
                if (!MirroredReflection.ChatWindowType.IsInstanceOfType(window))
                    continue;

                var scenarioName = MirroredReflection.CurrentScenarioNameField.GetValue(window) as string;
                if (string.IsNullOrEmpty(scenarioName))
                    continue;

                var blocked =
                    (bool)MirroredReflection.IsScenarioBranchBlockedMethod.Invoke(progressInstance,
                        new object[] { scenarioName });
                if (blocked)
                    window.Close();
            }
        }
        catch (Exception exception)
        {
            Log.Error($"{TheGrandLibraryOfMirrored.LogPrefix} Failed to close blocked branch windows: {exception}");
        }
    }
}