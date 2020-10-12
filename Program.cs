using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Nopat
{
    class Laskuri
    {

        public class Stats
        {
            private decimal voittoPros;
            private decimal expKuolleetHyo;
            private decimal expKuolleetPuo;

            private Taistelut taistelut;

            private int[] alkuasetelma;
            
            public Stats (params int[] nums)
            {
                this.alkuasetelma = nums;
            }

            internal void Laske()
            {
                int toistot         = alkuasetelma[0];
                int joukotHyokkays  = alkuasetelma[1];
                int joukotPuolustus = alkuasetelma[2];
                int modHyokkays     = alkuasetelma[3];
                int modPuolustus    = alkuasetelma[4];

                Taistelut taistelut = new Taistelut();
                for (int i = 0; i < toistot; i++)
                {
                    taistelut.Lisaa(joukotHyokkays, joukotPuolustus, modHyokkays, modPuolustus); 
                }

                this.voittoPros = taistelut.tulos();
                this.expKuolleetHyo = taistelut.kuolleetHyokkaajat();
                this.expKuolleetPuo = taistelut.kuolleetPuolustajat();

                this.taistelut = taistelut;

            }

            public void Tulosta()
            {
                Console.WriteLine("Simuloitu " + alkuasetelma[0] + " taistelua arvoilla");
                Console.WriteLine(alkuasetelma[1] + " hyökkääjää");
                Console.WriteLine(alkuasetelma[2] + " puolustajaa");
                Console.WriteLine("Hyökkäävällä armeijalla heittoihin +" + alkuasetelma[3]);
                Console.WriteLine("Puolustavalla armeijalla heittoihin +" + alkuasetelma[4]);
                Console.WriteLine();
                Console.WriteLine("Tulokset:");
                Console.WriteLine("Hyökkääjän mahdollisuus voittaa on " + voittoPros + "%");
                Console.WriteLine("Hyökkääjä voi olettaa tappavan " + expKuolleetPuo + " puolustajaa");
                Console.WriteLine("Hyökkääjä voi olettaa että " + expKuolleetHyo + " hyökkääjää kuolee");

                //Console.WriteLine(taistelut.voitot);
                //Console.WriteLine(taistelut.haviot);
                //Console.WriteLine(taistelut.kuolleetHyo);
                //Console.WriteLine(taistelut.kuolleetPuo);

            }
        }

        public class Taistelut
        {
            public int voitot;
            public int haviot;
            public int kuolleetHyo;
            public int kuolleetPuo;

            public Taistelut() { }

            public void Lisaa(int joukotHyokkays, int joukotPuolustus, int modHyokkays, int modPuolustus)
            {

                var d6 = new Random();
                int hyokkaajat = joukotHyokkays;
                int puolustajat = joukotPuolustus;

                while (hyokkaajat > 0 && puolustajat > 0)
                {

                    int[] nopatHyo;
                    int[] nopatPuo;
                    if (hyokkaajat > 2) { nopatHyo = new int[3]; }
                    else { nopatHyo = new int[hyokkaajat]; }
                    if (puolustajat > 1) { nopatPuo = new int[2]; }
                    else { nopatPuo = new int [puolustajat]; }

                    for ( int i = 0; i < nopatHyo.Length; i++)
                    {
                        nopatHyo[i] = d6.Next(1, 7);
                    }

                    for ( int i = 0; i < nopatPuo.Length; i++)
                    {
                        nopatPuo[i] = d6.Next(1, 7);
                    }

                    Array.Sort(nopatHyo);
                    Array.Reverse(nopatHyo);
                    Array.Sort(nopatPuo);
                    Array.Reverse(nopatPuo);
                    
                    for ( int i = 0; i < Math.Min(nopatHyo.Length, nopatPuo.Length); i++)
                    {
                        if (puolustajat < 1 || hyokkaajat < 1) { break; }

                        if (nopatPuo[i] + modPuolustus > 6) { nopatPuo[i] = 6; }
                        else if (nopatPuo[i] + modPuolustus < 1) { nopatPuo[i] = 1; }
                        else { nopatPuo[i] = nopatPuo[i] + modPuolustus; }

                        if (nopatHyo[i] + modHyokkays > 6) { nopatHyo[i] = 6; }
                        else if (nopatHyo[i] + modHyokkays < 1) { nopatHyo[i] = 1; }
                        else { nopatHyo[i] = nopatHyo[i] + modHyokkays; }

                        //Console.WriteLine(nopatPuo[i] + " " + nopatHyo[i]);

                        if (nopatPuo[i] >= nopatHyo[i]) 
                        { 
                            hyokkaajat--;
                            this.kuolleetHyo++;

                            //if( hyokkaajat == 0) { Console.WriteLine("puolustusvoitto"); }

                        }
                        else
                        {
                            puolustajat--;
                            this.kuolleetPuo++;

                            //if (puolustajat == 0) { Console.WriteLine("hyökkäysvoitto"); }
                        }

                    }
                }

                if (hyokkaajat > puolustajat) { this.voitot++; }
                else { this.haviot++; }
            }

            internal decimal tulos()
            {
                return (decimal)((decimal) voitot / (decimal)(voitot + haviot) * (decimal) 100);
            }

            internal decimal kuolleetHyokkaajat()
            {
                return (decimal)((decimal)kuolleetHyo / (decimal)(voitot + haviot) * (decimal)1);
            }

            internal decimal kuolleetPuolustajat()
            {
                return (decimal)((decimal)kuolleetPuo / (decimal)(voitot + haviot) * (decimal)1);
            }

        }

        //Laskee mahdollisuuksia voittaa taistelu Riskissä
        static void Main(string[] args)
        {
            try
            {
                int t1 = Int32.Parse(args[0]);
                int t2 = Int32.Parse(args[1]);
                int t3 = Int32.Parse(args[2]);
                int t4 = Int32.Parse(args[3]);
                int t5 = Int32.Parse(args[4]);

            }
            catch
            {
                Console.WriteLine("Huonot Parametrit");
                return;
            }

            int toistot = Int32.Parse(args[0]);
            int joukotHyokkays = Int32.Parse(args[1]);
            int joukotPuolustus = Int32.Parse(args[2]);
            int modHyokkays = Int32.Parse(args[3]);
            int modPuolustus = Int32.Parse(args[4]);

            Stats stats = new Stats(toistot, joukotHyokkays, joukotPuolustus, modHyokkays, modPuolustus);
            stats.Laske();
            stats.Tulosta();
        }
    }
}
