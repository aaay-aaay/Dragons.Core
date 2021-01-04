using BepInEx;
using Mono.Cecil;
using System.IO;
using BepInEx.Logging;
using System.Collections.Generic;
using BepInEx.Preloader;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System;
using BepInEx.Configuration;
using BepInEx.MonoMod.Loader;

namespace Dragons.Core
{
    public static class Patcher
    {
        static Patcher()
        {
            logger.LogWarning("Here be dragons!");
        }
        
        public static void Initialize()
        {
            logger.LogInfo("Finding dragons...");
            if (initialized)
            {
                logger.LogInfo("Dragons already found!");
                return;
            }
            else
            {
                Type AssemblyPatcher = typeof(EnvVars).Assembly.GetType("BepInEx.Preloader.Patching.AssemblyPatcher");
                
                initialized = true;
                AssemblyPatcher.GetMethod("PatchAndLoad").Invoke(null, new string[] {Path.Combine(Paths.BepInExRootPath, "plugins")});
                logger.LogInfo("Dragons found (plugins)");
                
                ConfigEntry<bool> ConfigDumpAssemblies = (ConfigEntry<bool>)AssemblyPatcher.GetField("ConfigDumpAssemblies", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                bool origDumpAsm = ConfigDumpAssemblies.Value;
                ConfigDumpAssemblies.Value = true;
                
                AssemblyPatcher.GetMethod("PatchAndLoad").Invoke(null, new string[] {Path.Combine(Paths.BepInExRootPath, "monomod")});
                logger.LogInfo("Dragons found (monomod)");
                
                ConfigDumpAssemblies.Value = origDumpAsm;
                
                // new Hook(typeof(Mono.Cecil.ModuleDefinition).GetMethod("ReadModule", new Type[] {typeof(string), typeof(ReaderParameters)}), typeof(Patcher).GetMethod("CecilDetour"));
            }
        }
        
        /*
        public static ModuleDefinition CecilDetour(CecilDetourOrig orig, string fileName, ReaderParameters parameters)
        {
            ModuleDefinition realModule = orig(fileName, parameters);
            if (realModule.Assembly == null) return realModule;
            
            if (Path.GetDirectoryName(fileName) == Path.Combine(Paths.BepInExRootPath, "monomod") || Path.GetDirectoryName(fileName) == Path.Combine(Paths.BepInExRootPath, "plugins"))
            {
                fileName = Path.Combine(Path.Combine(Paths.BepInExRootPath, "DumpedAssemblies"), Path.GetFileName(fileName));
                File.AppendAllText(Path.Combine(Paths.BepInExRootPath, "dragonLog.txt"), "Considered " + fileName + "\n");
            }
            
            return orig(fileName, parameters);
        }
        */
        
        public static void Patch(AssemblyDefinition assembly)
        {
        }
        
        public static IEnumerable<string> TargetDLLs
        {
            get
            {
                return new List<string>();
            }
        }
        
        public static ManualLogSource logger = Logger.CreateLogSource("Dragons");
        public static bool initialized;
        
        // public delegate ModuleDefinition CecilDetourOrig(string fileName, ReaderParameters parameters);
        public delegate void PerformPatchesOrig(string modDirectory);
        
        
        public static string updateURL = "http://beestuff.pythonanywhere.com/audb/api/mods/0/20";
        public static int version = 0;
        public static string keyE = "AQAB";
        public static string keyN = "yu7XMmICrzuavyZRGWoknFIbJX4N4zh3mFPOyfzmQkil2axVIyWx5ogCdQ3OTdSZ0xpQ3yiZ7zqbguLu+UWZMfLOBKQZOs52A9OyzeYm7iMALmcLWo6OdndcMc1Uc4ZdVtK1CRoPeUVUhdBfk2xwjx+CvZUlQZ26N1MZVV0nq54IOEJzC9qQnVNgeeHxO1lRUTdg5ZyYb7I2BhHfpDWyTvUp6d5m6+HPKoalC4OZSfmIjRAi5UVDXNRWn05zeT+3BJ2GbKttwvoEa6zrkVuFfOOe9eOAWO3thXmq9vJLeF36xCYbUJMkGR2M5kDySfvoC7pzbzyZ204rXYpxxXyWPP5CaaZFP93iprZXlSO3XfIWwws+R1QHB6bv5chKxTZmy/Imo4M3kNLo5B2NR/ZPWbJqjew3ytj0A+2j/RVwV9CIwPlN4P50uwFm+Mr0OF2GZ6vU0s/WM7rE78+8Wwbgcw6rTReKhVezkCCtOdPkBIOYv3qmLK2S71NPN2ulhMHD9oj4t0uidgz8pNGtmygHAm45m2zeJOhs5Q/YDsTv5P7xD19yfVcn5uHpSzRIJwH5/DU1+aiSAIRMpwhF4XTUw73+pBujdghZdbdqe2CL1juw7XCa+XfJNtsUYrg+jPaCEUsbMuNxdFbvS0Jleiu3C8KPNKDQaZ7QQMnEJXeusdU=";
        // ------------------------------------------------
    }
}