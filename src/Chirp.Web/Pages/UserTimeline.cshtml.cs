using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
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

        Cheeps = _repository.GetCheepsFromAuthor(author, pageNumber);
        return Page();
    }
}
