using System.Collections.Generic;
using TestProject.Scripts.Cards;

namespace TestProject.Scripts.Games
{
    public class Slot
    {
        public string? CardId;
        public PolicyCard? Card;

        public int BaseGdpFlatBonus = 0;
        public int EffectToNatureFlatBonus = 0;
        public int EffectToHighClassFlatBonus = 0;
        public int EffectToLowClassFlatBonus = 0;

        public float BaseGdpMultiplier = 1f;
        public float EffectToNatureMultiplier = 1f;
        public float EffectToHighClassMultiplier = 1f;
        public float EffectToLowClassMultiplier = 1f;

        public bool IsNullified = false;
        public bool IsDisabled = false;

        public List<ModifierEntry> Modifiers = [];

        public int FinalBaseGdp => IsNullified || Card is null? 0 
            : (int)((Card.BaseGdp + BaseGdpFlatBonus) * BaseGdpMultiplier);

        public int FinalEffectToNature => Card is null ? 0 
            : (int)((Card.EffectToNature + EffectToNatureFlatBonus) * EffectToNatureMultiplier);

        public int FinalEffectToHighClass => Card is null ? 0 
            : (int)((Card.EffectToHighClass + EffectToHighClassFlatBonus) * EffectToHighClassMultiplier);

        public int FinalEffectToLowClass => Card is null ? 0 
            : (int)((Card.EffectToLowClass + EffectToLowClassFlatBonus) * EffectToLowClassMultiplier);
    
        public void ResolveCard()
        {
            if (CardId is null) return;
            Card = CardRegistry.Get(CardId) as PolicyCard;
        }
    }
}