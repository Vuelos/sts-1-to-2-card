using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedRecklessCharge : CardModel
{
    // 显示伤害值
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(7m, ValueProp.Move)
    };

    // 显示眩晕信息
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
    {
        HoverTipFactory.FromCard<Dazed>()
    };

    public RedRecklessCharge()
        : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) throw new ArgumentNullException(nameof(cardPlay.Target));

        // 攻击特效
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // 生成眩晕卡
        if (base.CombatState != null)
        {
            CardModel dazed = base.CombatState.CreateCard<Dazed>(base.Owner);
            var addedCards = await CardPileCmd.AddGeneratedCardToCombat(dazed, PileType.Draw, addedByPlayer: true);

            // 显示卡牌加入抽牌堆动画
            CardCmd.PreviewCardPileAdd(addedCards);
        }

        await Cmd.Wait(0.5f); // 保持动画停顿
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m); // 升级后伤害 7 -> 10
    }
}