using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models;
using sts1to2card.src.RedIronclad.powers;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedBerserk : CardModel
{
    // 使用普通 IEnumerable 初始化
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new PowerVar<VulnerablePower>(2m),   // 给自己2层易伤
        new EnergyVar(1)                      // 回合开始获得能量
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
    {
        HoverTipFactory.FromPower<VulnerablePower>(),
        base.EnergyHoverTip
    };

    public RedBerserk()
        : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        
        // 给自己施加易伤（只施加一次）
        await PowerCmd.Apply<VulnerablePower>(
            base.Owner.Creature, 
            base.DynamicVars.Vulnerable.BaseValue, 
            base.Owner.Creature, 
            this
        );

        // 回合开始获得能量
        await PowerCmd.Apply<RedBerserkPower>(
            base.Owner.Creature, 
            base.DynamicVars.Energy.BaseValue, 
            base.Owner.Creature, 
            this, 
            false
        );
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Vulnerable.UpgradeValueBy(-1m); // 升级后易伤降为1层
    }
}