//using Thinktecture.IdentityModel.WebApi;

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Thinktecture.IdentityModel.Client;
using Thinktecture.IdentityModel.WebApi;
using Thinktecture.Samples.Models;
using TodoApi.Models;

namespace Thinktecture.Samples.Controller
{
    [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
    public class TodoController : ApiController
    {
        private TodoApiContext db = new TodoApiContext();

        // GET: api/Todo
        [ResourceAuthorize("Read", "Todo")]
        public IQueryable<Todo> GetTodos()
        {
            return db.Todos.OrderByDescending(todo => todo.TodoId).Take(10);
        }

        // POST: api/Todo
        [ResourceAuthorize("Post", "Todo")]
        [ResponseType(typeof(Todo))]
        public IHttpActionResult PostTodo(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var principal = User as ClaimsPrincipal;

            var owner = (from c in principal.Claims
                         where c.Type == "hovland.name/Display name"
                         select c.Value).FirstOrDefault();

            todo.Owner = owner;
            db.Todos.Add(todo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = todo.TodoId }, todo);
        }

        void Tweet(string text)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var oauth = new OAuth2Client(new Uri("https://localhost/AuthServer/tweeterApi/oauth/authorize"));
            var url = oauth.CreateCodeFlowUrl("todoAPI", "tweet", "https://localhost/TodoApi/callback?text=" + text);
            var http = new HttpClient();
            var task = http.GetAsync(url);
            task.Wait();

            var response = task.Result;
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(response.Content.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}