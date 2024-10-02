using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions.Must;
using Support;
using UnityEngine.SceneManagement;
using Services;
using Model;
using Controller;
using System.IO;
using UI;
using System.Reflection;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Channels;
using Newtonsoft.Json;
using System.CodeDom;

// MOD
namespace RSM
{
    public class Loader
    {
        public static void Init()
        {
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<RocketScienceMod>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }
        private static GameObject Load;
    }



    public class RocketScienceMod : MonoBehaviour
    {
        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public void OnGUI()
        {
            // Define the button size and position
            float buttonWidth = 200;
            float buttonHeight = 50;
            float buttonX = (Screen.width / 2) - (buttonWidth / 2);
            float buttonY = (Screen.height / 2) - (buttonHeight / 2);
            // Create a button
            if(Settings.isInitialized)
            {
                Services.Game game = Services.Settings.game;
                if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Press me"))
                {
                    Controller.AssemblyShopController asc = Support.Finders.ComponentByTag<AssemblyShopController>(Tag.assemblyShopController, false);
                    var parts_field = typeof(AssemblyShopController).GetField("parts", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(asc) as AssemblyParts;
                    var pf_parts_field = typeof(AssemblyParts).GetField("parts", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(parts_field) as List<AssemblyPartItem>;
                    MethodInfo mi_add_part = typeof(AssemblyParts).GetMethod("AddPart", BindingFlags.NonPublic | BindingFlags.Instance);
                    var part = Clone(pf_parts_field.ElementAt(0).part);
                    var asm = asc.module;
                    if (part != null)
                    {
                        part.defindex = 1001;
                        part.description = "Part of Valera";
                        part.name = "Part of Valera";
                        part.iconName = "rocket_parts/command_1.png";
                        mi_add_part.Invoke(parts_field, new object[] { part });
                        var filter = asm.filter;
                        asm.SetFilter(null);
                        asm.SetFilter(filter);
                    }
                }
            }
        }
    }

}


