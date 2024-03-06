using GameNetcodeStuff;
using HarmonyLib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Unity.Netcode;
using Unity.Collections;

namespace FastClimbingV2.Configs
{

    [Serializable]
    [HarmonyPatch]
    public class ConfigSync
    {
        public static ConfigSync defaultConfig;

        public static ConfigSync Instance;

        public static PlayerControllerB localPlayerController;

        public static bool isSynced = false;

        public float climbSpeedMultiplier = 1.8f;


        public static void BuildDefaultConfigSync()
        {
            Instance = new ConfigSync();
        }

        public static void BuildServerConfigSync()
        {
            if (Instance == null)
            {
                defaultConfig = new ConfigSync();
                defaultConfig.climbSpeedMultiplier = FastClimbingConfigs.CLIMB_SPEED_MULTIPLIER.Value;
                Instance = defaultConfig;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "ConnectClientToPlayerObject")]
        [HarmonyPostfix]
        public static void InitializeLocalPlayer(PlayerControllerB __instance)
        {
            localPlayerController = __instance;
            if (NetworkManager.Singleton.IsServer)
            {
                BuildServerConfigSync();
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("BetterStamina-OnRequestConfigSync", OnReceiveConfigSyncRequest);
                OnLocalClientConfigSync();
            }
            else
            {
                isSynced = false;
                BuildDefaultConfigSync();
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("BetterStamina-OnReceiveConfigSync", OnReceiveConfigSync);
                RequestConfigSync();
            }
        }

        public static void RequestConfigSync()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                FastClimbingV2.Logger.LogDebug("Sending config sync request to server.");
                FastBufferWriter messageStream = new FastBufferWriter(4, Allocator.Temp);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("BetterStamina-OnRequestConfigSync", 0uL, messageStream);
            }
            else
            {
                FastClimbingV2.Logger.LogError("Failed to send config sync request.");
            }
        }

        public static void OnReceiveConfigSyncRequest(ulong clientId, FastBufferReader reader)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                FastClimbingV2.Logger.LogDebug("Receiving config sync request from client with id: " + clientId + ". Sending config sync to client.");
                byte[] array = SerializeConfigToByteArray(Instance);
                FastBufferWriter messageStream = new FastBufferWriter(array.Length + 4, Allocator.Temp);
                int value = array.Length;
                messageStream.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
                messageStream.WriteBytesSafe(array);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("BetterStamina-OnReceiveConfigSync", clientId, messageStream);
            }
        }

        public static void OnReceiveConfigSync(ulong clientId, FastBufferReader reader)
        {
            if (reader.TryBeginRead(4))
            {
                reader.ReadValueSafe(out int value, default(FastBufferWriter.ForPrimitives));
                if (reader.TryBeginRead(value))
                {
                    FastClimbingV2.Logger.LogDebug("Receiving config sync from server.");
                    byte[] value2 = new byte[value];
                    reader.ReadBytesSafe(ref value2, value);
                    Instance = DeserializeFromByteArray(value2);
                    OnLocalClientConfigSync();
                }
                else
                {
                    FastClimbingV2.Logger.LogError("Error receiving sync from server.");
                }
            }
            else
            {
                FastClimbingV2.Logger.LogError("Error receiving bytes length.");
            }
        }

        public static void OnLocalClientConfigSync()
        {
            //need think of smth
            isSynced = true;
        }

        public static byte[] SerializeConfigToByteArray(ConfigSync config)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, config);
            return memoryStream.ToArray();
        }

        public static ConfigSync DeserializeFromByteArray(byte[] data)
        {
            MemoryStream serializationStream = new MemoryStream(data);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (ConfigSync)binaryFormatter.Deserialize(serializationStream);
        }
    }
}