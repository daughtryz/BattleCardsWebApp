using BattleCards.Data;
using BattleCards.ViewModels.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.Services
{
    public interface ICardsService
    {
        void AddCard(string name,string imageUrl,string keyword,int attack,int health,string description);

        void AddToCollection(int cardId,string userId);

        void RemoveFromCollection(int cardId, string userId);

        IEnumerable<Card> AllCards();

        IEnumerable<Card> MyCollection(string userId);
    }
}
