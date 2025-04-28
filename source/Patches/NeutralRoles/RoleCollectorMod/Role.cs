using AmongUs.GameOptions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.NeutralRoles.RoleCollectorMod
{
    public class Role : MonoBehaviour
    {
        public PlayerControl DeadPlayer = null;
    }

    [HarmonyPatch]
    public static class RoleExtensions
    {
        public static void ClearRoles(this List<GameObject> obj)
        {
            foreach (GameObject t in obj)
            {
                UnityEngine.Object.Destroy(t);
            }
            obj.Clear();
        }

        public static GameObject CreateRole(this Vector3 location, PlayerControl victim)
        {
            GameObject Role = new GameObject("Role");
            Role.transform.position = location;
            Role.layer = LayerMask.NameToLayer("Players");
            SpriteRenderer render = Role.AddComponent<SpriteRenderer>();
            render.sprite = TownOfUs.RoleSprite;
            Vector3 scale = render.transform.localScale;
            scale.x *= 0.5f;
            scale.y *= 0.5f;
            render.transform.localScale = scale;
            BoxCollider2D splatCollider = Role.AddComponent<BoxCollider2D>();
            splatCollider.size = new Vector2(render.size.x, render.size.y);
            var scene = Role.AddComponent<Role>();
            scene.DeadPlayer = victim;
            return Role;
        }
    }
}
