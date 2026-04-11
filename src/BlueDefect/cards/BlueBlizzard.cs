using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts1to2card.src.BlueDefect.cards
{
    public sealed class BlueBlizzard : CardModel
    {
        public BlueBlizzard()
            : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
        {
        }
        private const string Timmes = "Timmes";

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        
            new List<DynamicVar>
            {
                new DamageVar(2m, ValueProp.Move),
                new DynamicVar(Timmes, 2m)
            };
        
        protected override void OnUpgrade()
        {
            base.DynamicVars[Timmes].UpgradeValueBy(1m);
        }

        public override async Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
        {
            if (CombatState == null)
                return;

            if (player != Owner || orb.GetType() != typeof(FrostOrb))
                return;

            base.DynamicVars[Timmes].UpgradeValueBy(1m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState == null)
                return;

            if (cardPlay.Target == null)
                return;

            for (int i = 0; i < base.DynamicVars["Channeled"].IntValue; i++)
            {
                await CreatureCmd.Damage(choiceContext, cardPlay.Target, base.DynamicVars.Damage, this);
            }
        }
    }
}