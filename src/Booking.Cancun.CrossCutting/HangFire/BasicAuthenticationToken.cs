namespace Booking.Cancun.Infraestructure.CrossCutting.HangFire;

public class BasicAuthenticationToken
{
    private readonly string[] _tokens;

    public string Username => _tokens[0];
    public string Password => _tokens[1];

    public BasicAuthenticationToken(string[] tokens)
    {
        _tokens = tokens;
    }

    public bool AreInvalid()
    {
        return ContainsTwo_Tokens() && ValidTokenValue(Username) && ValidTokenValue(Password);
    }

    public bool CredentialsMatch(string user, string pass)
    {
        return Username.Equals(user) && Password.Equals(pass);
    }

    private bool ValidTokenValue(string token)
    {
        return string.IsNullOrWhiteSpace(token);
    }

    private bool ContainsTwo_Tokens()
    {
        return _tokens.Length == 2;
    }
}
