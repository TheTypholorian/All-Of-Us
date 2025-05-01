using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Anarchist : Modifier
    {
        public bool Revealed = false;

        public Anarchist(PlayerControl player) : base(player)
        {
            Name = "Anarchist";
            TaskText = () => Revealed ? "Convince crew that you're the real mayor" : "Who's the mayor now?";
            Color = Patches.Colors.Impostor;
            ModifierType = ModifierEnum.Anarchist;
        }

        public GameObject RevealButton = new GameObject();
    }
}
