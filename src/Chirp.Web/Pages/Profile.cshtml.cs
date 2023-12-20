using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ProfileModel : PageModel
{
    public ICheepRepository CheepRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public IFollowRepository FollowRepository { get; private set; }
    public IDictionary<string, string> Claims { get; set; }
    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public IEnumerable<string> Followers { get; set; }
    public IEnumerable<string> Followings { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingsCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }

    public ProfileModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowRepository followRepository)
    {
        CheepRepository = cheepRepository;
        AuthorRepository = authorRepository;
        FollowRepository = followRepository;

        Claims = new Dictionary<string, string>();
        Cheeps = new List<CheepDTO>();
        Followers = new HashSet<string>();
        Followings = new HashSet<string>();
    }

    public ActionResult OnPostFollow(string author)
    {
        if (FollowRepository != null && User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            FollowRepository.CreateFollow(User.Identity.Name, author);
        }

        return Redirect($"{Request.PathBase}{Request.Path}?page={CurrentPage}");
    }

    public ActionResult OnPostUnfollow(string author)
    {
        if (FollowRepository != null && User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            FollowRepository.DeleteFollow(User.Identity.Name, author);
        }

        return Redirect($"{Request.PathBase}{Request.Path}?page={CurrentPage}");
    }

    /// <summary>
    /// The method deletes a users account and all cheep and follow entities by that user.
    /// Afterwards the user is singed out of chirp and redirected to the public timeline.
    /// </summary>
    /// <param name="author"></param>
    public void OnPostDeleteAccount(string author)
    {
        if (CheepRepository != null && FollowRepository != null && AuthorRepository != null)
        {
            CheepRepository.DeleteCheepsFromAuthor(author);
            FollowRepository.DeleteFollowsFromAuthor(author);
            AuthorRepository.DeleteAuthor(author);
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        Response.Redirect("/");
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        CurrentPage = page;

        if (CheepRepository != null && FollowRepository != null && User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            int cheepCount = await CheepRepository.GetMyCheepCountAsync(User.Identity.Name);
            int pageSize = 32;

            PageCount = cheepCount / pageSize;

            if (cheepCount % pageSize != 0)
            {
                PageCount++;
            }

            if (page < 1)
            {
                page = 1;
                CurrentPage = 1;
            }

            Cheeps = await CheepRepository.GetMyCheepsAsync(User.Identity.Name, page, pageSize);
            Followers = await FollowRepository.GetFollowersAsync(User.Identity.Name);
            Followings = await FollowRepository.GetFollowingsAsync(User.Identity.Name);
            FollowersCount = await FollowRepository.GetFollowersCountAsync(User.Identity.Name);
            FollowingsCount = await FollowRepository.GetFollowingsCountAsync(User.Identity.Name);

            GetClaims();

            return Page();
        }

        return RedirectToPage();
    }

    private void GetClaims()
    {
        foreach (var claim in User.Claims)
        {
            string type = claim.Type;

            if (type.Contains("givenname"))
            {
                Claims["realname"] = claim.Value;
            }
            else if (type.Equals("name"))
            {
                Claims["username"] = claim.Value;
            }
            else if (type.Equals("emails"))
            {
                Claims["email"] = claim.Value;
            }
            else if (type.Contains("identityprovider"))
            {
                Claims["identityprovider"] = claim.Value;
            }
        }
    }
}