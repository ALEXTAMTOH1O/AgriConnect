using System.Text.RegularExpressions;

namespace AgriConnect;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent(); // Rebuild trigger
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Erreur", "Veuillez entrer votre nom.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            await DisplayAlert("Erreur", "Veuillez entrer une adresse email valide.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Erreur", "Veuillez entrer votre mot de passe.", "OK");

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new System.Globalization.IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
