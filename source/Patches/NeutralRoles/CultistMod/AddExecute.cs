using HarmonyLib;
using TMPro;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.CrewmateRoles.PriestMod
{
    public class AddPriestExecute
    {
        public static void UpdateButton(Priest role, MeetingHud __instance)
        {
            var skip = __instance.SkipVoteButton;

            if (role.Execute == null)
            {
                GenButton(role, __instance);
            }

            role.Execute.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !skip.voteComplete &&!role.Player.IsJailed());
            role.Execute.voteComplete = skip.voteComplete;
            role.Execute.GetComponent<SpriteRenderer>().enabled = skip.GetComponent<SpriteRenderer>().enabled;
            role.Execute.GetComponentsInChildren<TextMeshPro>()[0].text = "Execute";
        }

        public static void GenButton(Priest role, MeetingHud __instance)
        {
            var skip = __instance.SkipVoteButton;
            role.Execute = Object.Instantiate(skip, skip.transform.parent);
            role.Execute.Parent = __instance;
            role.Execute.SetTargetPlayerId(251);
            role.Execute.transform.localPosition = skip.transform.localPosition + new Vector3(0f, -0.17f, 0f);
            skip.transform.localPosition += new Vector3(0f, 0.20f, 0f);
            UpdateButton(role, __instance);
        }

        public static void AddCultistButton(MeetingHud __instance)
        {
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return;
            var prosRole = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
            GenButton(prosRole, __instance);
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ClearVote))]
        public class MeetingHudClearVote
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return;
                var prosRole = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
                UpdateButton(prosRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        public class MeetingHudConfirm
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return;
                var prosRole = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
                prosRole.Execute.ClearButtons();
                UpdateButton(prosRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Select))]
        public class MeetingHudSelect
        {
            public static void Postfix(MeetingHud __instance, int __0)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return;
                var prosRole = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
                if (__0 != 251) prosRole.Execute.ClearButtons();

                UpdateButton(prosRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public class MeetingHudVotingComplete
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return;
                var prosRole = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
                UpdateButton(prosRole, __instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        public class MeetingHudUpdate
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return;
                var prosRole = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
                switch (__instance.state)
                {
                    case MeetingHud.VoteStates.Discussion:
                        if (__instance.discussionTimer < GameOptionsManager.Instance.currentNormalGameOptions.DiscussionTime)
                        {
                            prosRole.Execute.SetDisabled();
                            break;
                        }


                        prosRole.Execute.SetEnabled();
                        break;
                }

                UpdateButton(prosRole, __instance);
            }
        }
    }
}