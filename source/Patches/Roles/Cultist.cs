using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Cultist : Role
    {
        public PlayerControl ClosestPlayer;
        public List<byte> CampaignedPlayers = new List<byte>();
        public DateTime LastCampaigned;
        public bool CanCampaign;

        public Cultist(PlayerControl player) : base(player)
        {
            Name = "Cultist";
            ImpostorText = () => "Form a cult";
            TaskText = () => "Indoctrinate people into your cult";
            Color = Patches.Colors.Cultist;
            RoleType = RoleEnum.Cultist;
            AddToRoleHistory(RoleType);
            Faction = Faction.NeutralBenign;
            CanCampaign = true;
            LastCampaigned = DateTime.UtcNow;
        }
        public GameObject RevealButton = new GameObject();

        public float CampaignTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastCampaigned;
            var num = CustomGameOptions.CampaignCd * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}