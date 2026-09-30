using Multiplayer.Compat;
using Verse;

namespace MultiplayerTheGrandLibraryOfMirroredPatch.Source.Mods;

/// <summary>
///     Multiplayer Patch for The Grand Library of 「Mirrored」by heimu,
///     Last Update: 24 Aug @ 9:34am 2026
///     <see href="https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656" />
///     The mod is a visual-novel/story framework. Shared simulation state lives in the
///     <c>RimMomotalk.MomoStoryProgress</c> GameComponent, advanced on a 200-tick timer by
///     <c>RimMomotalk.MomoStoryTriggerSystem.CheckTriggers</c>. Chat windows
///     (<c>RimMomotalk.Window_MomoChat</c>) are per-client UI: scenario playback timing is driven by
///     <c>Time.realtimeSinceStartup</c>, so dialogue rendering and the chat history stay local on purpose.
///     Only the trigger-relevant progress state is synced, keeping scenario unlocks deterministic
///     on every client.
///     The patch is split into focused components, see:
///     <see cref="MirroredReflection" />, <see cref="StoryProgressSync" />, <see cref="TriggerWealthPatch" />,
///     <see cref="OptionSelectionSync" />, <see cref="BranchWindowSync" />, <see cref="PlaybackQueueReset" />,
///     <see cref="TypingRandomnessPatch" />.
/// </summary>
[MpCompatFor("heimu.RimMomotalk")]
public class TheGrandLibraryOfMirrored
{
    internal const string LogPrefix = "[Multiplayer The Grand Library of 「Mirrored」Patch]";

    public TheGrandLibraryOfMirrored(ModContentPack content)
    {
        LongEventHandler.ExecuteWhenFinished(LatePatch);
    }

    private static void LatePatch()
    {
        Log.Message($"{LogPrefix} Initializing...");

        if (!MirroredReflection.ResolveProgress())
        {
            Log.Error($"{LogPrefix} Could not find type RimMomotalk.MomoStoryProgress, aborting patch.");
            return;
        }

        MirroredReflection.ResolveChatWindow();
        MirroredReflection.ResolveOptionEvents();
        MirroredReflection.ResolvePlaybackQueue();

        MpCompatPatchLoader.LoadPatch(typeof(TriggerWealthPatch));
        MpCompatPatchLoader.LoadPatch(typeof(OptionSelectionSync));
        MpCompatPatchLoader.LoadPatch(typeof(BranchWindowSync));
        MpCompatPatchLoader.LoadPatch(typeof(PlaybackQueueReset));

        StoryProgressSync.RegisterSyncMethods();
        TypingRandomnessPatch.Apply();

        Log.Message($"{LogPrefix} Initialized.");
    }
}