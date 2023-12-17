using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;

    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public string Text { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;

        Cheeps = new List<CheepDTO>();
        Text = "";
    }

    public async Task<ActionResult> OnPostFollow(Author author)
    {
        Console.WriteLine("Following");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostUnfollow(Author author)
    {
        Console.WriteLine("Unfollowing");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPost(string text)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            _cheepRepository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnGetAsync(string author, [FromQuery] int page)
    {
        CurrentPage = page;

        if (_cheepRepository != null)
        {
            int cheepCount = await _cheepRepository.GetCheepCountFromAuthorAsync(author);
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

            Cheeps = await _cheepRepository.GetCheepsFromAuthorAsync(author, page, pageSize);

            return Page();
        }

        return RedirectToPage();
    }
}