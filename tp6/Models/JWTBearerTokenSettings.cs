namespace tp6.Models;
public class JWTBearerTokenSettings
{
    public string SecretKey
    {
        get;
        set;
    }
    public string Audience
    { get; set; }
    public string Issuer
    { get; set; }
    public int ExpireTimeInSeconds { get; set; }
}