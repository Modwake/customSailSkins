using System;
using System.IO;
using System.Collections.Generic;
using Harmony;
using BWModLoader;
using UnityEngine;

namespace customSailSkins
{
    [Mod]
    public class Main : MonoBehaviour
    {
        static string texturesFilePath = "/Managed/Mods/Assets/customSails/";
        static Dictionary<string, Texture2D> sailSkins = new Dictionary<string, Texture2D>();
        static Texture defaultSails;
        static bool setDefault = false;
        static string configFilePath = "/Managed/Mods/Configs/customSailSkins.cfg";
        static void debugLog(string message)
        {
            //Just easier to type than Log.logger.Log
            //Will always log, so only use in try{} catch(Exception e) {} when absolutely needed
            Log.logger.Log(message);
        }

        void Start()
        {
            //Setup harmony patching
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.archie");
            harmony.PatchAll();

            initFiles();
        }

        void initFiles()
        {
            if (!File.Exists(Application.dataPath + configFilePath))
            {
                Directory.CreateDirectory(Application.dataPath + "/Managed/Mods/Configs/");
                StreamWriter streamWriter = new StreamWriter(Application.dataPath + configFilePath);
                streamWriter.WriteLine("STEAMID64HERE=SAILNAMEHERE");
                streamWriter.Close();
            }

            if (!Directory.Exists(Application.dataPath + texturesFilePath))
            {
                Directory.CreateDirectory(Application.dataPath + texturesFilePath);
            }

            initSails();
        }
        void initSails()
        {
            Texture2D sail;
            string[] array = File.ReadAllLines(Application.dataPath + configFilePath);
            for (int i = 0; i <= array.Length; i++)
            {
                string[] contents = array[i].Split('=');
                sail = loadTexture(contents[1]);
                sailSkins.Add(contents[0], sail);
            }
        }

        public static Texture2D loadTexture(string texName)
        {
            try
            {
                byte[] data = File.ReadAllBytes(Application.dataPath + texturesFilePath + texName + ".png");
                Texture2D tex = new Texture2D(1024, 512, TextureFormat.RGB24, false);
                tex.LoadImage(data);
                tex.name = texName;
                return tex;
            }
            catch (Exception e)
            {
                debugLog(string.Format("Error loading texture {0}", texName));
                debugLog(e.Message);
                Texture2D tex = Texture2D.whiteTexture;
                tex.name = "NOFLAG";
                // Return default white texture on failing to load
                return tex;
            }
        }

        [HarmonyPatch(typeof(SailHealth), "OnEnable")]
        static class sailSkinPatch
        {
            static void Postfix(SailHealth __instance)
            {
                try
                {
                    Transform shipTransf = __instance.transform.root;
                    if (shipTransf)
                    {
                        int teamNum = int.Parse(shipTransf.name.Split('m')[1]);
                        string steamID = GameMode.Instance.teamCaptains[teamNum - 1].steamID.ToString();
                        if (!setDefault)
                        {
                            defaultSails = __instance.GetComponent<Renderer>().material.mainTexture;
                            setDefault = true;
                        }
                        if (sailSkins.TryGetValue(steamID, out Texture2D newTex))
                        {
                            if (newTex.name != "NOFLAG")
                            {
                                __instance.GetComponent<Renderer>().material.mainTexture = newTex;
                            }
                        }
                        else
                        {
                            __instance.GetComponent<Renderer>().material.mainTexture = defaultSails;
                        }
                    }
                }
                catch (Exception e)
                {
                    debugLog(e.Message);
                }
            }
        }

    }
}
