using EmbedIO.Routing;
using EmbedIO.WebApi;
using EmbedIO;


namespace EmbedIO_ServerConsole.Controllers
{
    public class TestController : WebApiController
    {
        [Route(HttpVerbs.Get, "/testresponse")]
        public int GetTestResponse()
        {
            return 12345;
        }
    }
}
