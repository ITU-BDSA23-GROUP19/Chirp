using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    public ICheepRepository CheepRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public IFollowRepository FollowRepository { get; private set; }
    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public string Text { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowRepository followRepository)
    {
        CheepRepository = cheepRepository;
        AuthorRepository = authorRepository;
        FollowRepository = followRepository;

        Cheeps = new List<CheepDTO>();
        Text = "";
    }

    public async Task<ActionResult> OnPostFollow(Author author)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            FollowRepository.CreateFollow(new FollowDTO(User.Identity.Name), new FollowDTO("Jacqualine Gilcoine"));
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostUnfollow(Author author)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            FollowRepository.DeleteFollow(new FollowDTO(User.Identity.Name), new FollowDTO("Jacqualine Gilcoine"));
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPost(string text)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            CheepRepository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnGetAsync(string author, [FromQuery] int page)
    {
        CurrentPage = page;

        if (CheepRepository != null)
        {
            int cheepCount = await CheepRepository.GetCheepCountFromAuthorAsync(author);
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

            Cheeps = await CheepRepository.GetCheepsFromAuthorAsync(author, page, pageSize);

            return Page();
        }

        return RedirectToPage();
    }
}