using BattleCards.Data;
using BattleCards.Services;
using BattleCards.ViewModels.Cards;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardsService;
        private readonly ApplicationDbContext db;

        public CardsController(ICardsService cardsService,ApplicationDbContext db)
        {
            this.cardsService = cardsService;
            this.db = db;
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var currentCard = this.db.Cards.FirstOrDefault(x => x.Id == cardId);

            var currentUser = this.db.Users.FirstOrDefault(x => x.Id == this.User);


            if (this.db.UserCards.Any(x => x.UserId == currentUser.Id && x.CardId == currentCard.Id))
            {
                return this.Redirect("/Cards/All");
            }

            this.cardsService.AddToCollection(cardId, this.User);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if(!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            this.cardsService.RemoveFromCollection(cardId, this.User);

            return this.Redirect("/Cards/Collection");
        }
        public HttpResponse Collection()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var collectionViewModel = new AllCardsViewModel
            {
                Cards = this.cardsService.MyCollection(this.User),
            };
            return this.View(collectionViewModel);
        }
        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var allCardsViewModel = new AllCardsViewModel
            {
                Cards = this.cardsService.AllCards(),
            };
            return this.View(allCardsViewModel);

        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(CardInputModel input)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(input.Name.Length < 5 || input.Name.Length > 15)
            {
                throw new Exception("Name should be between 5 and 15!");
            }
            if(input.ImageUrl == null)
            {
                throw new Exception("Image url should not be null!");
            }
            if (input.Keyword == null)
            {
                throw new Exception("Keyword should not be null!");
            }
            if(input.Attack < 0)
            {
                throw new Exception("Attack cannot be negative");
            }

            if (input.Health < 0)
            {
                throw new Exception("Health cannot be negative");
            }

            if(input.Description.Length > 200)
            {
                throw new Exception("Description length should be equal or below 200 characters");
            }

            if (input.Description == null)
            {
                throw new Exception("Description cannot be null!");
            }
            this.cardsService.AddCard(input.Name, input.ImageUrl, input.Keyword, input.Attack, input.Health, input.Description);


            return this.Redirect("/Cards/All");
        }
    }
}
