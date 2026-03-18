using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using sts1to2card.src.RedIronclad.cards;

namespace sts1to2card.src.RedIronclad.powers;

public class RedFlexPower : TemporaryStrengthPower
{
	public override AbstractModel OriginModel => ModelDb.Card<RedFlex>();
}