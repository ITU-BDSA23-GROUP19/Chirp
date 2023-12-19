using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    public ICheepRepository CheepRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public IFollowRepository FollowRepository { get; private set; }
    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowRepository followRepository)
    {
        CheepRepository = cheepRepository;
        AuthorRepository = authorRepository;
        FollowRepository = followRepository;

        Cheeps = new List<CheepDTO>();
        Name = "";
        Text = "";
    }

    public ActionResult OnPostFollow(string author)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            FollowRepository.CreateFollow(User.Identity.Name, author);
        }

        return Redirect($"{Request.PathBase}{Request.Path}?page={CurrentPage}");
    }

    public IActionResult OnPostUnfollow(string author)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            FollowRepository.DeleteFollow(User.Identity.Name, author);
        }

        return Redirect($"{Request.PathBase}{Request.Path}?page={CurrentPage}");
    }

    public ActionResult OnPost(string text)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            CheepRepository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
        }

        return RedirectToPage();
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        CurrentPage = page;

        if (CheepRepository != null)
        {
            int cheepCount = await CheepRepository.GetCheepCountAsync();
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

            Cheeps = await CheepRepository.GetCheepsAsync(page, pageSize);

            return Page();
        }

        return RedirectToPage();
    }
}