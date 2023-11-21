using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    [BindProperty]
    public string Text { get; set; } "";

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public IActionResult OnPost(string text)
    {
        /*if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            _repository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));
            Console.WriteLine("The onPost method is accessed.");

        }
        Console.WriteLine("The onPost method is accessed.");
        return RedirectToPage("Public");*/

        return RedirectToPage("Public");
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        CurrentPage = page;

        int cheepCount = await _repository.GetCheepCountAsync();
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

        Cheeps = await _repository.GetCheepsAsync(page, pageSize);

        return Page();
    }
}