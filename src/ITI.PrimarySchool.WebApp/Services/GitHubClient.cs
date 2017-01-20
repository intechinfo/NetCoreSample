using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class GitHubClient
    {
        public async Task<IEnumerable<string>> GetFollowedUsers( string githubAccessToken )
        {
            using( HttpClient client = new HttpClient() )
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add( "Authorization", string.Format( "token {0}", githubAccessToken ) );
                headers.Add( "User-Agent", "PrimarySchool" );
                HttpResponseMessage response = await client.GetAsync( "https://api.github.com/user/following" );

                using( TextReader tr = new StreamReader( await response.Content.ReadAsStreamAsync() ) )
                using( JsonTextReader jsonReader = new JsonTextReader( tr ) )
                {
                    JToken json = JToken.Load( jsonReader );
                    return json.Select( u => ( string )u[ "login" ] ).ToList();
                }
            }
        }
    }
}