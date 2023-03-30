namespace Tugas_4;

public class User
{
    private static uint _id = 1;
    public uint Id;
    public string FirstName;
    public string LastName;
    public string Password;
    public string Username;

    public string FullName => $"{FirstName} {LastName}";

    public User(string firstName, string lastName, string password)
    {
        Validation.FirstNameValidation(firstName);
        Validation.LastNameValidation(lastName);
        Validation.PasswordValidation(password);

        Id = _id++;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Username = UserService.GenerateUsername(FirstName, LastName);
    }

    public override string ToString()
    {
        return "==============================\n" +
               $"Id : {Id}\n" +
               $"FirstName : {FirstName}\n" +
               $"LastName : {LastName}\n" +
               $"Username : {Username}\n" +
               $"Password : {Password}\n" +
               "==============================\n";
    }
}