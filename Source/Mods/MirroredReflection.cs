using System.Reflection;
using HarmonyLib;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Shared reflection cache for the Grand Library of 「Mirrored」 assemblies.
///     All lookups are string-based so the patch never needs a compile-time reference
///     to the mod DLLs. Caches are filled in <see cref="TheGrandLibraryOfMirrored" /> late patch,
///     before any game code can run, and read by the patch components at runtime.
/// </summary>
internal static class MirroredReflection
{
    internal static Type ProgressType { get; private set; }
    internal static Type ChatWindowType { get; private set; }
    internal static Type OptionSelectedType { get; private set; }

    internal static FieldInfo ProgressInstanceField { get; private set; }
    internal static MethodInfo IsScenarioBranchBlockedMethod { get; private set; }
    internal static FieldInfo CurrentScenarioNameField { get; private set; }

    internal static MethodInfo RaiseSelectedMethod { get; private set; }
    internal static FieldInfo SelectedScenarioNameField { get; private set; }
    internal static FieldInfo SelectedContactIdField { get; private set; }
    internal static FieldInfo SelectedLabelField { get; private set; }
    internal static FieldInfo SelectedDisplayTextField { get; private set; }
    internal static FieldInfo SelectedNextNodeField { get; private set; }
    internal static FieldInfo SelectedIndexField { get; private set; }
    internal static FieldInfo SelectedIsBranchChoiceField { get; private set; }

    internal static FieldInfo PlaybackQueueField { get; private set; }
    internal static MethodInfo PlaybackQueueClearMethod { get; private set; }
    internal static FieldInfo PlaybackActiveField { get; private set; }
    internal static FieldInfo ActiveScenarioKeyField { get; private set; }

    internal static bool ResolveProgress()
    {
        ProgressType ??= AccessTools.TypeByName("RimMomotalk.MomoStoryProgress");
        return ProgressType != null;
    }

    internal static void ResolveChatWindow()
    {
        ChatWindowType = AccessTools.TypeByName("RimMomotalk.Window_MomoChat");
        if (ChatWindowType == null || ProgressType == null)
        {
            Log.Warning(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not find type RimMomotalk.Window_MomoChat, blocked branch windows will not auto-close.");
            return;
        }

        ProgressInstanceField = AccessTools.DeclaredField(ProgressType, "Instance");
        IsScenarioBranchBlockedMethod = AccessTools.DeclaredMethod(ProgressType, "IsScenarioBranchBlocked");
        CurrentScenarioNameField = AccessTools.DeclaredField(ChatWindowType, "CurrentScenarioName");

        if (ProgressInstanceField == null || IsScenarioBranchBlockedMethod == null || CurrentScenarioNameField == null)
            Log.Warning(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not cache reflection for blocked branch windows, auto-close disabled.");
    }

    internal static void ResolveOptionEvents()
    {
        var optionEventsType = AccessTools.TypeByName("RimMomotalk.MomoOptionEvents");
        OptionSelectedType = AccessTools.TypeByName("RimMomotalk.MomoOptionSelected");

        if (optionEventsType == null || OptionSelectedType == null)
        {
            Log.Warning(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not find option event types, option selection sync disabled.");
            return;
        }

        RaiseSelectedMethod = AccessTools.DeclaredMethod(optionEventsType, "RaiseSelected");
        SelectedScenarioNameField = AccessTools.DeclaredField(OptionSelectedType, "scenarioName");
        SelectedContactIdField = AccessTools.DeclaredField(OptionSelectedType, "contactId");
        SelectedLabelField = AccessTools.DeclaredField(OptionSelectedType, "label");
        SelectedDisplayTextField = AccessTools.DeclaredField(OptionSelectedType, "displayText");
        SelectedNextNodeField = AccessTools.DeclaredField(OptionSelectedType, "nextNode");
        SelectedIndexField = AccessTools.DeclaredField(OptionSelectedType, "index");
        SelectedIsBranchChoiceField = AccessTools.DeclaredField(OptionSelectedType, "isBranchChoice");

        if (RaiseSelectedMethod == null
            || SelectedScenarioNameField == null
            || SelectedContactIdField == null
            || SelectedLabelField == null
            || SelectedDisplayTextField == null
            || SelectedNextNodeField == null
            || SelectedIndexField == null
            || SelectedIsBranchChoiceField == null)
            Log.Warning(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not cache reflection for option events, option selection sync disabled.");
    }

    internal static void ResolvePlaybackQueue()
    {
        var debugChatActionsType = AccessTools.TypeByName("RimMomotalk.DebugChatActions");
        if (debugChatActionsType == null)
        {
            Log.Warning(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not find type RimMomotalk.DebugChatActions, playback queue reset disabled.");
            return;
        }

        PlaybackQueueField = AccessTools.DeclaredField(debugChatActionsType, "scenarioPlaybackQueue");
        PlaybackActiveField = AccessTools.DeclaredField(debugChatActionsType, "scenarioPlaybackActive");
        ActiveScenarioKeyField = AccessTools.DeclaredField(debugChatActionsType, "activeScenarioKey");

        if (PlaybackQueueField != null)
            PlaybackQueueClearMethod = AccessTools.Method(PlaybackQueueField.FieldType, "Clear");

        if (PlaybackQueueField == null || PlaybackQueueClearMethod == null || PlaybackActiveField == null ||
            ActiveScenarioKeyField == null)
            Log.Warning(
                $"{TheGrandLibraryOfMirrored.LogPrefix} Could not cache reflection for the scenario playback queue, reset on load disabled.");
    }
}