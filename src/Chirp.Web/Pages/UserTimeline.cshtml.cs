using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        Cheeps = new List<CheepViewModel>();
    }

    public ActionResult OnGet(string author)
    {
        string? page = Request.Query["page"];

        int pageNumber = 1;
        if (page != null)
        {
            pageNumber = int.Parse(page);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
        }

        Cheeps = _service.GetCheepsFromAuthor(author, pageNumber);
        return Page();
    }
}
