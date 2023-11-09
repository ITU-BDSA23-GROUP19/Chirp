using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public string Text { get; set; } = "";

    public PublicModel(ICheepRepository repository)
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

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        if (page < 1)
        {
            page = 1;
        }

        Cheeps = await _repository.GetCheepsAsync(page);

        return Page();
    }
}