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
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts1to2card.src.BlueDefect.cards
{
    public sealed class BlueDoomAndGloom : CardModel
    {
        public BlueDoomAndGloom()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
        {
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(4m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState == null)
                return;

            if (cardPlay.Target == null)
                return;
                
            await CreatureCmd.Damage(choiceContext, cardPlay.Target, base.DynamicVars.Damage, this);
            await OrbCmd.Channel<DarkOrb>(choiceContext, Owner);
        }
    }
}