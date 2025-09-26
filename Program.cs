/* En programkod för en gästbok där inlägg kan sparas, visas, läggas till och tas bort av Jennifer Jakobsson */

using System;
using System.Collections.Generic; // importerar samlingar (för lista)
using System.IO; // hanterar filer för att spara och ladda in json-filen
using System.Text.Json; // används för att omvandla listan till json

class Guestbook
{
    static List<Post> PostsList = new List<Post>(); // skapar lista (PostsList) för att lagra inlägg (Post)
    static string FilePath = "guestbook.json"; // fil för att spara inlägg

    static void Main(string[] args) // huvudmetod
    {
        LoadPosts(); // ladda in inlägg från fil vid start

        bool pursue = true; // håller igång programmet tills avslutas

        while (pursue)
        {
            Console.Clear(); // rensa konsoll

            Console.WriteLine("\nJennifers gästbok:");
            ShowPosts(); // visar alla inlägg

            Console.WriteLine("\n1. Skriv i gästboken");
            Console.WriteLine("2. Ta bort inlägg");
            Console.WriteLine("\nX. Avbryt/Avsluta");
            Console.Write("\nVälj siffra/bokstav för önskad metod: ");
            string? choice = Console.ReadLine(); // läser valet

            switch (choice) // alternativ beroende på val
            {
                case "1":
                    AddPost(); // metod för att skriva i gästbok
                    break;
                case "2":
                    DeletePost(); // metod för att ta bort inlägg
                    break;
                case "X":
                case "x":
                    pursue = false; // avsluta
                    break;
                default:
                    Console.WriteLine("\nOgiltigt alternativ: Ange korrekt bokstav eller siffra för metod");
                    break;
            }

            Console.WriteLine("\n\nTrycka på valfri knapp för att fortsätta: ");
            Console.ReadKey(); // väntar på att knapp ska tryckas för att fortsätta
        }

        SavePosts(); // spara inlägg när programmet avslutas
    }

    class Post
    {
        public int Index { get; set; } // plats för inlägg (get set läser och sätter värde för egenskapen)
        public string Name { get; set; } // namn på person
        public string Text { get; set; } // själva inlägget

        public Post(int index, string name, string text)
        {
            Index = index;
            Name = name;
            Text = text;
        }
    }

    static void AddPost() // metod för att lägga till inlägg
    {
        Console.Write("\nAnge ditt namn: ");
        string? name = Console.ReadLine();

        if (string.Equals(name, "X", StringComparison.OrdinalIgnoreCase)) // avbryt
        {
            Console.WriteLine("- Metod avbruten");
            return;
        }

        while (string.IsNullOrWhiteSpace(name)) // säkerställer att inget lämnas tomt
        {
            Console.Write("\nOBS | Du måste ange ett namn. \nVänligen ange ditt namn: ");
            name = Console.ReadLine();

            if (string.Equals(name, "X", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("- Metod avbruten");
                return;
            }
        }

        Console.Write("Ditt tillägg i gästboken: ");
        string? text = Console.ReadLine();

        if (string.Equals(text, "X", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("- Metod avbruten");
            return;
        }

        while (string.IsNullOrWhiteSpace(text))
        {
            Console.Write("\nOBS | Du måste skriva något i inlägget. \nVänligen skriv ett inlägg: ");
            text = Console.ReadLine();

            if (string.Equals(text, "X", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("- Metod avbruten");
                return;
            }
        }

        int index = PostsList.Count + 1; // tilldelar index
        PostsList.Add(new Post(index, name!, text!));

        Console.WriteLine("\nTack för ditt bidrag till gästboken!");

        SavePosts(); // spara inlägg direkt efter tillägg
    }

    static void ShowPosts() // visar alla inlägg
    {
        if (PostsList.Count == 0) // finns inga inlägg...
        {
            Console.WriteLine("[0] Inga inlägg att visa"); // ...visas meddelande
        }
        else
        {
            foreach (var post in PostsList) // ...annars loopas alla inlägg och skrivs ut
            {
                Console.WriteLine($"[{post.Index}] {post.Name} - {post.Text}");
            }
        }
    }

    static void DeletePost() // raderar inlägg
    {
        Console.Write("Ange index för det inlägg du vill ta bort: ");
        string? input = Console.ReadLine();

        if (string.Equals(input, "X", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("- Metod avbruten");
            return;
        }

        if (int.TryParse(input, out int index)) // index som angivits letas upp i listan...
        {
            Post? toRemove = PostsList.Find(post => post.Index == index);
            if (toRemove != null)
            {
                PostsList.Remove(toRemove); // ...och tas bort
                Console.WriteLine("\nInlägget har raderats!");
                UpdateIndex(); // ...och uppdaterar listan
                SavePosts(); // spara ändringar efter radering
            }
            else
            {
                Console.WriteLine("\nInlägg med det indexet hittades inte");
            }
        }
        else
        {
            Console.WriteLine("\nOgiltigt index");
        }
    }

    static void UpdateIndex() // säkerställer korrekt index efter inlägg raderats
    {
        for (int i = 0; i < PostsList.Count; i++)
        {
            PostsList[i].Index = i + 1; // justerar index för korrekt ordning
        }
    }

    static void SavePosts() // sparar inlägg till fil som json
    {
        string json = JsonSerializer.Serialize(PostsList);
        File.WriteAllText(FilePath, json);
    }

    static void LoadPosts() // laddar in inlägg från json-fil
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            PostsList = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
        }
    }
}
