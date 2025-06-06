using HarmonyLib;
using TownOfUs.Roles;
using UnityEngine;

namespace TownOfUs.NeutralRoles.CultistMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingHudUpdate
    {
        public static void Postfix(MeetingHud __instance)
        {
            var localPlayer = PlayerControl.LocalPlayer;
            var _role = Role.GetRole(localPlayer);
            if (_role?.RoleType != RoleEnum.Cultist) return;
            if (localPlayer.Data.IsDead) return;
            var role = (Cultist)_role;
            foreach (var state in __instance.playerStates)
            {
                var targetId = state.TargetPlayerId;
                var playerData = Utils.PlayerById(targetId)?.Data;
                if (playerData == null || playerData.Disconnected)
                {
                    role.CampaignedPlayers.Remove(targetId);
                    continue;
                }
                if (role.CampaignedPlayers.Contains(targetId) && role.Player.PlayerId != targetId) state.NameText.color = Color.magenta;
            }
        }
    }
}