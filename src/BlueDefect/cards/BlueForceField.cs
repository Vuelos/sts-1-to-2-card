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

namespace sts1to2card.src.BlueDefect.cards
{
    public sealed class BlueForceField : CardModel
    {
        public BlueForceField()
            : base(4, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
        {
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Block.UpgradeValueBy(2m);
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState == null)
                return;

            base.DynamicVars.Energy.UpgradeValueBy(-1m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState == null)
                return;

            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
            base.DynamicVars.Block.UpgradeValueBy(-1m);
        }
    }
}