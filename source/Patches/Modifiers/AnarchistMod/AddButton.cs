using System;
using System.Linq;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.AssassinMod;
using TownOfUs.CrewmateRoles.VigilanteMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using TownOfUs.NeutralRoles.DoomsayerMod;

namespace TownOfUs.Modifiers.AnarchistMod
{
    public class AddRevealButtonAnarchist
    {
        public static Sprite RevealSprite => TownOfUs.RevealSprite;

        public static void GenButton(Anarchist role, int index)
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

        public static void RemoveAssassin(Anarchist anarchist)
        {
            PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(
                x => x.TargetPlayerId == anarchist.Player.PlayerId);

            if (PlayerControl.LocalPlayer.Is(AbilityEnum.Assassin))
            {
                var assassin = Ability.GetAbility<Assassin>(PlayerControl.LocalPlayer);
                ShowHideButtons.HideTarget(assassin, voteArea.TargetPlayerId);
                voteArea.NameText.transform.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Doomsayer))
            {
                var doomsayer = Role.GetRole<Doomsayer>(PlayerControl.LocalPlayer);
                ShowHideButtonsDoom.HideTarget(doomsayer, voteArea.TargetPlayerId);
                voteArea.NameText.transform.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
                foreach (var (targetId, guessText) in doomsayer.RoleGuess)
                {
                    if (!guessText.isActiveAndEnabled || voteArea.TargetPlayerId != targetId) continue;
                    guessText.gameObject.SetActive(false);
                }
            }
            else if (PlayerControl.LocalPlayer.Is(RoleEnum.Vigilante))
            {
                var vigilante = Role.GetRole<Vigilante>(PlayerControl.LocalPlayer);
                ShowHideButtonsVigi.HideTarget(vigilante, voteArea.TargetPlayerId);
                voteArea.NameText.transform.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
            }
        }

        private static Action Reveal(Anarchist role)
        {
            void Listener()
            {
                role.RevealButton.Destroy();
                role.Revealed = true;
                Utils.Rpc(CustomRPC.ElectAnarchist, role.Player.PlayerId);
            }

            return Listener;
        }

        public static void AddAnarchistButtons(MeetingHud __instance)
        {
            foreach (var role in Modifier.GetModifiers(ModifierEnum.Anarchist))
            {
                var anarchist = (Anarchist)role;
                anarchist.RevealButton.Destroy();
            }

            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(ModifierEnum.Anarchist)) return;
            if (PlayerControl.LocalPlayer.IsJailed()) return;
            var anarchistmod = Modifier.GetModifier<Anarchist>(PlayerControl.LocalPlayer);
            if (anarchistmod.Revealed) return;
            for (var i = 0; i < __instance.playerStates.Length; i++)
                if (PlayerControl.LocalPlayer.PlayerId == __instance.playerStates[i].TargetPlayerId)
                {
                    GenButton(anarchistmod, i);
                }
        }
    }
}