using System.Text.RegularExpressions;

namespace dt071g_projekt;

class Program
{
    static void Main(string[] args)
    {
        UserManagement userManagement = new UserManagement();
        Balance userBalance = new Balance();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("V Ä L K O M M E N !\n");
            Console.WriteLine("L O G G A  I N / R E G I S T R E R A  D I G\n\n");
            Console.WriteLine("1. Registrera dig");
            Console.WriteLine("2. Logga in");
            Console.WriteLine("X. Avsluta programmet\n");
            Console.Write("Välj 1 eller 2: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.Clear();
                Console.WriteLine("Du måste ange 1 eller 2!");
                continue;
            }
            switch (input)
            {
                case "1":
                    while (true)
                    {

                        Console.Clear();
                        Console.Write("Skriv ett användarnamn: ");
                        string? usernameInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(usernameInput))
                        {
                            Console.Clear();
                            Console.WriteLine("Du måste ange ett namn");
                            continue;
                        }

                        /* Använder regex för att kontrollera om namnet innehåller endast bokstäver
                        Regex: ^[a-zA-ZåäöÅÄÖ]+$ tillåter endast bokstäver (inklusive svenska bokstäver)*/
                        if (!Regex.IsMatch(usernameInput, @"^[a-zA-ZåäöÅÄÖ\s]+$"))
                        {
                            Console.Clear();
                            Console.WriteLine("Namnet får endast innehålla bokstäver. Försök igen.");
                            continue;
                        }

                        Console.Write("Skriv ett lösenord: ");
                        string? passwordInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(passwordInput))
                        {
                            Console.Clear();
                            Console.WriteLine("Du måste ange ett lösenord");
                            continue;
                        }

                        // Kollar så att man inte har ett för enkelt lösenord
                        if (passwordInput.Length < 8)
                        {
                            Console.Clear();
                            Console.WriteLine("Lösenordet måste vara minst 8 tecken långt");
                            continue;
                        }

                        userManagement.addUser(usernameInput, passwordInput);

                        // Bekräfta att användaren har registrerats och gå tillbaka till huvudmenyn
                        Console.Clear();
                        Console.WriteLine("Registrering lyckades! Klicka på valfri tangent för att gå till huvudmenyn!");
                        Console.ReadKey();  // Väntar på att användaren trycker på en tangent innan de återvänder till huvudmenyn

                        break;
                    }
                    break;
                case "2":
                    while (true)
                    {

                        Console.Clear();
                        Console.Write("Skriv ditt användarnamn: ");
                        string? usernameInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(usernameInput))
                        {
                            Console.Clear();
                            Console.WriteLine("Du måste ange ett namn");
                            continue;
                        }

                        /* Använder regex för att kontrollera om namnet innehåller endast bokstäver
                        Regex: ^[a-zA-ZåäöÅÄÖ]+$ tillåter endast bokstäver (inklusive svenska bokstäver)*/
                        if (!Regex.IsMatch(usernameInput, @"^[a-zA-ZåäöÅÄÖ\s]+$"))
                        {
                            Console.Clear();
                            Console.WriteLine("Namnet får endast innehålla bokstäver. Försök igen.");
                            continue;
                        }

                        Console.Write("Skriv ditt lösenord: ");
                        string? passwordInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(passwordInput))
                        {
                            Console.Clear();
                            Console.WriteLine("Du måste ange ett lösenord");
                            continue;
                        }

                        if (userManagement.loginUser(usernameInput, passwordInput) == false)
                        {
                            Console.WriteLine("Lösenord/Användarnamn är felaktigt");
                            continue;
                        }

                        // När inloggningen lyckats, hämta och visa användarens saldo
                        Console.Clear();
                        decimal balance = userBalance.GetBalance(usernameInput);
                        Console.WriteLine($"Inloggad som {usernameInput}. Ditt saldo är: {balance} kr");

                        Console.WriteLine("Vad vill du göra?");
                        Console.WriteLine("1. Sätta in pengar");
                        Console.WriteLine("2. Ta ut pengar");
                        Console.WriteLine("X. Logga ut");
                        Console.Write("Skriv 1, 2 eller X: ");
                        string? action = Console.ReadLine();

                        switch (action)
                        {
                            case "1":
                                Console.Write("Hur mycket vill du sätta in?\n ");
                                try
                                {
                                    decimal insert = Convert.ToDecimal(Console.ReadLine());
                                    balance += insert;
                                    userBalance.updateBalance(usernameInput, balance);
                                    Console.WriteLine($"Du har satt in {insert} kr. Nytt saldo: {balance} kr");
                                    break;
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Ogiltigt belopp, försök igen.");
                                    continue; // Gå tillbaka till insättningsmenyn
                                }


                            case "2":
                                Console.WriteLine("Hur mycket vill du ta ut?\n ");
                                try
                                {
                                    decimal withdrawal = Convert.ToDecimal(Console.ReadLine());
                                    if (withdrawal > balance)
                                    {
                                        Console.WriteLine("Du har inte tillräckligt med pengar på ditt konto.");
                                    }
                                    else
                                    {
                                        balance -= withdrawal;
                                        userBalance.updateBalance(usernameInput, balance);
                                        Console.WriteLine($"Du har tagit ut {withdrawal} kr. Nytt saldo: {balance} kr");
                                    }
                                    break;
                                }catch (FormatException)
                                {
                                    Console.WriteLine("Ogiltigt belopp, försök igen.");
                                    continue; // Gå tillbaka till uttagsmenyn
                                }


                            case "X":
                            case "x":
                                Console.Clear();
                                Console.WriteLine("Du har loggat ut. Klicka på valfri tangent för att gå vidare");
                                Console.ReadKey(); // Väntar på att användaren trycker på en tangent
                                continue; // Gå tillbaka till huvudmenyn
                            default:
                                Console.WriteLine("Ogiltigt val, försök igen.");
                                break;
                        }

                        break;
                    }
                    break;
                case "X":
                case "x":
                    Console.WriteLine("Avslutar programmet.");
                    return;  // Avslutar loopen och programmet
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }
}
