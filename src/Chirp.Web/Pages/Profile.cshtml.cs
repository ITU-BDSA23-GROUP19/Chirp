using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ProfileModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IFollowRepository _followRepository;

    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public IEnumerable<FollowDTO> Follows { get; set; }

    public int CurrentPage { get; set; }
    public int PageCount { get; set; }

    public ProfileModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowRepository followRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _followRepository = followRepository;

        Cheeps = new List<CheepDTO>();
        Follows = new HashSet<FollowDTO>();

    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        CurrentPage = page;

        if (_cheepRepository != null && User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            int cheepCount = await _cheepRepository.GetCheepCountFromAuthorAsync(User.Identity.Name);
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

            Cheeps = await _cheepRepository.GetCheepsFromAuthorAsync(User.Identity.Name, page, pageSize);

            return Page();
        }

        return RedirectToPage();
    }

    public void DeleteAccount(string author, FollowDTO followDTO)
    {
        _cheepRepository.DeleteCheepsFromAuthor(author);
        _followRepository.DeleteFollows(followDTO);
        _authorRepository.DeleteAuthor(author);
    }
}