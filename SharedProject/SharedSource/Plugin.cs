using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Runtime;
using Barotrauma;
using System.Runtime.CompilerServices;
using NeuroSDKCsharp;
using Microsoft.Xna.Framework;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace AgnesControl
{
    public partial class Plugin : IAssemblyPlugin
    {
        Character? agnesCharacter = null;

        public Plugin()
        {
        }

        public void Initialize()
        {
            LuaCsLogger.LogMessage("Initializing...");
            SdkSetup.Initialize("Barotrauma", "ws://localhost:8000");

            GameMain.LuaCs.Hook.Add("roundStart", "OnRoundStarted", delegate (object[] args)
            {
                LuaCsLogger.LogMessage("Got round start...");

                JobPrefab job = JobPrefab.Get("assistant");

                if (Submarine.MainSub == null)
                {
                    LuaCsLogger.LogMessage("Submarine doesnt exist yet :(");
                    return null;
                }

                CharacterInfo characterInfo = new(CharacterPrefab.HumanSpeciesName, jobOrJobPrefab: job, variant: 0, name: "Agnes Borp")
                {
                    TeamID = Submarine.MainSub.TeamID
                };

                WayPoint[] spawnpoints = WayPoint.SelectCrewSpawnPoints([characterInfo], Submarine.MainSub);

                if (spawnpoints == null || spawnpoints.Length <= 0)
                {
                    LuaCsLogger.LogMessage("Spawns couldnt be found :(");
                    return null;
                }

                WayPoint spawnpoint = spawnpoints[0];

                if (spawnpoint == null)
                {
                    LuaCsLogger.LogMessage("Spawns couldnt be found :(");
                    return null;
                }

                LuaCsLogger.LogMessage($"Got spawn location: {spawnpoint}");

                Entity.Spawner.AddCharacterToSpawnQueue(CharacterPrefab.HumanSpeciesName, spawnpoint.Position, characterInfo, onSpawn: character =>
                {
                    LuaCsLogger.LogMessage("Spawned new Character");

                    GameMain.GameSession?.CrewManager?.AddCharacter(character);

                    character.GiveJobItems(isPvPMode: GameMain.GameSession?.GameMode is PvPMode, spawnpoint);
                    character.GiveIdCardTags(spawnpoint);
                    character.Info.StartItemsGiven = true;

                    agnesCharacter = character;
                });

                return null;
            });
        }

        public void OnLoadCompleted()
        {
            // After all plugins have loaded
            // Put code that interacts with other plugins here.
        }

        public void PreInitPatching()
        {
            // Not yet supported: Called during the Barotrauma startup phase before vanilla content is loaded.
        }

        public void Dispose()
        {
            // Cleanup your plugin!
            throw new NotImplementedException();
        }
    }
}
