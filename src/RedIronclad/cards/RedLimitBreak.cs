using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Models;

namespace sts1to2card.src.red.cards
{
    public sealed class RedLimitBreak : CardModel
    {
        public RedLimitBreak() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
        {
        }

        // 初始具有消耗词条
        public override IEnumerable<CardKeyword> CanonicalKeywords
        {
            get
            {
                return new CardKeyword[] { CardKeyword.Exhaust };
            }
        }

        // 无固定变量（翻倍效果由逻辑实现）
        protected override IEnumerable<DynamicVar> CanonicalVars
        {
            get
            {
                return Array.Empty<DynamicVar>();
            }
        }

        // 力量提示
        protected override IEnumerable<IHoverTip> ExtraHoverTips
        {
            get
            {
                return new IHoverTip[] { HoverTipFactory.FromPower<StrengthPower>() };
            }
        }

        // ====== 关键：加载 Inflame 的火焰特效资源 ======
        protected override IEnumerable<string> ExtraRunAssetPaths
        {
            get
            {
                return NGroundFireVfx.AssetPaths;
            }
        }

        // ====== 打牌前播放火焰特效 ======
        public override async Task OnEnqueuePlayVfx(Creature? target)
        {
            NCombatRoom? instance = NCombatRoom.Instance;
            if (instance != null)
            {
                instance.CombatVfxContainer.AddChildSafely(
                    NGroundFireVfx.Create(base.Owner.Creature, VfxColor.Red)
                );
            }

            await CreatureCmd.TriggerAnim(
                base.Owner.Creature,
                "Cast",
                base.Owner.Character.CastAnimDelay
            );
        }

        // 打出时：获取当前力量值，并施加相同数值的力量（实现翻倍）
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 播放力量增益特效（参考 Inflame）
            NPowerUpVfx.CreateNormal(base.Owner.Creature);

            int currentStrength = base.Owner.Creature.GetPowerAmount<StrengthPower>();

            await PowerCmd.Apply<StrengthPower>(
                base.Owner.Creature,
                currentStrength,
                base.Owner.Creature,
                this,
                false
            );
        }

        // 升级移除消耗
        protected override void OnUpgrade()
        {
            base.RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}