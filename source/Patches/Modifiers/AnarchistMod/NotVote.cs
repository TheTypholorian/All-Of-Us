using HarmonyLib;
using Reactor.Utilities.Extensions;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;

namespace TownOfUs.Modifiers.AnarchistMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))] // BBFDNCCEJHI
    public static class VotingComplete
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PlayerControl.LocalPlayer.Is(ModifierEnum.Anarchist))
            {
                var anarchist = Modifier.GetModifier<Anarchist>(PlayerControl.LocalPlayer);
                if (!anarchist.Revealed) anarchist.RevealButton.Destroy();
            }
        }
    }
}