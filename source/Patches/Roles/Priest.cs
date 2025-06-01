using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Priest : Role
    {
        public List<byte> CultMembers;
        public bool ExecuteThisMeeting { get; set; }
        public PlayerVoteArea Execute { get; set; }

        public Priest(PlayerControl player, List<byte> cult) : base(player)
        {
            Name = "Priest";
            ImpostorText = () => "Execute the Impostors";
            TaskText = () => "Find the impostors, and your cult will help you remove them";
            Color = Patches.Colors.Priest;
            RoleType = RoleEnum.Priest;
            AddToRoleHistory(RoleType);
            Faction = Faction.Crewmates;
            CultMembers = cult;
        }
    }
}