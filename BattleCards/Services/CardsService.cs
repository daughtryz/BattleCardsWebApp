using BattleCards.Data;
using BattleCards.ViewModels.Cards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext db;

        public CardsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void AddCard(string name, string imageUrl, string keyword, int attack, int health, string description)
        {
            

            Card card = new Card
            {
                Name = name,
                ImageUrl = imageUrl,
                Keyword = keyword,
                Attack = attack,
                Health = health,
                Description = description
            };

            this.db.Cards.Add(card);
            this.db.SaveChanges();
        }

        public void AddToCollection(int cardId, string userId)
        {
            var currentCard = this.db.Cards.FirstOrDefault(x => x.Id == cardId);

            var currentUser = this.db.Users.FirstOrDefault(x => x.Id == userId);


            

            var userCard = new UserCard
            {
                UserId = currentUser.Id,
                User = currentUser,
                CardId = currentCard.Id,
                Card = currentCard,
            };
            currentUser.UserCards.Add(userCard);
            this.db.UserCards.Add(userCard);
            this.db.SaveChanges();
            
        }

        public IEnumerable<Card> AllCards()
        {
            var allCards = this.db.Cards.Select(c => new Card
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Keyword = c.Keyword,
                Attack = c.Attack,
                Health = c.Health,
                Description = c.Description,
            }).ToList();

            return allCards;
        }

        public IEnumerable<Card> MyCollection(string userId)
        {
            var currentUserCards = this.db.UserCards.Where(x => x.UserId == userId).ToList();

            var currentUser = this.db.Users.Where(x => x.Id == userId).FirstOrDefault();

            var allCards = new List<Card>();

            foreach (var userCards in currentUserCards)
            {
                foreach (var card in this.db.Cards)
                {
                    if(card.Id == userCards.CardId)
                    {
                        allCards.Add(card);
                    }
                }
            }

             allCards = allCards.Select(c => new Card
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Keyword = c.Keyword,
                Attack = c.Attack,
                Health = c.Health,
                Description = c.Description,
            }).ToList();


            return allCards;
        }

        public void RemoveFromCollection(int cardId, string userId)
        {
            var currentUserCard = this.db.UserCards.FirstOrDefault(x => x.UserId == userId && x.CardId == cardId);
            var currentUser = this.db.Users.Where(x => x.Id == userId).FirstOrDefault();

            if(currentUserCard == null)
            {
                return;
            }

            currentUser.UserCards.Remove(currentUserCard);
            this.db.UserCards.Remove(currentUserCard);

            this.db.SaveChanges();
                   
        }


    }
}
