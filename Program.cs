using System.Text.RegularExpressions;

namespace dt071g_projekt;

class Program
{
    static void Main(string[] args)
    {
        UserManagement userManagement = new UserManagement();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("V Ä L K O M M E N !\n");
            Console.WriteLine("L O G G A  I N / R E G I S T R E R A  D I G\n\n");
            Console.WriteLine("1. Registrera dig");
            Console.WriteLine("2. Logga in");
            Console.WriteLine("X. Avsluta programmet");
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

                        userManagement.addUser(usernameInput, passwordInput);

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
            // Rensa konsolen efter varje menyval
            Console.Clear();

        }
    }
}
