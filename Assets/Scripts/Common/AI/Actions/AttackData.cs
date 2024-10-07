using CrashKonijn.Goap.Classes.References;
using UnityEngine;

namespace Common.AI.Actions
{
    public class AttackData : CommonData
    {
        public static readonly int ATTACK = Animator.StringToHash("Attack");

        // todo:
        [GetComponentInChildren] public Animator Animator { get; set; }
    }
}