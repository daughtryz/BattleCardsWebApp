namespace BattleCards.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {

        
        public HttpResponse Index()
        {
            if(this.IsUserLoggedIn())
            {
                return this.Redirect("/Cards/All");
            }
            return this.View();
        }

        [HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return this.Index();
        }

    }
}