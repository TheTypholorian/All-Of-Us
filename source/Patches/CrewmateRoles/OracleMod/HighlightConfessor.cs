using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.OracleMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HighlightConfessor
    {
        public static void UpdateMeeting(Oracle role, MeetingHud __instance)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                int accuracy = (int)role.Accuracy;
                foreach (var state in __instance.playerStates)
                {
                    if (player.PlayerId != state.TargetPlayerId) continue;
                    if (player == role.Confessor)
                    {
                        if (role.RevealedFaction == Faction.Crewmates) state.NameText.text += $"<color=#00FFFFFF> ({accuracy}% Crew)</color>";
                        else if (role.RevealedFaction == Faction.Impostors) state.NameText.text += $"<color=#FF0000FF> ({accuracy}% Imp)</color>";
                        else state.NameText.text += $"<color=#808080FF> ({accuracy}% Neut)</color>";
                    }
                }
            }
        }
        public static void Postfix(HudManager __instance)
        {
            if (!MeetingHud.Instance || PlayerControl.LocalPlayer.Data.IsDead) return;
            foreach (var oracle in Role.GetRoles(RoleEnum.Oracle))
            {
                var role = Role.GetRole<Oracle>(oracle.Player);
                if (role == null || role.Player == null || role.Player.Data == null || role.Player.Data.Disconnected || !role.Player.Data.IsDead || role.Confessor == null) return;
                UpdateMeeting(role, MeetingHud.Instance);
            }
        }
    }
}