using System.Collections.Specialized;
using System.Reflection.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorRepository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    [BindProperty]
    public AuthorDTO Author { get; set; }

    [BindProperty]
    public string Text { get; set; } = "";

    public PublicModel(ICheepRepository repository, IAuthorRepository authorRepository)
    {
        repository = _repository;
        authorRepository = _authorRepository;
    }


    public async Task<RedirectToPageResult> OnPostFollow(Author author)
    {
        Console.WriteLine("WOWSADOWSA");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            //var followDTO = new FollowDTO(User.Identity.Name, author.Name);
            //_authorRepository.FollowAuthor(followDTO);
            Console.WriteLine("kan jeg følge folk nu så?");
        }
        return RedirectToPage("");

    }

    public void changeText(string text)
    {

    }

    public async Task OnPostUnfollow(Author author)
    {
        Console.WriteLine("Ja nu unfollower vi");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            //mangler at fjerne en followdto fra _authorrepository - måske mangler også funktionalitet til at fjerne overhovedet??
        }

    }

    /*public IActionResult OnPost(string text)
    {
        var userEmail = User.Claims.Where(c => c.Type == "Email");
        AuthorDTO author = User.Claims.Where(c => c.Type.Equals(userName));
        var userName = User.Claims.Where(c => c.Type == "Name");
        //first we must look in the database to see if the user exists:
        if (author == null)
        {
            //check if author exists. If it doesn't:
            //create new author object.
            //then create new authorDTO object. (which then adds that author in the repository.)
            //
        }

        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {

            Text = text;
            _repository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));

        }
        return RedirectToPage("Public");
    }*/

    //public IActionResult onRegister(string )

    //@User.Claims.Where(c => c.GetType() == "")

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