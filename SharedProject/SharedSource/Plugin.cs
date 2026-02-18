using System.Runtime.CompilerServices;
using Barotrauma;
using NeuroSDKCsharp;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace NeuroSauma;

/// <summary>
/// Plugin to integrate NeuroSamaSDK with Barotrauma.
/// </summary>
public partial class Plugin() : IAssemblyPlugin
{
    private const string CHARACTER_NAME = "Agnes Borp";

    private Character? _aiAgent;

    /// <summary>
    /// Called when the mod is initialised.
    /// </summary>
    public void Initialize()
    {
        LuaCsLogger.LogMessage("Initializing...");
        SdkSetup.Initialize("Barotrauma", "ws://localhost:8000");

        // OnRoundStart
        GameMain.LuaCs.Hook.Add("roundStart", "OnRoundStarted", delegate (object[] args)
        {
            LuaCsLogger.LogMessage("Round Started");

            // Get crew manager
            var crewManager = GameMain.GameSession.CrewManager;
            if (crewManager is null)
            {
                LuaCsLogger.LogError("Unable to get CrewManager");
                return null;
            }

            // Create character
            var job = JobPrefab.Get("assistant");
            CharacterInfo characterInfo = new(CharacterPrefab.HumanSpeciesName, jobOrJobPrefab: job, variant: 0, name: CHARACTER_NAME)
            {
                TeamID = Submarine.MainSub.TeamID
            };

            // Prepare crew manager
            crewManager.AddCharacterInfo(characterInfo);
            crewManager.HasBots = true;

            // Get spawnpoint
            var spawnpoint = WayPoint.SelectCrewSpawnPoints([characterInfo], Submarine.MainSub).FirstOrDefault();
            if (spawnpoint is null)
            {
                LuaCsLogger.LogError("Unable to find spawnpoint for NPC");
                return null;
            }

            // Spawn character
            _aiAgent = Character.Create(characterInfo, spawnpoint.WorldPosition, characterInfo.Name, isRemotePlayer: false, hasAi: true);
            _aiAgent.AIController.Enabled = false;
            _aiAgent.GiveJobItems(GameMain.GameSession.GameMode is PvPMode, spawnpoint);
            _aiAgent.GiveIdCardTags(spawnpoint);
            _aiAgent.LoadTalents();
            crewManager.AddCharacter(_aiAgent);

            return null;
        });
    }

    /// <summary>
    /// Called after all plugins have loaded.
    /// </summary>
    public void OnLoadCompleted()
    {
    }

    /// <summary>
    /// Called during the Barotrauma setup phase before vanilla content is loaded.
    /// </summary>
    public void PreInitPatching()
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _aiAgent?.Remove();
        GameMain.LuaCs.Hook.Remove("roundStart", "OnRoundStarted");
        GC.SuppressFinalize(this);
    }
}
