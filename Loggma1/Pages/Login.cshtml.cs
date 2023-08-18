using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loggma1.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnPost(string username, string password)
        {
            // Burada kullanýcý adý ve parola kontrolünü gerçekleþtirin.
            // Eðer giriþ baþarýlý ise yönlendirme yapabilirsiniz.

            // Örnek olarak, doðrudan ana sayfaya yönlendirme yapabilirsiniz:
            return RedirectToPage("/Index");

            // Veya bir baþka sayfaya yönlendirme yapabilirsiniz:
            // return RedirectToPage("/AnotherPage");
        }
    }
}
