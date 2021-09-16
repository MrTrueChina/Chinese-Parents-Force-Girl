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
        private static bool Prefix()
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return true;
            }

            Main.ModEntry.Logger.Log("生成新周目玩家数据方法即将调用");

            // 以下代码复制粘贴自反编译
            typeof(end_panel_info).GetMethod("AddFamilyRecord", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[0]);
            JobManager.Instance.AddJob(ending_manager.IsntanceEndingManager.resultfinal);
            if (record_manager.InstanceManagerRecord.CurrentRecord.Generations == ReadXml.GetAchievement("New journey"))
            {
                global_data.InstanceGlobalData.SetAchievement("New journey");
            }
            record_manager.InstanceManagerRecord.CurrentRecord.Generations++;
            if (record_manager.InstanceManagerRecord.IsBoy())
            {
                record_manager.InstanceManagerRecord.CurrentRecord.mother_name = string.Empty;
                record_manager.InstanceManagerRecord.CurrentRecord.father_id = 0;
                record_manager.InstanceManagerRecord.CurrentRecord.father_name = player_data.Instance.self_name;
                record_manager.InstanceManagerRecord.CurrentRecord.mother_id = player_data.Instance.mother_id;
            }
            else
            {
                BoysManager.Instance.Clear();
                record_manager.InstanceManagerRecord.CurrentRecord.father_name = string.Empty;
                record_manager.InstanceManagerRecord.CurrentRecord.mother_id = 0;
                record_manager.InstanceManagerRecord.CurrentRecord.mother_name = player_data.Instance.self_name;
                record_manager.InstanceManagerRecord.CurrentRecord.father_id = player_data.Instance.father_id;
            }
            int generations = record_manager.InstanceManagerRecord.CurrentRecord.Generations;
            if (generations <= 2)
            {
                ////////----////////----//////// Mod 逻辑代码 ////////----////////----////////

                // 原来的逻辑，保证二周目的性别和一周目相反
                //record_manager.InstanceManagerRecord.CurrentRecord.playerSex = record_manager.InstanceManagerRecord.CurrentRecord.playerSex % 2 + 1;

                // 直接设置性别为 2：女孩
                record_manager.InstanceManagerRecord.CurrentRecord.playerSex = 2;

                ////////----////////----//////// Mod 逻辑代码 ////////----////////----////////

                // 以下代码复制粘贴自反编译
            }
            else
            {
                int playerSex = record_manager.InstanceManagerRecord.CurrentRecord.playerSex;
                int num = record_manager.InstanceManagerRecord.CurrentRecord.boyProbability;
                bool flag = NumbercialDesigner.probability((float)num);

                ////////----////////----//////// Mod 逻辑代码 ////////----////////----////////

                // 原来的逻辑，按照前面计算的随机数设置性别
                //record_manager.InstanceManagerRecord.CurrentRecord.playerSex = ((!flag) ? 2 : 1);

                // 直接设置性别为 2：女孩
                record_manager.InstanceManagerRecord.CurrentRecord.playerSex = 2;

                ////////----////////----//////// Mod 逻辑代码 ////////----////////----////////

                // 以下代码复制粘贴自反编译
                if (playerSex == record_manager.InstanceManagerRecord.CurrentRecord.playerSex)
                {
                    if (playerSex == 1)
                    {
                        num -= 25;
                    }
                    else
                    {
                        num += 25;
                    }
                }
                else
                {
                    num = 50;
                }
                record_manager.InstanceManagerRecord.CurrentRecord.boyProbability = num;
            }
            record_manager.InstanceManagerRecord.CurrentRecord.backgroundIndex = DEF.BackgroundIndex[record_manager.InstanceManagerRecord.CurrentRecord.playerSex - 1, Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f))];
            record_manager.InstanceManagerRecord.CurrentRecord.round_current = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.age = 1;
            record_manager.InstanceManagerRecord.CurrentRecord.naodong_level = 1;
            record_manager.InstanceManagerRecord.CurrentRecord.Pressure = 50f;
            record_manager.InstanceManagerRecord.CurrentRecord.Parentdream = 50f;
            record_manager.InstanceManagerRecord.CurrentRecord.Shadow = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.naodong_add_value = 1f;
            record_manager.InstanceManagerRecord.CurrentRecord.Face = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.math = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.chinese = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.english = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.Liberalarts = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.science = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.gaokaotype = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.Iq = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Eq = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Imagination = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Memory = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Stamination = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Charm = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Confidence = 0f;
            record_manager.InstanceManagerRecord.CurrentRecord.Potentiality = 100f;
            record_manager.InstanceManagerRecord.CurrentRecord.Wisdom = 50f;
            record_manager.InstanceManagerRecord.CurrentRecord.IQ_round = record_manager.InstanceManagerRecord.CurrentRecord.family_IQ_round;
            record_manager.InstanceManagerRecord.CurrentRecord.EQ_round = record_manager.InstanceManagerRecord.CurrentRecord.family_EQ_round;
            record_manager.InstanceManagerRecord.CurrentRecord.ImaginationRound = record_manager.InstanceManagerRecord.CurrentRecord.family_ImaginationRound;
            record_manager.InstanceManagerRecord.CurrentRecord.MemoryRound = record_manager.InstanceManagerRecord.CurrentRecord.family_MemoryRound;
            record_manager.InstanceManagerRecord.CurrentRecord.StaminationRound = record_manager.InstanceManagerRecord.CurrentRecord.family_StaminationRound;
            record_manager.InstanceManagerRecord.CurrentRecord.father_job = ending_manager.IsntanceEndingManager.resultfinal.jobId;
            record_manager.InstanceManagerRecord.CurrentRecord.father_status = player_data.Instance.status;
            record_manager.InstanceManagerRecord.CurrentRecord.round_money = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.money = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.self_name = ReadXml.GetString("InitName");
            record_manager.InstanceManagerRecord.CurrentRecord.school = "InitSchool";
            record_manager.InstanceManagerRecord.CurrentRecord.importSchool = false;
            record_manager.InstanceManagerRecord.CurrentRecord.status = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.WorksList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.lotteryNum = null;
            record_manager.InstanceManagerRecord.CurrentRecord.lotteryRewardNum = null;
            record_manager.InstanceManagerRecord.CurrentRecord.lotteryCount = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.GirlsDictionary = new Dictionary<int, int>();
            record_manager.InstanceManagerRecord.CurrentRecord.BoysDictionary = new Dictionary<int, BoyRecord>();
            record_manager.InstanceManagerRecord.CurrentRecord.BoysEvents = new Dictionary<int, int>();
            record_manager.InstanceManagerRecord.CurrentRecord.CloseFriendNoCampany = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.CloseFriendSpecialEvent = false;
            if (record_manager.InstanceManagerRecord.IsBoy())
            {
                foreach (int key in ReadXml.GetIDList("girl", null, null))
                {
                    record_manager.InstanceManagerRecord.CurrentRecord.GirlsDictionary.Add(key, 0);
                }
            }
            record_manager.InstanceManagerRecord.CurrentRecord.FriendlistDictionary = new Dictionary<int, int>();
            List<int> idlist = ReadXml.GetIDList("friend", null, null);
            foreach (int num2 in idlist)
            {
                XmlData data = ReadXml.GetData("friend", num2);
                int @int = data.GetInt("sex");
                if (@int == 0 || @int == record_manager.InstanceManagerRecord.CurrentRecord.playerSex)
                {
                    record_manager.InstanceManagerRecord.CurrentRecord.FriendlistDictionary.Add(num2, data.GetInt("init"));
                }
            }
            record_manager.InstanceManagerRecord.CurrentRecord.StoryeventList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.LearnedskillList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.UnlockskillList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.TaskList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.LearnedTrickList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.AlreadyIds = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.RewardTimes = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.LifeprocessList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.FacefightList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.NatureList = new List<int>();
            record_manager.InstanceManagerRecord.CurrentRecord.RecordcomedyInts = new List<int>();
            action_manager.Instance.ComedyList.Clear();
            record_manager.InstanceManagerRecord.CurrentRecord.ChallengeDictionary = new Dictionary<int, ChallengeData>();
            ChallengeManager.Instance.Clear();
            record_manager.InstanceManagerRecord.CurrentRecord.AnniversaryOrder = 1;
            record_manager.InstanceManagerRecord.CurrentRecord.AnniversaryComplete = false;
            record_manager.InstanceManagerRecord.CurrentRecord.candidateCount = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.starBigAwardCount = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.lotteryRewardCount = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.compositionCount = 0;
            player_data.Instance.candidateCount = 0;
            player_data.Instance.starBigAwardCount = 0;
            player_data.Instance.lotteryRewardCount = 0;
            player_data.Instance.compositionCount = 0;
            record_manager.InstanceManagerRecord.CurrentRecord.loverType = LoverType.Failed;
            record_manager.InstanceManagerRecord.save_data_override();
            map_init.Instantce.RemoveMap(record_manager.InstanceManagerRecord.CurrentRecord.Name);

            // 阻断对原方法的调用
            return false;
        }
    }

    /// <summary>
    /// 在开新档时生成新的玩家数据的方法
    /// </summary>
    [HarmonyPatch(typeof(record_manager), "create_new")]
    public static class record_manager_create_new
    {
        private static bool Prefix(ref record __result, string name)
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return true;
            }

            Main.ModEntry.Logger.Log("开新档时生成玩家数据方法即将调用");

            // 以下代码直接复制粘贴自反编译代码
            __result = new record();
            __result.Id = 1;
            __result.is_restart = true;
            __result.Rank = 1;
            __result.Name = name;
            __result.Generations = 1;
            __result.money = 0;

            ////////----////////----//////// Mod 逻辑代码 ////////----////////----////////

            // 游戏原本的随机 1/2，1 是儿子，2 是女儿
            //__result.playerSex = Mathf.RoundToInt(UnityEngine.Random.Range(1f, 2f));

            // 强制设为 2，即女儿
            __result.playerSex = 2;

            ////////----////////----//////// Mod 逻辑代码 ////////----////////----////////

            // 以下代码直接复制粘贴自反编译代码
            __result.backgroundIndex = DEF.BackgroundIndex[__result.playerSex - 1, Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f))];
            __result.PotentialityRound = 0;
            __result.ideal = 0;
            __result.family_IQ_round = 5f;
            __result.family_EQ_round = 5f;
            __result.family_ImaginationRound = 5f;
            __result.family_MemoryRound = 5f;
            __result.family_StaminationRound = 5f;
            __result.IQ_round = 5f;
            __result.EQ_round = 5f;
            __result.ImaginationRound = 5f;
            __result.MemoryRound = 5f;
            __result.StaminationRound = 5f;
            __result.LifeprocessList = new List<int>();
            __result.LearnedTrickList = new List<int>();
            __result.LearnedskillList = new List<int>();
            __result.StoryeventList = new List<int>();
            __result.TaskList = new List<int>();
            __result.UnlockskillList = new List<int>();
            __result.FacefightList = new List<int>();
            __result.NatureList = new List<int>();
            __result.AlreadyIds = new List<int>();
            __result.RewardTimes = 0;
            __result.self_name = ReadXml.GetString("InitName");
            __result.school = "InitSchool";
            __result.importSchool = false;
            __result.status = new List<int>();
            __result.FriendlistDictionary = new Dictionary<int, int>();
            List<int> idlist = ReadXml.GetIDList("friend", null, null);
            foreach (int num in idlist)
            {
                XmlData data = ReadXml.GetData("friend", num);
                int @int = data.GetInt("sex");
                if (@int == 0 || @int == __result.playerSex)
                {
                    __result.FriendlistDictionary.Add(num, data.GetInt("init"));
                }
            }
            __result.father_name = ReadXml.GetString("InitFatherName");
            __result.father_job = 7;
            __result.round_money = 0;
            __result.father_status = new List<int>();
            __result.GirlsDictionary = new Dictionary<int, int>();
            __result.BoysDictionary = new Dictionary<int, BoyRecord>();
            __result.BoysEvents = new Dictionary<int, int>();
            __result.WorksList = new List<int>();
            __result.BoughtList = new List<int>();
            __result.RecordTrickDictionary = new Dictionary<int, bool>();
            __result.RecordcomedyInts = new List<int>();
            __result.SpecialComedyInts = new List<int>();
            __result.FamilyRecord = new Dictionary<int, FamilyRecord>();
            __result.AnniversaryComplete = false;

            // 取消对原本方法的调用
            return false;
        }
    }
}
