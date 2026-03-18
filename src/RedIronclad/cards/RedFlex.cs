using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Models;
using sts1to2card.src.RedIronclad.powers;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedFlex : CardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>
        {
            new PowerVar<StrengthPower>(2m)
        };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip>
        {
            HoverTipFactory.FromPower<StrengthPower>()
        };

    public RedFlex()
        : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    // 打牌前播放黄光特效（参考 Inflame）
    public override async Task OnEnqueuePlayVfx(Creature? target)
    {
        if (base.Owner?.Creature != null)
        {
            NPowerUpVfx.CreateNormal(base.Owner.Creature); // 黄光特效
            await CreatureCmd.TriggerAnim(
                base.Owner.Creature,
                "Cast",
                base.Owner.Character.CastAnimDelay
            );
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<RedFlexPower>(
            base.Owner.Creature,
            base.DynamicVars.Strength.BaseValue,
            base.Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Strength.UpgradeValueBy(2m);
    }
}