using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.RedIronclad.powers
{
	public sealed class RedEvolvePower : PowerModel
	{
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner.Creature == base.Owner)
			{
				if (card.Type == CardType.Status)
				{
					base.Flash();
					if (base.Owner.Player != null)
                    {
                        await CardPileCmd.Draw(choiceContext, base.Amount, base.Owner.Player, false);
                    }
				}
			}
		}
	}
}