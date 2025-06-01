using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.PriestMod
{
    [HarmonyPatch(typeof(PlayerVoteArea))]
    public class AllowExtraVotes
    {
        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.VoteForMe))]
        public static class VoteForMe
        {
            public static bool Prefix(PlayerVoteArea __instance)
            {
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Priest)) return true;
                var role = Role.GetRole<Priest>(PlayerControl.LocalPlayer);
                if (__instance.Parent.state == MeetingHud.VoteStates.Proceeding ||
                    __instance.Parent.state == MeetingHud.VoteStates.Results)
                    return false;

                if (__instance != role.Execute)
                {
                    role.ExecuteThisMeeting = true;
                    Utils.Rpc(CustomRPC.PriestExecute, false, role.Player.PlayerId);
                    return true;
                }
                else
                {
                    MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(false);
                    AddPriestExecute.UpdateButton(role, MeetingHud.Instance);
                    if (!AmongUsClient.Instance.AmHost)
                    {
                        Utils.Rpc(CustomRPC.PriestExecute, true, role.Player.PlayerId);
                    }
                    return false;
                }
            }
        }
    }
}