using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.colorless.cards;

public sealed class ColorlessMadness : CardModel
{
    public ColorlessMadness()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new List<CardKeyword> { CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 遍历手牌，随机选择攻击/技能/能力牌并耗能归0
        var hand = PileType.Hand.GetPile(base.Owner).Cards;
        var validCards = new List<CardModel>();

        foreach (var card in hand)
        {
            if (card != this &&
                (card.Type == CardType.Attack || card.Type == CardType.Skill || card.Type == CardType.Power))
            {
                validCards.Add(card);
            }
        }

        // 随机选择一张有效牌
        var selected = base.Owner.RunState.Rng.CombatCardSelection.NextItem(validCards);
        if (selected != null)
        {
            selected.EnergyCost.SetThisCombat(0, reduceOnly: true);
        }

        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}