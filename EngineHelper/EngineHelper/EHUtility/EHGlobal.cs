using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyStorage;
using Microsoft.Xna.Framework.GamerServices;
using EngineHelper;

namespace EngineHelper.EHUtility
{
    public class EHGlobal
    {
        // A generic EasyStorage save device
        public static IAsyncSaveDevice SaveDevice;

        //We can set up different file names for different things we may save.
        //In this example we're going to save the items in the 'Options' menu.
        //I listed some other examples below but commented them out since we
        //don't need them. YOU CAN HAVE MULTIPLE OF THESE
        //public static string fileName_options = "YourGame_Options";
        public static string fileName_game = "EH_Game";
        public static string fileName_Levels = "EH_Levels";
        //public static string fileName_awards = "YourGame_Awards";

        //This is the name of the save file you'll find if you go into your memory
        //options on the Xbox. If you name it something like 'MyGameSave' then
        //people will have no idea what it's for and might delete your save.
        //YOU SHOULD ONLY HAVE ONE OF THESE
        public static string containerName = "EH_Save";
    }
}
