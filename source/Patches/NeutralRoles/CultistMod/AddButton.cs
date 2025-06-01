using System;
using System.Linq;
using Reactor.Utilities.Extensions;
using TownOfUs.Roles;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using TownOfUs.CrewmateRoles.MayorMod;

namespace TownOfUs.NeutralRoles.CultistMod
{
    public class AddRevealButtonCultist
    {
        public static Sprite RevealSprite => TownOfUs.RevealSprite;

        public static void GenButton(Cultist role, int index)
        {
            var confirmButton = MeetingHud.Instance.playerStates[index].Buttons.transform.GetChild(0).gameObject;

            var newButton = Object.Instantiate(confirmButton, MeetingHud.Instance.playerStates[index].transform);
            var renderer = newButton.GetComponent<SpriteRenderer>();
            var passive = newButton.GetComponent<PassiveButton>();

            renderer.sprite = RevealSprite;
            newButton.transform.position = confirmButton.transform.position - new Vector3(0.75f, 0f, 0f);
            newButton.transform.localScale *= 0.8f;
            newButton.layer = 5;
            newButton.transform.parent = confirmButton.transform.parent.parent;

            passive.OnClick = new Button.ButtonClickedEvent();
            passive.OnClick.AddListener(Reveal(role));
            role.RevealButton = newButton;
        }


        private static Action Reveal(Cultist role)
        {
            void Listener()
            {
                role.RevealButton.Destroy();
                if (role.CampaignedPlayers.ToArray().Where(x => Utils.PlayerById(x) != null && Utils.PlayerById(x).Data != null && !Utils.PlayerById(x).Data.IsDead && !Utils.PlayerById(x).Data.Disconnected).ToList().Count * 2 >=
                    PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead && !x.Data.Disconnected && !x.Is(RoleEnum.Cultist)).ToList().Count)
                {
                    Role.RoleDictionary.Remove(role.Player.PlayerId);
                    var mayorRole = new Priest(role.Player, role.CampaignedPlayers);
                    mayorRole.RegenTask();
                    Utils.Rpc(CustomRPC.Appoint, role.Player.PlayerId);
                    //AddPriestButtons.AddPriestButtons(MeetingHud.Instance); TODO
                }
                else
                {
                    HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "You need to indoctrinate more people!");
                }
            }

            return Listener;
        }

        public static void AddCultistButtons(MeetingHud __instance)
        {
            foreach (var role in Role.GetRoles(RoleEnum.Cultist))
            {
                var politician = (Cultist)role;
                politician.RevealButton.Destroy();
            }

            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Cultist)) return;
            if (PlayerControl.LocalPlayer.IsJailed()) return;
            var politicianrole = Role.GetRole<Cultist>(PlayerControl.LocalPlayer);
            politicianrole.CanCampaign = true;
            for (var i = 0; i < __instance.playerStates.Length; i++)
                if (PlayerControl.LocalPlayer.PlayerId == __instance.playerStates[i].TargetPlayerId)
                {
                    GenButton(politicianrole, i);
                }
        }
    }
}