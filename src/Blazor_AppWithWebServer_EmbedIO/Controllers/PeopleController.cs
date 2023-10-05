using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.Utilities;
using EmbedIO.WebApi;
using Swan.Cryptography;


namespace Blazor_AppWithWebServer_EmbedIO.Controllers
{
    // A very simple controller to handle People CRUD.
    // Notice how it Inherits from WebApiController and the methods have WebApiHandler attributes 
    // This is for sampling purposes only.
    public sealed class PeopleController : WebApiController
    {
        // Gets all records.
        // This will respond to 
        //     GET http://localhost:9696/api/people
        [Route(HttpVerbs.Get, "/people")]
        public Task<IEnumerable<Person>> GetAllPeople() => Person.GetDataAsync();

        // Gets the first record.
        // This will respond to 
        //     GET http://localhost:9696/api/people/first
        [Route(HttpVerbs.Get, "/people/first")]
        public async Task<Person> GetFirstPeople() => (await Person.GetDataAsync().ConfigureAwait(false)).First();

        // Gets a single record.
        // This will respond to 
        //     GET http://localhost:9696/api/people/1
        //     GET http://localhost:9696/api/people/{n}
        //
        // If the given ID is not found, this method will return false.
        // By default, WebApiModule will then respond with "404 Not Found".
        //
        // If the given ID cannot be converted to an integer, an exception will be thrown.
        // By default, WebApiModule will then respond with "500 Internal Server Error".
        [Route(HttpVerbs.Get, "/people/{id?}")]
        public async Task<Person> GetPeople(int id)
            => (await Person.GetDataAsync().ConfigureAwait(false)).FirstOrDefault(x => x.Id == id)
            ?? throw HttpException.NotFound();

        // Echoes request form data in JSON format.
        [Route(HttpVerbs.Post, "/echo")]
        public Dictionary<string, object> Echo([FormData] NameValueCollection data)
            => data.ToDictionary();

        // Select by name
        [Route(HttpVerbs.Get, "/peopleByName/{name}")]
        public async Task<Person> GetPeopleByName(string name)
            => (await Person.GetDataAsync().ConfigureAwait(false)).FirstOrDefault(x => x.Name == name)
            ?? throw HttpException.NotFound();
    }

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string EmailAddress { get; set; }

        internal static async Task<IEnumerable<Person>> GetDataAsync()
        {
            // Imagine this is a database call :)
            await Task.Delay(0).ConfigureAwait(false);

            return new List<Person> {
                new Person {
                    Id = 1,
                    Name = "Mario Di Vece",
                    Age = 31,
                    EmailAddress = "mario@unosquare.com"
                },
                new Person {
                    Id = 2,
                    Name = "Geovanni Perez",
                    Age = 32,
                    EmailAddress = "geovanni.perez@unosquare.com"
                },
                new Person {
                    Id = 3,
                    Name = "Luis Gonzalez",
                    Age = 29,
                    EmailAddress = "luis.gonzalez@unosquare.com"
                }
            };
        }
    }
}