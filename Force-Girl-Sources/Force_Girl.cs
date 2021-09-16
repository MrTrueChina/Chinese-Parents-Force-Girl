using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityModManagerNet;
using System.Reflection;

namespace MtC.Mod.ChineseParents.ForceGirl
{
    public static class Main
    {
        /// <summary>
        /// Mod 对象
        /// </summary>
        public static UnityModManager.ModEntry ModEntry { get; set; }

        /// <summary>
        /// 这个 Mod 是否启动
        /// </summary>
        public static bool enabled;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            // 保存 Mod 对象
            ModEntry = modEntry;
            ModEntry.OnToggle = OnToggle;

            // 加载 Harmony
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();

            modEntry.Logger.Log("强制生女儿 Mod 加载完成");

            // 返回加载成功
            return true;
        }

        /// <summary>
        /// Mod Manager 对 Mod 进行控制的时候会调用这个方法
        /// </summary>
        /// <param name="modEntry"></param>
        /// <param name="value">这个 Mod 是否激活</param>
        /// <returns></returns>
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            // 将 Mod Manager 切换的状态保存下来
            enabled = value;

            // 返回 true 表示这个 Mod 切换到 Mod Manager 切换的状态，返回 false 表示 Mod 依然保持原来的状态
            return true;
        }
    }

    /// <summary>
    /// 开新周目时生成数据的方法，里面有制作组说过的二周目性别和一周目相反的代码
    /// </summary>
    [HarmonyPatch(typeof(end_panel_info), "reset_record")]
    public static class end_panel_info_reset_record
    {
        private static void Postfix()
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return;
            }

            Main.ModEntry.Logger.Log("生成新周目玩家数据方法调用完毕");

            // 如果随机出来的就是女儿则不需要处理
            if (record_manager.InstanceManagerRecord.CurrentRecord.playerSex == 2)
            {
                Main.ModEntry.Logger.Log("新档已经是女儿，不进行处理");
                return;
            }

            Main.ModEntry.Logger.Log("新档不是女儿，进行处理");

            // 反编译出的代码，目测是所有可能登场的朋友的列表，男女根据需要使用一部分
            List<int> idlist = ReadXml.GetIDList("friend", null, null);

            // 循环是反编译出来的，清除儿子使用的朋友列表
            foreach (int num2 in idlist)
            {
                XmlData data = ReadXml.GetData("friend", num2);
                int @int = data.GetInt("sex");
                if (@int == 0 || @int == record_manager.InstanceManagerRecord.CurrentRecord.playerSex)
                {
                    record_manager.InstanceManagerRecord.CurrentRecord.FriendlistDictionary.Remove(num2);
                }
            }

            // 性别直接设为 2，即女儿
            record_manager.InstanceManagerRecord.CurrentRecord.playerSex = 2;

            // 反编译出的代码，应该是设置背景的
            record_manager.InstanceManagerRecord.CurrentRecord.backgroundIndex = DEF.BackgroundIndex[record_manager.InstanceManagerRecord.CurrentRecord.playerSex - 1, Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f))];

            // 把女儿的朋友加入列表
            foreach (int num2 in idlist)
            {
                XmlData data = ReadXml.GetData("friend", num2);
                int @int = data.GetInt("sex");
                if (@int == 0 || @int == record_manager.InstanceManagerRecord.CurrentRecord.playerSex)
                {
                    record_manager.InstanceManagerRecord.CurrentRecord.FriendlistDictionary.Add(num2, data.GetInt("init"));
                }
            }
        }
    }

    /// <summary>
    /// 在开新档时生成新的玩家数据的方法
    /// </summary>
    [HarmonyPatch(typeof(record_manager), "create_new")]
    public static class record_manager_create_new
    {
        private static void Postfix(ref record __result)
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return;
            }

            Main.ModEntry.Logger.Log("开新档时生成玩家数据方法调用完毕");

            // 如果随机出来的就是女儿则不需要处理
            if(__result.playerSex == 2)
            {
                Main.ModEntry.Logger.Log("新档已经是女儿，不进行处理");
                return;
            }

            Main.ModEntry.Logger.Log("新档不是女儿，进行处理");

            // 反编译出的代码，目测是所有可能登场的朋友的列表，男女根据需要使用一部分
            List<int> idlist = ReadXml.GetIDList("friend", null, null);

            // 循环是反编译出来的，清除儿子使用的朋友列表
            foreach (int num in idlist)
            {
                XmlData data = ReadXml.GetData("friend", num);
                int @int = data.GetInt("sex");
                if (@int == 0 || @int == __result.playerSex)
                {
                    // 反编译代码中的添加进朋友列表步骤
                    //__result.FriendlistDictionary.Add(num, data.GetInt("init"));

                    // 这里使用移除，移除儿子使用的朋友
                    __result.FriendlistDictionary.Remove(num);
                }
            }

            // 性别直接设为 2，即女儿
            __result.playerSex = 2;

            // 反编译出的代码，应该是设置背景的
            __result.backgroundIndex = DEF.BackgroundIndex[__result.playerSex - 1, Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f))];

            // 把女儿的朋友加入列表
            foreach (int num in idlist)
            {
                XmlData data = ReadXml.GetData("friend", num);
                int @int = data.GetInt("sex");
                if (@int == 0 || @int == __result.playerSex)
                {
                    __result.FriendlistDictionary.Add(num, data.GetInt("init"));
                }
            }
        }
    }
}
