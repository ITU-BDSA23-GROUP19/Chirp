using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public int PageCount { get; set; } = 0;
    public string Text { get; set; } = "";

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public void OnPost(string text)
    {
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            _repository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
        }
    }

    public async Task<ActionResult> OnGetAsync(string author, [FromQuery] int page)
    {
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
        }

        Cheeps = await _repository.GetCheepsFromAuthorAsync(author, page, pageSize);

        return Page();
    }
}