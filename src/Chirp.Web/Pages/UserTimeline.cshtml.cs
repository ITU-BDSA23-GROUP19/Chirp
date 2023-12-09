using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _arepository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();

    [BindProperty]
    public AuthorDTO Author { get; set; }
    [BindProperty]
    public CheepDTO Cheep { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    [BindProperty]
    public string Text { get; set; } = "";

    public UserTimelineModel(ICheepRepository repository, IAuthorRepository arepository)
    {
        _repository = repository;
        _arepository = arepository;
    }

    //Get author email and name from azure
    //Save that email and name in an author
    //Insert that author into our database
    public void SignInAsync()
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(userEmailClaim) && !string.IsNullOrEmpty(userNameClaim))
            {
                var userEmail = userEmailClaim.ToString();
                var userName = userNameClaim.ToString();

                var authorExists = await _arepository.GetAuthorFromEmailAsync(userEmail);

                if (authorExists == null) {
                    var newAuthor = new AuthorDTO(userName, userEmail);
                    await _arepository.CreateAuthor(newAuthor);
                }

            }
        }
    }

    public void OnPost(string text)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            var authorName = User.Identity.Name; 
            _repository.CreateCheep(new CheepDTO(authorName, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
        }
    }

    public async Task<ActionResult> OnGetAsync(string author, [FromQuery] int page)
    {
        CurrentPage = page;

        int cheepCount = await _repository.GetCheepCountFromAuthorAsync(author);
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

        Cheeps = await _repository.GetCheepsFromAuthorAsync(author, page, pageSize);

        return Page();
    }
}