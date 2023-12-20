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
    public int FollowersCount { get; set; }
    public int FollowingsCount { get; set; }
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

    public ActionResult OnPost(string text)
    {
        if (CheepRepository != null && User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            CheepRepository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnGetAsync(string author, [FromQuery] int page)
    {
        CurrentPage = page;

        if (CheepRepository != null && FollowRepository != null)
        {
            IEnumerable<string> followings = await FollowRepository.GetFollowingsAsync(author);
            int cheepCount = await CheepRepository.GetUserTimelineCheepCountAsync(author, followings);
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

            Cheeps = await CheepRepository.GetUserTimelineCheepsAsync(author, followings, page, pageSize);
            FollowersCount = await FollowRepository.GetFollowersCountAsync(author);
            FollowingsCount = await FollowRepository.GetFollowingsCountAsync(author);

            return Page();
        }

        return RedirectToPage();
    }
}