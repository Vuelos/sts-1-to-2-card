using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.colorless.cards;

public sealed class ColorlessChrysalis : CardModel
{
    // 关键词：消耗
    public override IEnumerable<CardKeyword> CanonicalKeywords 
        => new List<CardKeyword> { CardKeyword.Exhaust };

    // 动态变量：生成卡数量
    protected override IEnumerable<DynamicVar> CanonicalVars 
        => new List<DynamicVar> { new CardsVar(3) };

    public ColorlessChrysalis()
        : base(2, CardType.Skill, CardRarity.Event, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取本场战斗可用的技能牌
        IEnumerable<CardModel> forCombat = CardFactory.GetForCombat(
            base.Owner,
            from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
            where c.Type == CardType.Skill   // 改成技能牌
            select c,
            base.DynamicVars.Cards.IntValue,
            base.Owner.RunState.Rng.CombatCardGeneration
        );

        // 生成到抽牌堆并本场战斗免费
        foreach (CardModel item in forCombat)
        {
            item.SetToFreeThisCombat();
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Draw, addedByPlayer: true, CardPilePosition.Random)
            );
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(2m);
    }
}