using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedSpotWeakness : CardModel
{
    // --- 边框高亮条件 ---
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (base.CombatState == null)
                return false;

            // 只要有敌人意图攻击 → 边框高亮
            return base.CombatState.HittableEnemies
                .Any(e => e.Monster?.IntendsToAttack ?? false);
        }
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip>
        {
            HoverTipFactory.FromPower<StrengthPower>()
        };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>
        {
            new PowerVar<StrengthPower>(3m)
        };

    public RedSpotWeakness()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放 Cast 动画
        await CreatureCmd.TriggerAnim(
            base.Owner.Creature,
            "Cast",
            base.Owner.Character.CastAnimDelay);

        // 自动判断敌人意图 → 获得力量
        bool shouldGain = base.CombatState?.HittableEnemies
            .Any(e => e.Monster?.IntendsToAttack ?? false) ?? false;

        if (shouldGain)
        {
            await PowerCmd.Apply<StrengthPower>(
                base.Owner.Creature,
                base.DynamicVars.Strength.BaseValue,
                base.Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        // 升级后力量 +1
        base.DynamicVars.Strength.UpgradeValueBy(1m);
    }
}