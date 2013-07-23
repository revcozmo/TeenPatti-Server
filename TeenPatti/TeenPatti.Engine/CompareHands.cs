﻿using System;
using System.Collections.Generic;
using System.Linq;
using TeenPatti.Model;

namespace TeenPatti.Engine
{
    public static class CompareHands
    {
        public static CompareResult Compare(this ClassicHand handA, ClassicHand handB)
        {
            var kindA = ResolveHandKind(handA);
            var kindB = ResolveHandKind(handB);

            if(kindA > kindB)
                return CompareResult.AWon;
            if (kindB > kindA)
                return CompareResult.BWon;

            //  If both players have same hand kind
            return ResolveSameKindWinner.Compare(kindA, handA, handB); 
        }

        private static HandKind ResolveHandKind(ClassicHand hand)
        {
            var cards = hand.Cards;
            
            //  1.  Trail

            if(cards[0].Value.Equals(cards[1].Value) && cards[1].Value.Equals(cards[2].Value))
                return HandKind.Trail;

            //  2.  Sequence

            var handClone = Clone(hand);
            var cardClone = handClone.Cards;
            if (Math.Abs(cardClone[0].Value - cardClone[1].Value) + Math.Abs(cardClone[1].Value - cardClone[2].Value) + Math.Abs(cardClone[0].Value - cardClone[2].Value) == 4)
                //  test for pure sequence
                if(cardClone[0].Suite.Equals(cardClone[1].Suite) && cardClone[1].Suite.Equals(cardClone[2].Suite))
                    return HandKind.PureSequence;
                else
                    return HandKind.Sequence;

            //  test with soft ace
            foreach (var card in cardClone.Where(card => card.Value.Equals(1)))
            {
                card.SetValue(14);
            }
            if (Math.Abs(cardClone[0].Value - cardClone[1].Value) + Math.Abs(cardClone[1].Value - cardClone[2].Value) + Math.Abs(cardClone[0].Value - cardClone[2].Value) == 4)
                //  test for pure sequence
                if (cardClone[0].Suite.Equals(cardClone[1].Suite) && cardClone[1].Suite.Equals(cardClone[2].Suite))
                    return HandKind.PureSequence;
                else
                    return HandKind.Sequence;
            
            //  3.  Colour

            if (cards[0].Suite.Equals(cards[1].Suite) && cards[1].Suite.Equals(cards[2].Suite))
                return HandKind.Colour;

            //  4.  Pair

            if(cards[0].Value.Equals(cards[1].Value) || cards[1].Value.Equals(cards[2].Value) ||cards[0].Value.Equals(cards[2].Value))
                return HandKind.Pair;

            //  5.  High card
            
            return HandKind.HighCard;
        }

        private static ClassicHand Clone(Hand hand)
        {
            var clonedHand = new ClassicHand();
            hand.Cards.ForEach(c=>clonedHand.Cards.Add(c));
            return clonedHand;
        }

        
    }
}