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
            // Burada kullan�c� ad� ve parola kontrol�n� ger�ekle�tirin.
            // E�er giri� ba�ar�l� ise y�nlendirme yapabilirsiniz.

            // �rnek olarak, do�rudan ana sayfaya y�nlendirme yapabilirsiniz:
            return RedirectToPage("/Index");

            // Veya bir ba�ka sayfaya y�nlendirme yapabilirsiniz:
            // return RedirectToPage("/AnotherPage");
        }
    }
}
