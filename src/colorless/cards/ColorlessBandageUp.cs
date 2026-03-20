using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts1to2card.src.colorless.cards
{
    public sealed class ColorlessBandageUp : CardModel
    {
        // 关键字：消耗
        public override IEnumerable<CardKeyword> CanonicalKeywords 
            => new List<CardKeyword> { CardKeyword.Exhaust };

        // 动态变量：回血量
        protected override IEnumerable<DynamicVar> CanonicalVars 
            => new List<DynamicVar> { new HealVar(4m) };  // 基础回血4，升级后6可在OnUpgrade修改

        public ColorlessBandageUp()
            : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) // 耗能0，消耗
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 播放施法动画
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            // 播放特效（参考燃烧之血）
            VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_heal");

            // 回复生命
            await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        }

        protected override void OnUpgrade()
        {
            // 升级回血量
            DynamicVars.Heal.UpgradeValueBy(2m); // 升级回血4→6
        }
    }
}