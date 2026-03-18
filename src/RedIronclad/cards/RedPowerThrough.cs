using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace sts1to2card.src.RedIronclad.cards;

public sealed class RedPowerThrough : CardModel
{
	public override bool GainsBlock => true;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			yield return HoverTipFactory.FromCard<Wound>();
		}
	}

	protected override IEnumerable<DynamicVar> CanonicalVars
	{
		get
		{
			yield return new BlockVar(15m, ValueProp.Move);
		}
	}

	public RedPowerThrough()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);

		for (int i = 0; i < 2; i++)
		{
			CardModel card = base.CombatState!.CreateCard<Wound>(base.Owner);
			await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, addedByPlayer: true);
		}
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Block.UpgradeValueBy(5m);
	}
}