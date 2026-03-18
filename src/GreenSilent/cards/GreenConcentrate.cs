using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.GreenSilent.cards;  // 替换为你的模组命名空间

/// <summary>
/// 集中：丢弃指定数量的牌，获得2点能量。
/// </summary>
public class GreenConcentrate : CardModel
{
    private const int EnergyGain = 2;               // 获得的能量固定为2
    private const int DiscardCount = 3;              // 基础丢弃数量
		public GreenConcentrate()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}
    

    // 卡牌基础数值：丢弃数量（Cards）和获得能量（Energy）
    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            yield return new CardsVar(DiscardCount);      // 默认3张
            yield return new EnergyVar(EnergyGain);       // 2能量
        }
    }

    // 打出时的效果：先选择并丢弃指定数量的牌，然后获得能量
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int discardCount = DynamicVars.Cards.IntValue;   // 当前丢弃数量（可能因升级变化）

        // 从手牌中选择 discardCount 张牌丢弃
        IEnumerable<CardModel> cardsToDiscard = await CardSelectCmd.FromHandForDiscard(
            choiceContext,
            base.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, discardCount),
            null,   // filter 参数，无需筛选
            this    // sourceCard 参数，表示触发此选择的卡牌
        );

        // 执行丢弃
        await CardCmd.Discard(choiceContext, cardsToDiscard);

        // 获得能量
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, base.Owner);
    }

    // 升级效果：丢弃数量减1（3→2）
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(-1);   // 丢弃数量减少1
        // 能量值不变
    }
}