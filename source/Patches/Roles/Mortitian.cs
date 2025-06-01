using System.Collections.Generic;
using Object = UnityEngine.Object;
using System;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Mortitian : Role
    {
        public ArrowBehaviour BodyArrow;
        public PlayerControl ClosestPlayer;
        public PlayerControl Camping = null;
        public bool CampedThisRound = false;
        public DateTime StartingCooldown { get; set; }
        public Mortitian(PlayerControl player) : base(player)
        {
            Name = "Mortitian";
            ImpostorText = () => "Camp Crewmates To Catch Their Killer";
            TaskText = () => "Camp crewmates then incriminate their killer";
            Color = Patches.Colors.Mystic;
            RoleType = RoleEnum.Mystic;
            AddToRoleHistory(RoleType);
        }

        public void DestroyArrow(byte targetPlayerId)
        {
            if (BodyArrow != null)
                Object.Destroy(BodyArrow);
            if (BodyArrow.gameObject != null)
                Object.Destroy(BodyArrow.gameObject);
            BodyArrow = null;
        }

        public float StartTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - StartingCooldown;
            var num = 10000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}