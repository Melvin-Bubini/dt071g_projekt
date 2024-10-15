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
            clearAndShowTitle();
            Console.WriteLine("V Ä L K O M M E N !");
            Console.WriteLine("L O G G A  I N / R E G I S T R E R A  D I G\n\n");
            Console.WriteLine("1. Registrera dig");
            Console.WriteLine("2. Logga in");
            Console.WriteLine("X. Avsluta programmet\n");
            Console.Write("Välj 1 eller 2: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                clearAndShowTitle();
                Console.WriteLine("Du måste ange 1 eller 2!");
                continue;
            }
            switch (input)
            {
                case "1":
                    while (true)
                    {

                        clearAndShowTitle();
                        Console.Write("Skriv ett användarnamn: ");
                        string? usernameInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(usernameInput))
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Du måste ange ett namn");
                            Console.ReadKey();
                            continue;
                        }

                        /* Använder regex för att kontrollera om namnet innehåller endast bokstäver
                        Regex: ^[a-zA-ZåäöÅÄÖ]+$ tillåter endast bokstäver (inklusive svenska bokstäver)*/
                        if (!Regex.IsMatch(usernameInput, @"^[a-zA-ZåäöÅÄÖ\s]+$"))
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Namnet får endast innehålla bokstäver. Försök igen.");
                            Console.ReadKey();
                            continue;
                        }

                        Console.Write("Skriv ett lösenord: ");
                        string? passwordInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(passwordInput))
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Du måste ange ett lösenord");
                            Console.ReadKey();
                            continue;
                        }

                        // Kollar så att man inte har ett för enkelt lösenord
                        if (passwordInput.Length < 8)
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Lösenordet måste vara minst 8 tecken långt");
                            Console.ReadKey();
                            continue;
                        }

                        userManagement.addUser(usernameInput, passwordInput);

                        // Bekräfta att användaren har registrerats och gå tillbaka till huvudmenyn
                        clearAndShowTitle();
                        Console.WriteLine("Registrering lyckades! Klicka på valfri tangent för att gå till huvudmenyn!");
                        Console.ReadKey();  // Väntar på att användaren trycker på en tangent innan de återvänder till huvudmenyn

                        break;
                    }
                    break;
                case "2":
                    while (true)
                    {

                        clearAndShowTitle();
                        Console.Write("Skriv ditt användarnamn: ");
                        string? usernameInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(usernameInput))
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Du måste ange ett namn");
                            Console.ReadKey();
                            continue;
                        }

                        /* Använder regex för att kontrollera om namnet innehåller endast bokstäver
                        Regex: ^[a-zA-ZåäöÅÄÖ]+$ tillåter endast bokstäver (inklusive svenska bokstäver)*/
                        if (!Regex.IsMatch(usernameInput, @"^[a-zA-ZåäöÅÄÖ\s]+$"))
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Namnet får endast innehålla bokstäver. Försök igen.");
                            Console.ReadKey();
                            continue;
                        }

                        Console.Write("Skriv ditt lösenord: ");
                        string? passwordInput = Console.ReadLine();

                        if (string.IsNullOrEmpty(passwordInput))
                        {
                            clearAndShowTitle();
                            Console.WriteLine("Du måste ange ett lösenord");
                            Console.ReadKey();
                            continue;
                        }

                        if (userManagement.loginUser(usernameInput, passwordInput) == false)
                        {
                            Console.WriteLine("Lösenord/Användarnamn är felaktigt, försök igen.");
                            Console.ReadKey();
                            continue;
                        }

                        // När inloggningen lyckats, hämta och visa användarens saldo
                        decimal balance = userBalance.GetBalance(usernameInput);
                        bool isLoggedIn = true;

                        while (isLoggedIn)
                        {

                            clearAndShowTitle();
                            Console.WriteLine($"Inloggad som {usernameInput}. Ditt saldo är: {balance} kr\n");
                            Console.WriteLine("Vad vill du göra?");
                            Console.WriteLine("1. Sätta in pengar");
                            Console.WriteLine("2. Ta ut pengar");
                            Console.WriteLine("3. Radera användare");
                            Console.WriteLine("X. Logga ut\n");
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
                                        Console.ReadKey();
                                        break;
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Ogiltigt belopp, försök igen.");
                                        Console.ReadKey();
                                        continue; // Gå tillbaka till insättningsmenyn
                                    }


                                case "2":
                                    Console.Write("Hur mycket vill du ta ut?\n ");
                                    try
                                    {
                                        decimal withdrawal = Convert.ToDecimal(Console.ReadLine());
                                        if (withdrawal > balance)
                                        {
                                            Console.WriteLine("Du har inte tillräckligt med pengar på ditt konto.");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            balance -= withdrawal;
                                            userBalance.updateBalance(usernameInput, balance);
                                            Console.WriteLine($"Du har tagit ut {withdrawal} kr. Nytt saldo: {balance} kr");
                                            Console.ReadKey();
                                        }
                                        break;
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Ogiltigt belopp, försök igen.");
                                        Console.ReadKey();
                                        continue; // Gå tillbaka till uttagsmenyn
                                    }

                                case "3":
                                    clearAndShowTitle();
                                    Console.WriteLine("Är du säker på att du vill radera användaren?\n 1. Ja\n 2.Nej");
                                    string? deleteInput = Console.ReadLine();
                                    if (deleteInput == "1")
                                    {
                                        userManagement.deleteUser(usernameInput);  // Raderar användaren om användaren valde "1"
                                        Console.WriteLine("Användare raderad! Klicka på valfri tangent för att gå vidare");
                                        Console.ReadKey();
                                    }
                                    else if (deleteInput == "2")
                                    {
                                        Console.WriteLine("Användaren har inte raderats.");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ogiltigt val! Skriv 1 för ja eller 2 för nej!");
                                        Console.ReadKey();
                                    }
                                    break;
                                case "X":
                                case "x":
                                    clearAndShowTitle();
                                    Console.WriteLine("Du har loggat ut. Klicka på valfri tangent för att gå vidare");
                                    Console.ReadKey(); // Väntar på att användaren trycker på en tangent
                                    isLoggedIn = false;  // Logga ut användaren
                                    break;
                                default:
                                    Console.WriteLine("Ogiltigt val, försök igen.");
                                    Console.ReadKey();
                                    break;
                            }
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

    static void clearAndShowTitle() // Metod för att alltid visa bankens namn i konsollen
    {
        Console.Clear();
        Console.WriteLine("B U B I N I  B A N K\n");
    }
}


