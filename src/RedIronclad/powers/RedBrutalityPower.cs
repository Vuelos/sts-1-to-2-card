using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.RedIronclad.powers;

public sealed class RedBrutalityPower : PowerModel
{
    private const string SelfDamageKey = "SelfDamage";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // 官方干净写法，不显示额外提示
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>();

    // 生命损失变量
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(SelfDamageKey, 1m, ValueProp.Unblockable | ValueProp.Unpowered)
    };

    // 回合开始时失去生命
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            Flash();
            DamageVar damageVar = (DamageVar)DynamicVars[SelfDamageKey];
            await CreatureCmd.Damage(choiceContext, Owner, damageVar.BaseValue, damageVar.Props, Owner, null);
        }
    }

    // 回合开始增加抽牌
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player == Owner.Player)
        {
            return count + 1;
        }
        return count;
    }
}