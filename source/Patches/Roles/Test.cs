using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUs.Extensions;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Test : Role
    {
        public Test(PlayerControl player) : base(player)
        {
            Name = "Test";
            ImpostorText = () => "Test";
            TaskText = () => "Test";
            Color = Patches.Colors.Test;
            RoleType = RoleEnum.Test;
            AddToRoleHistory(RoleType);
        }
    }
}