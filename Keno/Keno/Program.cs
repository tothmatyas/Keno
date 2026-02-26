using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Keno
{
    class NapiKeno
    {
        public int Ev { get; }
        public int Het { get; }
        public int Nap { get; }
        public string HuzasDatum { get; }
        public List<int> HuzottSzamok { get; }

        // 3. feladat:
        public NapiKeno(string sor)
        {

            var mezok = sor.Split(';');

            Ev = int.Parse(mezok[0]);
            Het = int.Parse(mezok[1]);
            Nap = int.Parse(mezok[2]);
            HuzasDatum = mezok[3];

            HuzottSzamok = new List<int>();
            for (int i = 4; i < mezok.Length; i++)
            {

                if (int.TryParse(mezok[i], out int szam))
                    HuzottSzamok.Add(szam);
            }
        }

        // 4. feladat: 
        public int TalalatSzam(List<int> tippek)
        {
            int db = 0;
            foreach (var t in tippek)
                if (HuzottSzamok.Contains(t)) db++;
            return db;
        }

        // 5. feladat:
        public bool Helyes
        {
            get
            {
                if (HuzottSzamok.Count != 20) return false;
                return HuzottSzamok.Distinct().Count() == 20;
            }
        }
        internal class Program
    {
        static void Main(string[] args)
        {
                List<NapiKeno> huzasok = new List<NapiKeno>();

          
                string fajlNev = "Huzasok.csv";

                var sorok = File.ReadAllLines(fajlNev).Skip(1); 
                foreach (var sor in sorok)
                {
                    huzasok.Add(new NapiKeno(sor));
                }

                Console.WriteLine("6. feladat: Állomány beolvasása sikeres!");

                int hibasDb = huzasok.Count(h => !h.Helyes);
                Console.WriteLine($"7. feladat: Hibás sorok száma:{hibasDb}");

               
                var joHuzasok = huzasok.Where(h => h.Helyes).ToList();

               // 9.feladat
                Console.WriteLine("9. feladat: Nyeremény számítása");


                var utolso = joHuzasok
                    .OrderBy(h => DateTime.ParseExact(h.HuzasDatum, "yyyy.MM.dd", CultureInfo.InvariantCulture))
                    .Last();

        
                List<int> tippek;
                while (true)
                {
                    Console.Write("Kérem a tippjét! Vesszővel elválasztva sorolja fel a számokat:");
                    string input = Console.ReadLine() ?? "";

                    tippek = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(x => int.Parse(x.Trim()))
                                  .ToList();

                    if (tippek.Count >= 1 && tippek.Count <= 10)
                        break;

                    Console.WriteLine("A játéktípus 1..10 lehet!");
                }

       
                int osszeg;
                while (true)
                {
                    Console.Write("Kérem a fogadási összeget! :");
                    if (int.TryParse(Console.ReadLine(), out osszeg) &&
                        osszeg >= 200 && osszeg <= 1000 &&
                        osszeg % 200 == 0)
                    {
                        break;
                    }

                    Console.WriteLine("Hibás összeg!");
                }

                int sz = Szorzo(utolso, tippek);
                int nyeremeny = osszeg * sz;

                if (nyeremeny == 0)
                    Console.WriteLine("Sajnos nem nyert!");
                else
                    Console.WriteLine($"Nyereménye:{nyeremeny}");

                //10.feladat
                Console.WriteLine();
                Console.WriteLine("10. feladat:");

                var fixTippek = new List<int> { 17, 28, 32, 44, 54, 63, 72, 75 };
                int tet = 4;
                int napiAr = 200 * tet;

                Console.WriteLine($"8-as játék 2020-ban, tét:{tet}X [{string.Join(",", fixTippek)}]");

                var ev2020 = joHuzasok.Where(h => h.Ev == 2020).ToList();

                long osszNyer = 0;

                foreach (var nap in ev2020)
                {
                    int szorzo = Szorzo(nap, fixTippek);
                    if (szorzo > 0)
                    {
                        int ny = napiAr * szorzo;
                        osszNyer += ny;
                        Console.WriteLine($"{nap.HuzasDatum} - {ny}");
                    }
                }

                long osszKolt = (long)ev2020.Count * napiAr;

                Console.WriteLine($"Összesen {osszKolt} Ft-ot költött Kenóra");
                Console.WriteLine($"Összesen {osszNyer} Ft-ot nyert");
            }


            static int Szorzo(NapiKeno keno, List<int> tippek)
            {
                Dictionary<String, int> nyeroParok = new Dictionary<string, int>(){
            {"10-10",1000000}, {"10-9",8000}, {"10-8",350}, {"10-7",30}, {"10-6",3}, {"10-5",1}, {"10-0",2},
            {"9-9",100000}, {"9-8",1200}, {"9-7",100}, {"9-6",12}, {"9-5",3}, {"9-0",1},
            {"8-8",20000}, {"8-7",350}, {"8-6",25}, {"8-5",5}, {"8-0",1},
            {"7-7",5000}, {"7-6",60}, {"7-5",6}, {"7-4",1}, {"7-0",1},
            {"6-6",500}, {"6-5",20}, {"6-4",3}, {"6-0",1},
            {"5-5",200}, {"5-4",10}, {"5-3",2},
            {"4-4",100}, {"4-3",2},
            {"3-3",15}, {"3-2",1},
            {"2-2",6},
            {"1-1",2}
        };

                int jatekTipus = tippek.Count;
                int talalatokSzama = keno.TalalatSzam(tippek);
                string kulcs = jatekTipus + "-" + talalatokSzama;

                if (nyeroParok.Keys.Contains(kulcs))
                    return nyeroParok[kulcs];
                else
                    return 0;
            }
        }
    }
}
