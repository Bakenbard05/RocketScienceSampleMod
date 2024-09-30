using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
        public Model.Spacecraft spacecraft;
        public Model.Rocket rocket;
        public Model.RocketStats rocketStats;
        public void OnGUI()
        {
            // Define the button size and position
            float buttonWidth = 200;
            float buttonHeight = 50;
            float buttonX = (Screen.width / 2) - (buttonWidth / 2);
            float buttonY = (Screen.height / 2) - (buttonHeight / 2);
            // Create a button
            if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Launch Me"))
            {
                Controller.LocalSpaceController localSpace = GameObject.FindGameObjectWithTag("LocalSpace").GetComponent<Controller.LocalSpaceController>();
                spacecraft = localSpace.selected;
                rocketStats = spacecraft.stats;
                rocket = spacecraft.rocket;
                spacecraft.SetThrottle(100.0f);
                List<Model.RocketPart> rocketParts = rocket.actionGroups.groups.Last();
                rocketParts.ForEach(part =>
                {
                    Model.EngineModule engineModule = part.GetModule<Model.EngineModule>();
                    engineModule.SetThrottle(100.0f);
                    engineModule.SetEnabled(true);
                });
            }
        }
    }

}

