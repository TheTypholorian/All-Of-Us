using HarmonyLib;
using Reactor.Utilities.Extensions;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.CultistMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))] // BBFDNCCEJHI
    public static class VotingComplete
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Cultist))
            {
                var politician = Role.GetRole<Cultist>(PlayerControl.LocalPlayer);
                politician.RevealButton.Destroy();
            }
        }
    }
}