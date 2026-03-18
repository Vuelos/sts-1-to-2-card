using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using sts1to2card.src.RedIronclad.powers;

namespace sts1to2card.src.RedIronclad.cards // 根据你的模组命名空间调整
{
    /// <summary>
    /// 进化：获得 EvolvePower，每次抽到状态牌时抽牌。
    /// </summary>
    public sealed class RedEvolve : CardModel
    {
        public RedEvolve()
            : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
        {
        }

        // 基础层数为1，升级后变为2
        protected override IEnumerable<DynamicVar> CanonicalVars
        {
            get
            {
                yield return new PowerVar<RedEvolvePower>(1m);
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 应用能力，层数取自 CanonicalVars 中的 "EvolvePower"
            await PowerCmd.Apply<RedEvolvePower>(
                base.Owner.Creature,
                base.DynamicVars["RedEvolvePower"].BaseValue,
                base.Owner.Creature,
                this,
                false);
        }

        protected override void OnUpgrade()
        {
            // 升级后层数+1
            base.DynamicVars["RedEvolvePower"].UpgradeValueBy(1m);
        }
    }
}