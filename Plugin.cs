using System;
using BepInEx;
using UnityEngine;
using Utilla;

namespace GorillaTagModTemplateProject
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.13")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool inRoom;
        private GameObject[] butts;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            if (GorillaLocomotion.Player.Instance != null)
            {
                // Create butts for all players
                GameObject playerButtPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                playerButtPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                butts = new GameObject[GorillaLocomotion.PlayerManager.playerCount];
                for (int i = 0; i < GorillaLocomotion.PlayerManager.playerCount; i++)
                {
                    butts[i] = Instantiate(playerButtPrefab);
                }
                Destroy(playerButtPrefab);
            }
        }

        void Update()
        {
            if (inRoom)
            {
                // Only render butts for the player who has the mod enabled
                for (int i = 0; i < GorillaLocomotion.PlayerManager.playerCount; i++)
                {
                    if (GorillaLocomotion.PlayerManager.GetPlayer(i).IsLocalPlayer && butts[i] != null)
                    {
                        butts[i].SetActive(true);
                    }
                    else if (butts[i] != null)
                    {
                        butts[i].SetActive(false);
                    }
                }
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
        }
    }
}
