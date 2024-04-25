using System;
using System.Collections.Generic;
using System.IO;

class Utas
{
    public string Nev { get; set; }
    public string Cim { get; set; }
    public string Telefonszam { get; set; }
}

class Utazas
{
    public string Uticel { get; set; }
    public decimal Ar { get; set; }
    public int MaxLetszam { get; set; }
    public List<Utas> Jelentkezettek { get; set; }

    public Utazas()
    {
        Jelentkezettek = new List<Utas>();
    }
}

class Program
{
    static List<Utas> utasok = new List<Utas>();
    static List<Utazas> utazasok = new List<Utazas>();
    private static int selectedMenuIndex = 1;

    public static void Main(string[] args)
    {
        LoadData();
        while (true)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                menu();

                key = Console.ReadKey();
                AdjustSelectedIndex(key);

            } while (key.Key != ConsoleKey.Enter);

            if (!HandleMenuSelection(selectedMenuIndex))
            {
                break;
            }
        }
    }

    private static void menu()
    {
        Console.WriteLine("Válasszon sor:");
        menuItem("Új utas felvétele", 1);
        menuItem("Utas adatainak módosítása", 2);
        menuItem("Új utazás felvétele", 3);
        menuItem("Utazásra jelentkezés", 4);
        menuItem("Utaslista nyomtatás", 5);
        menuItem("Kilépés", 6);
    }

    private static void menuItem(string text, int menuIndex)
    {
        if (menuIndex == selectedMenuIndex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{text} [<--]");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine(text);
        }
    }

    private static void AdjustSelectedIndex(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.DownArrow:
                selectedMenuIndex = (selectedMenuIndex % 6) + 1;
                break;
            case ConsoleKey.UpArrow:
                selectedMenuIndex = (selectedMenuIndex - 2 + 6) % 6 + 1;
                break;
        }
    }

    private static bool HandleMenuSelection(int selectedMenuIndex)
    {
        if (selectedMenuIndex == 1)
        {
            UjUtasFelvelese();
        }
        else if (selectedMenuIndex == 2)
        {
            UtasAdatokModositasa();
        }
        else if (selectedMenuIndex == 3)
        {
            UjUtazasFelvetele();
        }
        else if (selectedMenuIndex == 4)
        {
            UtazasraJelentkezes();
        }
        else if (selectedMenuIndex == 5)
        {
            UtaslistaNyomtatas();
        }
        else if (selectedMenuIndex == 6)
        {
            SaveData();
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Érvénytelen választás. Kérem, válasszon újra.");
        }
        Console.WriteLine("Nyomj meg egy gombot a folytatáshoz...");
        Console.ReadKey();
        return true;
    }

    static void UjUtasFelvelese()
    {
        Utas utas = new Utas();
        Console.WriteLine("Add meg az utas nevét:");
        utas.Nev = Console.ReadLine();
        Console.WriteLine("Add meg az utas címét:");
        utas.Cim = Console.ReadLine();
        Console.WriteLine("Add meg az utas telefonszámát:");
        utas.Telefonszam = Console.ReadLine();
        utasok.Add(utas);
        SaveData();
    }

    static void UtasAdatokModositasa()
    {
        Console.WriteLine("Add meg a módosítani kívánt utas nevét:");
        string nev = Console.ReadLine();
        Utas kivalasztottUtas = utasok.Find(u => u.Nev == nev);
        if (kivalasztottUtas != null)
        {
            Console.WriteLine("Add meg az új címet:");
            kivalasztottUtas.Cim = Console.ReadLine();
            Console.WriteLine("Add meg az új telefonszámot:");
            kivalasztottUtas.Telefonszam = Console.ReadLine();
            SaveData();
            Console.WriteLine("Az utas adatai sikeresen módosítva!");
        }
        else
        {
            Console.WriteLine("Nem található ilyen nevű utas!");
        }
    }

    static void UjUtazasFelvetele()
    {
        Utazas utazas = new Utazas();
        Console.WriteLine("Add meg az úticélt:");
        utazas.Uticel = Console.ReadLine();
        Console.WriteLine("Add meg az út árát:");
        utazas.Ar = Convert.ToDecimal(Console.ReadLine());
        Console.WriteLine("Add meg a maximális létszámot:");
        utazas.MaxLetszam = Convert.ToInt32(Console.ReadLine());
        utazasok.Add(utazas);
        SaveData();
    }

    static void UtazasraJelentkezes()
    {
        Console.WriteLine("Add meg az utas nevét:");
        string nev = Console.ReadLine();
        Utas kivalasztottUtas = utasok.Find(u => u.Nev == nev);
        if (kivalasztottUtas != null)
        {
            Console.WriteLine("Add meg az utazás nevét, amire jelentkezni szeretnél:");
            string utazasNeve = Console.ReadLine();
            Utazas kivalasztottUtazas = utazasok.Find(u => u.Uticel == utazasNeve);
            if (kivalasztottUtazas != null && kivalasztottUtazas.Uticel == utazasNeve)
            {
                Console.WriteLine("Add meg az előleget:");
                decimal eloleg = Convert.ToDecimal(Console.ReadLine());
                if (eloleg <= kivalasztottUtazas.Ar)
                {
                    kivalasztottUtazas.Jelentkezettek.Add(kivalasztottUtas);
                    SaveData();
                    Console.WriteLine("Sikeres jelentkezés az útra!");
                }
                else
                {
                    Console.WriteLine("Az előleg nem lehet több az út áránál!");
                }
            }
            else
            {
                Console.WriteLine("Nem található ilyen nevű utazás!");
            }
        }
        else
        {
            Console.WriteLine("Nem található ilyen nevű utas!");
        }
    }

    static void UtaslistaNyomtatas()
    {
        foreach (Utazas utazas in utazasok)
        {
            Console.WriteLine("Úticél: " + utazas.Uticel);
            Console.WriteLine("Ár: " + utazas.Ar);
            Console.WriteLine("Jelentkezettek:");
            foreach (Utas utas in utazas.Jelentkezettek)
            {
                Console.WriteLine("- " + utas.Nev);
            }
            Console.WriteLine();
        }
    }

    static void SaveData()
    {
        using (StreamWriter writer = new StreamWriter("adatok.txt"))
        {
            writer.WriteLine("### UTASOK ###");
            foreach (Utas utas in utasok)
            {
                writer.WriteLine("Név: " + utas.Nev);
                writer.WriteLine("Cím: " + utas.Cim);
                writer.WriteLine("Telefonszám: " + utas.Telefonszam);
                writer.WriteLine();
            }
            writer.WriteLine("### UTAZÁSOK ###");
            foreach (Utazas utazas in utazasok)
            {
                writer.WriteLine("Úticél: " + utazas.Uticel);
                writer.WriteLine("Ár: " + utazas.Ar);
                writer.WriteLine("Maximális létszám: " + utazas.MaxLetszam);
                writer.WriteLine("Jelentkezettek:");
                foreach (Utas utas in utazas.Jelentkezettek)
                {
                    writer.WriteLine("- " + utas.Nev);
                }
                writer.WriteLine();
            }
            Console.WriteLine("Adatok sikeresen mentve!");
        }
    }

    static void LoadData()
    {
        if (File.Exists("adatok.txt"))
        {
            string[] lines = File.ReadAllLines("adatok.txt");
            int index = Array.IndexOf(lines, "### UTAZÁSOK ###");
            LoadUtasok(lines, 1, index - 1);
            LoadUtazasok(lines, index + 1, lines.Length - 1);
        }
    }

    static void LoadUtasok(string[] lines, int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i += 4)
        {
            Utas utas = new Utas();
            utas.Nev = lines[i].Split(':')[1].Trim();
            utas.Cim = lines[i + 1].Split(':')[1].Trim();
            utas.Telefonszam = lines[i + 2].Split(':')[1].Trim();
            utasok.Add(utas);
        }
    }

    static void LoadUtazasok(string[] lines, int startIndex, int endIndex)
    {
        Utazas utazas = null;
        for (int i = startIndex; i < endIndex; i++)
        {
            if (lines[i].StartsWith("Úticél:"))
            {
                utazas = new Utazas();
                utazas.Uticel = lines[i].Split(':')[1].Trim();
            }
            else if (lines[i].StartsWith("Ár:"))
            {
                utazas.Ar = decimal.Parse(lines[i].Split(':')[1].Trim());
            }
            else if (lines[i].StartsWith("Maximális létszám:"))
            {
                utazas.MaxLetszam = int.Parse(lines[i].Split(':')[1].Trim());
            }
            else if (lines[i].StartsWith("Jelentkezettek:"))
            {
                i++;
                while (!string.IsNullOrEmpty(lines[i]) && !lines[i].StartsWith("###"))
                {
                    Utas utas = utasok.Find(u => u.Nev == lines[i].Trim('-'));
                    if (utas != null)
                    {
                        utazas.Jelentkezettek.Add(utas);
                    }
                    i++;
                }
                utazasok.Add(utazas);
            }
        }
    }
}