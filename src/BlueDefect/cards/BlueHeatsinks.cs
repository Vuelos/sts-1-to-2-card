using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using sts1to2card.src.BlueDefect.powers;

namespace sts1to2card.src.BlueDefect.cards
{
    public sealed class BlueHeatsinks : CardModel
    {
        public BlueHeatsinks()
            : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
        {
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars["Amount"].UpgradeValueBy(1m);
        }
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState == null)
                return;

            await PowerCmd.Apply<BlueHeatsinksPower>([base.Owner.Creature], base.DynamicVars["Amount"].IntValue, base.Owner.Creature, this);
        }
    }
}