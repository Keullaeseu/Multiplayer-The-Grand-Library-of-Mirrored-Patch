using Multiplayer.API;
using Multiplayer.Compat;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Relays dialogue option selections to every client.
///     The selection event bus (<c>RimMomotalk.MomoOptionEvents</c>) is public API: external
///     storyline mods may subscribe and apply game effects. The selection payload is plain
///     strings/ints/bools, so it is relayed through a synced method carrying primitives
///     instead of syncing the <c>MomoOptionSelected</c> DTO itself.
///     Local dialogue advancement (option.onSelected) still runs only on the clicking client
///     via <c>RimMomotalk.Window_MomoChat.OnOptionSelected</c>.
/// </summary>
internal static class OptionSelectionSync
{
    [MpCompatPrefix("RimMomotalk.MomoOptionEvents", "RaiseSelected")]
    private static bool PreRaiseSelected(object info)
    {
        if (!MP.IsInMultiplayer || MP.IsExecutingSyncCommand)
            return true;

        try
        {
            if (info == null || MirroredReflection.OptionSelectedType == null ||
                !MirroredReflection.OptionSelectedType.IsInstanceOfType(info))
                return true;

            SyncedOptionSelected(
                MirroredReflection.SelectedScenarioNameField?.GetValue(info) as string,
                MirroredReflection.SelectedContactIdField?.GetValue(info) as string,
                MirroredReflection.SelectedLabelField?.GetValue(info) as string,
                MirroredReflection.SelectedDisplayTextField?.GetValue(info) as string,
                MirroredReflection.SelectedNextNodeField?.GetValue(info) as string,
                MirroredReflection.SelectedIndexField == null
                    ? 0
                    : (int)MirroredReflection.SelectedIndexField.GetValue(info),
                MirroredReflection.SelectedIsBranchChoiceField != null &&
                (bool)MirroredReflection.SelectedIsBranchChoiceField.GetValue(info));
        }
        catch (Exception exception)
        {
            Log.Error(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Failed to sync option selection, running locally: {exception}");
            return true;
        }

        return false;
    }

    [MpCompatSyncMethod]
    private static void SyncedOptionSelected(string scenarioName, string contactId, string label, string displayText,
        string nextNode, int index, bool isBranchChoice)
    {
        try
        {
            if (MirroredReflection.OptionSelectedType == null || MirroredReflection.RaiseSelectedMethod == null)
            {
                Log.Error(
                    $"{TheGrandLibraryOfMirrored.LogPrefix} Cannot replay synced option selection, reflection cache is missing.");
                return;
            }

            var selected = Activator.CreateInstance(MirroredReflection.OptionSelectedType);
            MirroredReflection.SelectedScenarioNameField?.SetValue(selected, scenarioName);
            MirroredReflection.SelectedContactIdField?.SetValue(selected, contactId);
            MirroredReflection.SelectedLabelField?.SetValue(selected, label);
            MirroredReflection.SelectedDisplayTextField?.SetValue(selected, displayText);
            MirroredReflection.SelectedNextNodeField?.SetValue(selected, nextNode);
            MirroredReflection.SelectedIndexField?.SetValue(selected, index);
            MirroredReflection.SelectedIsBranchChoiceField?.SetValue(selected, isBranchChoice);

            MirroredReflection.RaiseSelectedMethod.Invoke(null, new[] { selected });
        }
        catch (Exception exception)
        {
            Log.Error($"{TheGrandLibraryOfMirrored.LogPrefix} Failed to replay synced option selection: {exception}");
        }
    }

    /// <summary>
    ///     Clears the stale last selection when the game changes so it never leaks
    ///     into the newly loaded game. Subscriber delegates belong to other mods
    ///     and are intentionally left alone.
    /// </summary>
    internal static void ClearLastSelected()
    {
        try
        {
            MirroredReflection.LastSelectedField?.SetValue(null, null);
        }
        catch (Exception exception)
        {
            Log.Error($"{TheGrandLibraryOfMirrored.LogPrefix} Failed to clear last option selection: {exception}");
        }
    }
}