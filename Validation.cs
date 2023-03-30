namespace Tugas_4;

public struct Validation
{
    // min name
    private const int MinFirstName = 2;
    private const int MinLastName = 2;
    // min password length
    private const int MinPassword = 8;

    private static readonly Exception PasswordInvalid = new Exception("Password must have at least 8 characters, contains uppercase, lowercase and number."); 
    private static readonly Exception FirstNameInvalid = new Exception("Firstname must have at least 2 characters"); 
    private static readonly Exception LastNameInvalid = new Exception("Lastname must have at least 2 characters"); 

    public static void PasswordValidation(string password)
    {
        if (password is null || password.Length < MinPassword) throw PasswordInvalid;
        
        bool isContainUppercase = false;
        bool isContainLowercase = false;
        bool isContainNumber = false;
        
        foreach (var p in password)
        {
            if (Char.IsLetter(p) && Char.IsUpper(p)) isContainUppercase = true;
            if (Char.IsLetter(p) && Char.IsLower(p)) isContainLowercase = true;
            if (Char.IsNumber(p)) isContainNumber = true;
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