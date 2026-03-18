using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using sts1to2card.src.RedIronclad.vfx;


namespace sts1to2card.src.red.cards
{
    public sealed class RedReaper : CardModel
    {
        public RedReaper()
            : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords
        {
            get
            {
                return new CardKeyword[] { CardKeyword.Exhaust };
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars
        {
            get
            {
                return new DynamicVar[]
                {
                    new DamageVar(4m, ValueProp.Move)
                };
            }
        }


        // ===== 播放特效 =====
        public override async Task OnEnqueuePlayVfx(Creature? target)
        {
            RedReaperVfx.Play(base.Owner.Creature);

            await CreatureCmd.TriggerAnim(
                base.Owner.Creature,
                "Attack",
                base.Owner.Character.AttackAnimDelay
            );
        }

        // ===== 卡牌效果 =====
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 播放闪电特效
            RedReaperVfx.Play(Owner.Creature);
            // 等一点时间让特效出现
            await Task.Delay(120);
            
            AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(base.CombatState!)
                .WithHitFx("vfx/vfx_attack_slash", null, null)
                .Execute(choiceContext);

            int totalDamage = attackCommand.Results.Sum(r => r.TotalDamage);

            await CreatureCmd.Heal(base.Owner.Creature, totalDamage, true);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}