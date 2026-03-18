using System.Collections.Generic; 
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedSeeingRed : CardModel
{
    // 显示能量值
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new EnergyVar(2)
    };

    // 初版带 Exhaust
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
    {
        CardKeyword.Exhaust
    };

    // 显示能量提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { base.EnergyHoverTip };

    public RedSeeingRed()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) // 基础消耗 1
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);     // 升级消耗 1 -> 0
    }
}