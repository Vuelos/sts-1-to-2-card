using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedExhume : CardModel
{
    // 保留消耗关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
    {
        CardKeyword.Exhaust
    };

    public RedExhume()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取消耗堆
        CardPile pile = PileType.Exhaust.GetPile(base.Owner);

        if (pile.Cards.Count == 0)
            return;

        // 选择界面
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);

        // 从消耗堆选择一张牌
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            pile.Cards,
            base.Owner,
            prefs
        )).FirstOrDefault();

        if (card is null)
            return;

        // 移动到手牌
            await CardPileCmd.Add(card, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        // 升级后费用 -1 (1 -> 0)
        base.EnergyCost.UpgradeBy(-1);
    }
}
