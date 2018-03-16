using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GithubApi.Controllers
{
    using Octokit;

    [Route("api/[controller]")]
    public class RepositoriesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetByLanguage(string language = "CSharp")
        {
            Language searchLanguage;
            if (!Language.TryParse(language, true, out searchLanguage))
            {
                return BadRequest("Please use a supported language " + string.Join(", ", Enum.GetNames(typeof(Language))));
            }

            var github = new GitHubClient(new ProductHeaderValue("mutmatt.GitHubSearchApi"));
            var repos = await github.Search.SearchRepo(new SearchRepositoriesRequest
                                                          {
                                                              Language = searchLanguage,
                                                              SortField = RepoSearchSort.Stars,
                                                              Order = SortDirection.Descending,
                                                              Stars = Range.GreaterThan(1000),//arbitrary starting point to limit scope
                                                              PerPage = 5
                                                          });

            return this.Json(repos);
        }
    }
}
