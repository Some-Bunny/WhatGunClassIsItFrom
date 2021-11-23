using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.RuntimeDetour;
using MonoMod;
using System.Reflection;


namespace WhatGunClassIsItFrom
{
    public class WhatGunClassIsItFromModule : ETGModule
    {
        public static readonly string MOD_NAME = "What Gun Class Is It From";
        public static readonly string VERSION = "1.0";
        public static readonly string TEXT_COLOR = "#FFD100";

        public override void Start()
        {
            try { new Hook(typeof(EncounterDatabaseEntry).GetMethod("GetModifiedLongDescription", BindingFlags.Public | BindingFlags.Instance), typeof(WhatGunClassIsItFromModule).GetMethod("GetModifiedLongDescriptionHook")); }
            catch (Exception e)
            {
                Log($"Something went VERY wrong setting up the GetModifiedLongDescription Hook, send a screenshot of this error to the #modding channel in the Gungeon Discord.", TEXT_COLOR);
                Log(e.ToString(), TEXT_COLOR);

            }
            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static string GetModifiedLongDescriptionHook(Func<EncounterDatabaseEntry, string> orig, EncounterDatabaseEntry self)
        {
            string result = orig(self);                
            if (self.shootStyleInt >= 0)
            {
               Gun gun = PickupObjectDatabase.GetById(self.pickupObjectId) as Gun;
               if (gun!=null)
               {
                    string txt = "NULL";
                    ClassKeys.TryGetValue(gun.gunClass, out txt);
                    if (txt == "NULL" || txt==null)
                    { result += "\n\n--------------\nThis gun is not in any class at all. How strange."; }
                    else
                    { result += "\n\n--------------\nThis gun is in the " +txt +" class."; }
               }
            }
            return result;
        }

        public static Dictionary<GunClass, string> ClassKeys = new Dictionary<GunClass, string>()
        {
            {GunClass.BEAM, "BEAM" },
            {GunClass.CHARGE, "CHARGE" },
            {GunClass.CHARM, "CHARM" },
            {GunClass.FULLAUTO, "FULLAUTO" },
            {GunClass.EXPLOSIVE, "EXPLOSIVE" },
            {GunClass.FIRE, "FIRE" },
            {GunClass.ICE, "ICE" },
            {GunClass.NONE, "NONE" },
            {GunClass.PISTOL, "PISTOL" },
            {GunClass.POISON, "POISON" },
            {GunClass.RIFLE, "RIFLE" },
            {GunClass.SHITTY, "SHITTY" },
            {GunClass.SHOTGUN, "SHOTGUN" },
            {GunClass.SILLY, "SILLY" }
        };



        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
