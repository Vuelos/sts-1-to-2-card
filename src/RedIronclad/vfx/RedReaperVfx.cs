using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace sts1to2card.src.RedIronclad.vfx
{
    public static class RedReaperVfx
    {
        public static void Play(Creature source)
        {
            var combat = source.CombatState;
            if (combat == null) return;

            // 播放闪电攻击音效
            SfxCmd.Play("event:/sfx/characters/defect/defect_lightning_evoke");

            var enemies = combat.GetOpponentsOf(source)
                .Where(e => e.IsHittable);

            foreach (var enemy in enemies)
            {
                VfxCmd.PlayOnCreature(enemy, "vfx/vfx_attack_lightning");
            }
        }
    }
}