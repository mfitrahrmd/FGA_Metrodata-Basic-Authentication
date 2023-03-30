namespace Tugas_4;

public struct Validation
{
    // min firstname length
    private const int MinFirstName = 2;

    // min firstname length
    private const int MinLastName = 2;

    // min password length
    private const int MinPassword = 8;

    private static readonly Exception PasswordInvalid =
        new($"Password must have at least {MinPassword} characters, contains uppercase, lowercase and number.");

    private static readonly Exception FirstNameInvalid = new($"Firstname must have at least {MinFirstName} characters");
    private static readonly Exception LastNameInvalid = new($"Lastname must have at least {MinLastName} characters");

    public static void PasswordValidation(string password)
    {
        if (password is null || password.Length < MinPassword) throw PasswordInvalid;

        var isContainUppercase = false;
        var isContainLowercase = false;
        var isContainNumber = false;

        foreach (var p in password)
        {
            if (char.IsLetter(p) && char.IsUpper(p)) isContainUppercase = true;
            if (char.IsLetter(p) && char.IsLower(p)) isContainLowercase = true;
            if (char.IsNumber(p)) isContainNumber = true;
        }

        if (!(isContainUppercase && isContainLowercase && isContainNumber)) throw PasswordInvalid;
    }

    public static void FirstNameValidation(string firstName)
    {
        if (firstName is null || firstName.Length < MinFirstName) throw FirstNameInvalid;
    }

    public static void LastNameValidation(string lastName)
    {
        if (lastName is null || lastName.Length < MinLastName) throw LastNameInvalid;
    }
}