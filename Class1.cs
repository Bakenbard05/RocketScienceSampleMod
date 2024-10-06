using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions.Must;
using Services;
using Controller;
using System.IO;
using UI;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

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
        Task r;
        Task s;
        public async void Load()
        {
            var path = Path.Combine(Support.Constants.StreamingAssetsDir(), "RSPart", "catalog.json");
            try
            {
                await AddressableHelper.LoadContentCatalog(path);
            } catch(System.Exception e)
            {
                Helpers.Exception("Cant load rspart catalog", e);
            }
        }

        public IEnumerable<AsyncOperationHandle<IList<IResourceLocation>>> LoadLocation(string path)
        {
            AsyncOperationHandle<IList<IResourceLocation>> t = Addressables.LoadResourceLocationsAsync(path);
            yield return t;
        }
        public async void SLoad(string path)
        {
            var sprite = await AddressableHelper.LoadAssetAsync<Sprite>(path);
            if(sprite != null)
            {
                Services.Console.Write(sprite.name);
            }
            var sprite2 = await AddressableHelper.LoadAssetAsync<Image>(path);
            if (sprite2 != null)
            {
                Services.Console.Write(sprite2.name);
            }
            var sprite3 = await AddressableHelper.LoadAssetAsync<Texture2D>(path);
            if (sprite3 != null)
            {
                Services.Console.Write(sprite3.name);
            }
        }
        public void GetLocation(string path)
        {
            var t = LoadLocation(path);
            foreach (var o in t.ToList())
            {
                if(o.Result.ToList().Count > 0)
                {
                    Services.Console.Write(o.Result.ToList()[0].InternalId);
                }
            }
        }
        public void Start()
        {
            Load();
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
                        part.iconName = "RSPart/icon.png";
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


