using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace HintaseurantaKarkkainen.Models
{
    class Category
    {
        public string Name { get; set; }
        public string AjaxUrl { get; set; }
        public string AjaxBodyData { get; set; }
    }

    /**
     * HUOM!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
     * Kategoreissa sopivuuksien vuoksi en käyttänyt "ä" ja "ö" kirjaimia. Niitä on korvattu "a":lla ja "o":lla
     */
    class Categories
    {
        private static List<Category> _categories = new List<Category>();
        private const string AjaxBodyLastDataPart = "&minPrice=&maxPrice=&pageSize=1000&beginIndex=0&orderBy=&ajaxRequest=true";

        /**
         * Palauttaa yhden kategorian. Sen paikan kertoo "position"-muuttuja
         */
        public static Category GetCategory(int position)
        {
            if (_categories.Count == 0) PopulateCategories();
            return _categories[position];
        }

        /**
         * Palauttaa kaikki kategoriat jossa esintyy merkkijono. Merkkijono voi erottaa merkillä "|", esim "pyyhkeet|matot".
         * Silloin parsataan kaikki kategoriat jossa esintyy sanat "pyyhkeet" tai "matot."
         * Muita esimerkkejä: "Lemmikkielaimet|Ratsastus", "Valaistus|Kodintekstiili|Keittio"
         */
        public static List<Category> GetCategoriesByString(string str)
        {
            if (_categories.Count == 0) PopulateCategories();

            var categoriesList = new List<Category>();
            string[] splitString = str.Split('|');

            foreach (string splitStringValue in splitString)
            {
                categoriesList.AddRange(_categories.Where(category => category.Name.ToLower().Contains(splitStringValue.ToLower())));
            }

            return categoriesList.Distinct().ToList();
        }

        /**
         * Palauttaa kaikki kategoriat. Kun esim. halutaan parsata koko kauppa.
         */
        public static List<Category> GetAllCategories()
        {
            if (_categories.Count == 0) PopulateCategories();
            return _categories;
        }

        /**
         * Palauttaa "_categories"-listan koko.
         */
        public static int GetCategoriesSize()
        {
            return _categories.Count;
        }

        /**
         * Apufunktio, joka rakentaa AJAX-kutsua varten body-osan
         */
        private static string GetFullAjaxBodyData(Store store, Catalog catalog, int category, string parentCategory)
        {
            return String.Format("storeId={0}&catalogId={1}&langId=-11&searchTerm=&categoryCatalogId=&categoryId={2}&parentCategoryId={3}",
                (int)store, (int)catalog, category, parentCategory) + AjaxBodyLastDataPart;
        }

        /**
         * Kaupan kaikki pääkategoriat valikoimasta
         * Ne ovat vakioita tällä hetkellä. Mutta Kärkkäinen voi muuttaa tilanteen.
         */
        private enum Store
        {
            KotiAndSisustaminen = 10151,
            PihaAndPuutarha = 10151,
            Urheilu = 10151,
            Era = 10151,
            KodintekniikkaAndViihde = 10151,
            Tietotekniikka = 10151,
            PelitAndViihde = 10151,
            Harraste = 10151,
            Lapset = 10151,
            Tyokalut = 10151,
            Rakentaminen = 10151,
            KauneusAndTerveys = 10151
        }

        /**
         * Alikategoriat, jotka ovat pääkategoriassa
         * Ne ovat vakioita tällä hetkellä. Mutta Kärkkäinen voi muuttaa tilanteen.
         */
        private enum Catalog
        {
            // Koti & Sisustaminen
            Valaistus = 10051,
            Kodintekstiili = 10051,
            Sisustustavara = 10051,
            Keittio = 10051,
            Kodinhoito = 10051,
            Toimistotarvike = 10051,

            // Piha & Puutarha
            PihanLeikkivalineet = 10051,
            Puutarha = 10051,
            PuutarhanHoito = 10051,

            // Urheilu
            Kesalajit = 10051,
            KuntourheiluPyoraily = 10051,
            Jaaurheilu = 10051,
            Urheilutekstiili = 10051,
            Hiihtourheilu = 10051,
            Talviurheilu = 10051,
            Sisapelit = 10051,

            // Erä
            Retkeily = 10051,
            AseetAndTarvikkeet = 10051,
            Metsastys = 10051,
            Kalastus = 10051,

            // Kodintekniikka & viihde
            Foto = 10051,
            Autohifi = 10051,
            Kodinkone = 10051,
            Musiikkitarvikkeet = 10051,
            ViihdeElektroniikka = 10051,

            // Tietotekniikka
            Puhelimet = 10051,
            Tietokoneet = 10051,
            Komponentit = 10051,
            Oheislaitteet = 10051,

            // Pelit & Viihde
            Pelit = 10051,
            Pelikonsolit = 10051,
            Kirjat = 10051,
            Elokuvat = 10051,

            // Harraste
            Lemmikkielaimet = 10051,
            Ratsastus = 10051,
            RCHarraste = 10051,
            TaideJaAskartelu = 10051,

            // Lapset
            Lelut = 10051,
            Lastentarvike = 10051,

            // Työkalut
            Kasityokalut = 10051,
            Tyovalineet = 10051,
            TyoasutJaMuut = 10051,
            Autotarvike = 10051,

            // Rakentaminen
            TulisijatKiukaat = 10051,
            Sisustaminen = 10051,
            Sisustustarvike = 10051,
            Sahkotarvike = 10051,

            // Kauneus & terveys
            HenkilokohtainenHygienia = 10051,
            Kosmetiikka = 10051,
            Luontaistuote = 10051
        }

        /**
         * Nämä ovat pääkategoreiden AJAX-kutsua varten urlit
         */
        private static class AjaxUrls
        {
            public const string ValaistusAjaxUrl = "https://www.karkkainen.com/verkkokauppa/valaistus/AjaxFacetedGrid";
            public const string KodintekstiiliAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kodintekstiili/AjaxFacetedGrid";
            public const string SisustustavaraAjaxUrl = "https://www.karkkainen.com/verkkokauppa/sisustustavara/AjaxFacetedGrid";
            public const string KeittioAjaxUrl = "https://www.karkkainen.com/verkkokauppa/keitti%C3%B6/AjaxFacetedGrid";
            public const string KodinhoitoAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kodinhoito/AjaxFacetedGrid";
            public const string ToimistotarvikeAjaxUrl = "https://www.karkkainen.com/verkkokauppa/toimistotarvike/AjaxFacetedGrid";
            public const string PihanleikkivalineetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/pihan-leikkiv%C3%A4lineet-27364001/AjaxFacetedGrid";
            public const string PuutarhaAjaxUrl = "https://www.karkkainen.com/verkkokauppa/puutarha/AjaxFacetedGrid";
            public const string PuutarhanhoitoAjaxUrl = "https://www.karkkainen.com/verkkokauppa/puutarhan-hoito/AjaxFacetedGrid";
            public const string KesalajitAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kes%C3%A4lajit/AjaxFacetedGrid";
            public const string KuntourheiluPyorailyAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kuntourheilu--py%C3%B6r%C3%A4ily/AjaxFacetedGrid";
            public const string JaaurheiluAjaxUrl = "https://www.karkkainen.com/verkkokauppa/j%C3%A4%C3%A4urheilu-27348501/AjaxFacetedGrid";
            public const string UrheilutekstiiliAjaxUrl = "https://www.karkkainen.com/verkkokauppa/urheilutekstiili/AjaxFacetedGrid";
            public const string HiihtourheiluAjaxUrl = "https://www.karkkainen.com/verkkokauppa/hiihtourheilu-27349001/AjaxFacetedGrid";
            public const string TalviurheiluAjaxUrl = "https://www.karkkainen.com/verkkokauppa/talviurheilu/AjaxFacetedGrid";
            public const string SisapelitAjaxUrl = "https://www.karkkainen.com/verkkokauppa/sis%C3%A4pelit/AjaxFacetedGrid";
            public const string RetkeilyAjaxUrl = "https://www.karkkainen.com/verkkokauppa/retkeily/AjaxFacetedGrid";
            public const string AseetAndTarvikkeetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/aseet---tarvikkeet/AjaxFacetedGrid";
            public const string MetsastysAjaxUrl = "https://www.karkkainen.com/verkkokauppa/mets%C3%A4stys/AjaxFacetedGrid";
            public const string KalastusAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kalastus/AjaxFacetedGrid";
            public const string FotoAjaxUrl = "https://www.karkkainen.com/verkkokauppa/foto/AjaxFacetedGrid";
            public const string AutohifiAjaxUrl = "https://www.karkkainen.com/verkkokauppa/autohifi/AjaxFacetedGrid";
            public const string KodinkoneAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kodinkone/AjaxFacetedGrid";
            public const string MusiikkitarvikkeetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/musiikkitarvikkeet/AjaxFacetedGrid";
            public const string ViihdeElektroniikkaAjaxUrl = "https://www.karkkainen.com/verkkokauppa/viihde-elektroniikka/AjaxFacetedGrid";
            public const string PuhelimetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/puhelimet-27321501/AjaxFacetedGrid";
            public const string TietokoneetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/tietokoneet-24198501/AjaxFacetedGrid";
            public const string KomponentitAjaxUrl = "https://www.karkkainen.com/verkkokauppa/komponentit/AjaxFacetedGrid";
            public const string OheislaitteetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/oheislaitteet/AjaxFacetedGrid";
            public const string PelitAjaxUrl = "https://www.karkkainen.com/verkkokauppa/pelit/AjaxFacetedGrid";
            public const string PelikonsolitAjaxUrl = "https://www.karkkainen.com/verkkokauppa/pelikonsolit/AjaxFacetedGrid";
            public const string KirjatAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kirjat/AjaxFacetedGrid";
            public const string ElokuvatAjaxUrl = "https://www.karkkainen.com/verkkokauppa/elokuvat/AjaxFacetedGrid";
            public const string LemmikkielaimetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/lemmikkiel%C3%A4imet/AjaxFacetedGrid";
            public const string RatsastusAjaxUrl = "https://www.karkkainen.com/verkkokauppa/ratsastus/AjaxFacetedGrid";
            public const string RCHarrasteAjaxUrl = "https://www.karkkainen.com/verkkokauppa/rc---harraste/AjaxFacetedGrid";
            public const string TaideJaAskarteluAjaxUrl = "https://www.karkkainen.com/verkkokauppa/rc---harraste/AjaxFacetedGrid";
            public const string LelutAjaxUrl = "https://www.karkkainen.com/verkkokauppa/lelut/AjaxFacetedGrid";
            public const string LastentarvikeAjaxUrl = "https://www.karkkainen.com/verkkokauppa/lastentarvike/AjaxFacetedGrid";
            public const string KasityokalutAjaxUrl = "https://www.karkkainen.com/verkkokauppa/k%C3%A4sity%C3%B6kalut/AjaxFacetedGrid";
            public const string TyovalineetAjaxUrl = "https://www.karkkainen.com/verkkokauppa/ty%C3%B6v%C3%A4lineet/AjaxFacetedGrid";
            public const string TyoasutJaMuutAjaxUrl = "https://www.karkkainen.com/verkkokauppa/ty%C3%B6asut-ja-muut/AjaxFacetedGrid";
            public const string AutotarvikeAjaxUrl = "https://www.karkkainen.com/verkkokauppa/autotarvike/AjaxFacetedGrid";
            public const string TulisijatKiukaatAjaxUrl = "https://www.karkkainen.com/verkkokauppa/tulisijat--kiukaat/AjaxFacetedGrid";
            public const string SisustaminenAjaxUrl = "https://www.karkkainen.com/verkkokauppa/sisustaminen/AjaxFacetedGrid";
            public const string SisustustarvikeAjaxUrl = "https://www.karkkainen.com/verkkokauppa/sisustustarvike/AjaxFacetedGrid";
            public const string SahkotarvikeAjaxUrl = "https://www.karkkainen.com/verkkokauppa/s%C3%A4hk%C3%B6tarvike/AjaxFacetedGrid";
            public const string HenkilokohtainenHygieniaAjaxUrl = "https://www.karkkainen.com/verkkokauppa/henkil%C3%B6kohtainen-hygienia/AjaxFacetedGrid";
            public const string KosmetiikkaAjaxUrl = "https://www.karkkainen.com/verkkokauppa/kosmetiikka/AjaxFacetedGrid";
            public const string LuontaistuoteAjaxUrl = "https://www.karkkainen.com/verkkokauppa/luontaistuote/AjaxFacetedGrid";

        }

        /**
         * Täyttää _categories-listan.
         */
        public static void PopulateCategories()
        {
            long startTime = Libs.GetUnixTimestamp();

            if (_categories == null) throw new Exception("_categories is null. Exiting");
            // VALAISTUS
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Valaistus Ulko- ja pihavalaisimet",
                AjaxUrl = AjaxUrls.ValaistusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Valaistus, 24385001, "10051_24216001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Valaistus Yleisvalaisimet",
                AjaxUrl = AjaxUrls.ValaistusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Valaistus, 24393501, "10051_24216001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Valaistus Lamput",
                AjaxUrl = AjaxUrls.ValaistusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Valaistus, 24411001, "10051_24216001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Valaistus Kattovalaisimet",
                AjaxUrl = AjaxUrls.ValaistusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Valaistus, 24427001, "10051_24216001")
            });
            _categories.Add(new Category()
            {
                Name = " KotiAndSisustaminen Valaistus Sisustusvalaisimet",
                AjaxUrl = AjaxUrls.ValaistusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Valaistus, 24441001, "10051_24216001")
            });
            _categories.Add(new Category()
            {
                Name = " KotiAndSisustaminen Valaistus Valaisintarvikkeet",
                AjaxUrl = AjaxUrls.ValaistusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Valaistus, 24450001, "10051_24216001")
            });

            // KODINTEKSTIILI
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Pyyhkeet",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24246501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Matot",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24256501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Kasityotarvikkeet",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24284001, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Taloustekstiilit",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24333501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Kankaat",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24348501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Sisustustekstiilit",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24370501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Verhot",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24382001, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Verhotarvikkeet",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24399501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Sangyt ja patjat",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24410001, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Tyynyt ja paalliset",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24426501, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Peitteet",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24439001, "10051_24180501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodintekstiili Liinavaatteet",
                AjaxUrl = AjaxUrls.KodintekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodintekstiili, 24447001, "10051_24180501")
            });

            // SISUSTUSTAVARA
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Lahjatuotteet",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24236001, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Servetit",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24273501, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Sauna- ja kylpyhuone",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24334501, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Koriste-esineet",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24339501, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Kayttoesineet",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24359001, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Kynttilatarvikkeet",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24383501, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Kynttilat",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24400001, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Taulut, julisteet",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24462001, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Kehykset",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24469001, "10051_24186501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Sisustustavara Sesonkituotteet",
                AjaxUrl = AjaxUrls.SisustustavaraAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Sisustustavara, 24303001, "10051_24186501")
            });

            // KEITTIÖ
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Astiastot",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24421001, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Aterimet",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24431001, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Juomalasit/lasisto",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24395501, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Veitset",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24329001, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Tarjoiluvalineet",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24379501, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Keittiotyovalineet",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24362501, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Taloustavara",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24228001, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Keitto- ja paistoastiat",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24345001, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Kertakaytto",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24264501, "10051_24191001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Keittio Muut keittiovaolineet",
                AjaxUrl = AjaxUrls.KeittioAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Keittio, 24301001, "10051_24191001")
            });

            // KODINHOITO
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodinhoito Pyykinkuivausvalineet",
                AjaxUrl = AjaxUrls.KodinhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodinhoito, 24220001, "10051_24206501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodinhoito Puhdistus- ja siivouvalineet",
                AjaxUrl = AjaxUrls.KodinhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodinhoito, 24254501, "10051_24206501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodinhoito Yleispesuaineet",
                AjaxUrl = AjaxUrls.KodinhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodinhoito, 24294501, "10051_24206501")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Kodinhoito Tekstiilien pesuaineet",
                AjaxUrl = AjaxUrls.KodinhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Kodinhoito, 24318001, "10051_24206501")
            });

            // TOIMISTOTARVIKE
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Almanakat,kalenterit, kartat",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24233001, "10051_24210001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Paperitarvikkeet",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24259501, "10051_24210001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Kirjetarvikkeet, kortit",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24285001, "10051_24210001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Pakkaus, paketointi",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24335001, "10051_24210001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Kirjoitus-, piirustusvalineet",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24354001, "10051_24210001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Toimistovalineet",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24358501, "10051_24210001")
            });
            _categories.Add(new Category()
            {
                Name = "KotiAndSisustaminen Toimistotarvike Laskimet",
                AjaxUrl = AjaxUrls.ToimistotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KotiAndSisustaminen, Catalog.Toimistotarvike, 24381501, "10051_24210001")
            });

            // Pihan leikkivälineet
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PihanLeikkivalineet Trampoliinit",
                AjaxUrl = AjaxUrls.PihanleikkivalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PihanLeikkivalineet, 25508501, "10051_27364001")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PihanLeikkivalineet Keinut ja leikkikeskukset",
                AjaxUrl = AjaxUrls.PihanleikkivalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PihanLeikkivalineet, 25969001, "10051_27364001")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PihanLeikkivalineet Pomppulinnat",
                AjaxUrl = AjaxUrls.PihanleikkivalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PihanLeikkivalineet, 27364501, "10051_27364001")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PihanLeikkivalineet Uima-altaat",
                AjaxUrl = AjaxUrls.PihanleikkivalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PihanLeikkivalineet, 27367001, "10051_27364001")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PihanLeikkivalineet Polkuautot",
                AjaxUrl = AjaxUrls.PihanleikkivalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PihanLeikkivalineet, 27384501, "10051_27364001")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PihanLeikkivalineet Monkijat",
                AjaxUrl = AjaxUrls.PihanleikkivalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PihanLeikkivalineet, 24231001, "10051_27364001")
            });

            // Puutarha
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha Puutarha Grillit ja tarvikkeet",
                AjaxUrl = AjaxUrls.PuutarhaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.Puutarha, 24348001, "10051_24197501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha Puutarha Puutarhatyokalut",
                AjaxUrl = AjaxUrls.PuutarhaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.Puutarha, 24252001, "10051_24197501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha Puutarha Puutarhan rakennustarvikkeet",
                AjaxUrl = AjaxUrls.PuutarhaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.Puutarha, 24261501, "10051_24197501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha Puutarha Piharakennukset ja kalusteet",
                AjaxUrl = AjaxUrls.PuutarhaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.Puutarha, 24293001, "10051_24197501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha Puutarha Aitatarvikkeet",
                AjaxUrl = AjaxUrls.PuutarhaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.Puutarha, 24324001, "10051_24197501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha Puutarha Puutarharuukut,-patsaat",
                AjaxUrl = AjaxUrls.PuutarhaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.Puutarha, 24369501, "10051_24197501")
            });

            // Puutarhan hoito
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PuutarhanHoito Kasvinsuojeluaineet",
                AjaxUrl = AjaxUrls.PuutarhanhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PuutarhanHoito, 24477001, "10051_24202501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PuutarhanHoito Hyonteisten torjunta-aineet",
                AjaxUrl = AjaxUrls.PuutarhanhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PuutarhanHoito, 24477501, "10051_24202501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PuutarhanHoito Puutarhalannoitteet",
                AjaxUrl = AjaxUrls.PuutarhanhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PuutarhanHoito, 24478001, "10051_24202501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PuutarhanHoito Maanparannus",
                AjaxUrl = AjaxUrls.PuutarhanhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PuutarhanHoito, 24478501, "10051_24202501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PuutarhanHoito Puutarhan tarvikkeet",
                AjaxUrl = AjaxUrls.PuutarhanhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PuutarhanHoito, 24479001, "10051_24202501")
            });
            _categories.Add(new Category()
            {
                Name = "PihaAndPuutarha PuutarhanHoito Siemenet",
                AjaxUrl = AjaxUrls.PuutarhanhoitoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PihaAndPuutarha, Catalog.PuutarhanHoito, 24479501, "10051_24202501")
            });

            // Pesäpallo
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Pesapallo",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24417501, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Frisbeegolf",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24287501, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Rullalautailu, -luistelu",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24263001, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Golf",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24336501, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Katukiekko",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24313501, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Yleisurheilu",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24230501, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Tennis",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24403001, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Jalkapallo",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24390001, "10051_24189001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Kesalajit Vesiurheilu",
                AjaxUrl = AjaxUrls.KesalajitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Kesalajit, 24373501, "10051_24189001")
            });

            // Kuntourheilu, pyöräily
            _categories.Add(new Category()
            {
                Name = "Urheilu KuntourheiluPyoraily Pyoraily",
                AjaxUrl = AjaxUrls.KuntourheiluPyorailyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.KuntourheiluPyoraily, 24238501, "10051_24188501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu KuntourheiluPyoraily Vapaa-aika ja pelit",
                AjaxUrl = AjaxUrls.KuntourheiluPyorailyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.KuntourheiluPyoraily, 24274501, "10051_24188501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu KuntourheiluPyoraily Kuntourheilu",
                AjaxUrl = AjaxUrls.KuntourheiluPyorailyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.KuntourheiluPyoraily, 24305001, "10051_24188501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu KuntourheiluPyoraily Kengat",
                AjaxUrl = AjaxUrls.KuntourheiluPyorailyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.KuntourheiluPyoraily, 24327501, "10051_24188501")
            });

            // Jääurheilu
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Mailat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25719501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Kiekkoluistimet",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 26086001, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Kaunoluistimet",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 26125001, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Tarvikkeet",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25268501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Polvisuojat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25445501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Muut jaaurheluvaline",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25596501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Maalivahdinsuojat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25820001, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Kyynarsuojat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25904501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Kyparat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25974501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Kiekot",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 26037501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Housut",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 26157001, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Hartiasuojat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 26183001, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Hanskat",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 26204501, "10051_27348501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Jaaurheilu Teipit",
                AjaxUrl = AjaxUrls.JaaurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Jaaurheilu, 25059001, "10051_27348501")
            });

            // Urheilutekstiili
            _categories.Add(new Category()
            {
                Name = "Urheilu Urheilutekstiili Lapset & nuoret",
                AjaxUrl = AjaxUrls.UrheilutekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Urheilutekstiili, 24222501, "10051_24177501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Urheilutekstiili Unisex",
                AjaxUrl = AjaxUrls.UrheilutekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Urheilutekstiili, 24257501, "10051_24177501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Urheilutekstiili Miehet",
                AjaxUrl = AjaxUrls.UrheilutekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Urheilutekstiili, 24302001, "10051_24177501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Urheilutekstiili Naiset",
                AjaxUrl = AjaxUrls.UrheilutekstiiliAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Urheilutekstiili, 24331501, "10051_24177501")
            });

            // Hiihtourheilu
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Voitelu",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 24804501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Voiteet",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25034501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Sukset",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25585501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Siteet",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25712001, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Hiihtokengat",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 26123001, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Sauvat",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25814001, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Suksisarjat",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25433001, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Tarvikkeet",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25248501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Rullasuksivalineet",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25898501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Hionta",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 25969501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Hiihtotekstiilit",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 26033501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Hiihtolasit",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 26082501, "10051_27349001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Hiihtourheilu Muut hiihtourheiluva",
                AjaxUrl = AjaxUrls.HiihtourheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Hiihtourheilu, 24564001, "10051_27349001")
            });

            // Talviurheilu
            _categories.Add(new Category()
            {
                Name = "Urheilu Talviurheilu Lautailu",
                AjaxUrl = AjaxUrls.TalviurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Talviurheilu, 24269001, "10051_24182501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Talviurheilu Laskettelu",
                AjaxUrl = AjaxUrls.TalviurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Talviurheilu, 24298501, "10051_24182501")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Talviurheilu Lasketteluvarusteet",
                AjaxUrl = AjaxUrls.TalviurheiluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Talviurheilu, 24248501, "10051_24182501")
            });

            // Sisäpelit
            _categories.Add(new Category()
            {
                Name = "Urheilu Sisapelit Squash",
                AjaxUrl = AjaxUrls.SisapelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Sisapelit, 24280001, "10051_24184001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Sisapelit Lentopallo",
                AjaxUrl = AjaxUrls.SisapelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Sisapelit, 24290001, "10051_24184001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Sisapelit Poytatennis",
                AjaxUrl = AjaxUrls.SisapelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Sisapelit, 24312501, "10051_24184001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Sisapelit Sulkapallo",
                AjaxUrl = AjaxUrls.SisapelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Sisapelit, 24347001, "10051_24184001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Sisapelit Koripallo",
                AjaxUrl = AjaxUrls.SisapelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Sisapelit, 24363001, "10051_24184001")
            });
            _categories.Add(new Category()
            {
                Name = "Urheilu Sisapelit Salibandy",
                AjaxUrl = AjaxUrls.SisapelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Urheilu, Catalog.Sisapelit, 24380501, "10051_24184001")
            });

            // Retkeily
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Valaisimet",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24433001, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Lumikengat, erasukset, vaellussauvat",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24343501, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Teltat, makuupussit",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24451001, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Retkeilyasusteet",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24421501, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Retkivarusteet",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24448001, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Vaelluskengat",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24407001, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Reput, rinkat",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24431501, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Retkiruoat",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24386001, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Suunnistusvalineet",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24442001, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Kumiveneet, kanootit",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24321501, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Kellunta-asusteet",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24307501, "10051_24199501")
            });
            _categories.Add(new Category()
            {
                Name = "Era Retkeily Muut retkeilytarvikkeet",
                AjaxUrl = AjaxUrls.RetkeilyAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Retkeily, 24466001, "10051_24199501")
            });

            //
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Ilma-aseet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 27347001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Airsoft",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 27347501, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Jousiammunta",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24265001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Lupavapaat aseet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24286001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Aseet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24434501, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Aseenosat, -tarvikkeet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24406001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Patruunat",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24423001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Tahtaimet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24367501, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Asesailytys",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24389001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Latausvalineet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24422001, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Ampumaratavalineet",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24346501, "10051_24207001")
            });
            _categories.Add(new Category()
            {
                Name = "Era AseetAndTarvikkeet Asekirjat, videot",
                AjaxUrl = AjaxUrls.AseetAndTarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.AseetAndTarvikkeet, 24317001, "10051_24207001")
            });

            // Metsästys
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Metsastyskirjat ja -videot",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24232001, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Ansaraudat, pyydykset",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24257001, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Naamioverkot, valineet",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24288501, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Riistankasittely",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24691001, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Houkuttimet",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24332001, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Tutkat, puhelimet, yms",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24356001, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Jalkineet",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24368501, "10051_24209001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Metsastys Metsastysasusteet",
                AjaxUrl = AjaxUrls.MetsastysAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Metsastys, 24389501, "10051_24209001")
            });

            // Kalastus
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Kalastusasusteet",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24304001, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Pyydyskalastus",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24334001, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Pilkki-, onkikalastus",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24339001, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Kahluuvarusteet, kelluntarenkaat",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24360001, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Perhokalastus",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24378001, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Kaikuluotaimet",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24398501, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Pakit, rasiat, laukut",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24414001, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Viehekalastus",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24427501, "10051_24206001")
            });
            _categories.Add(new Category()
            {
                Name = "Era Kalastus Muut kalastustarvikkeet",
                AjaxUrl = AjaxUrls.KalastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Era, Catalog.Kalastus, 24462501, "10051_24206001")
            });

            // Foto
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Foto Kamerat",
                AjaxUrl = AjaxUrls.FotoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Foto, 24235001, "10051_24194001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Foto Kameratarvikkeet",
                AjaxUrl = AjaxUrls.FotoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Foto, 24303501, "10051_24194001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Foto Kiikarit ja kaukoputket",
                AjaxUrl = AjaxUrls.FotoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Foto, 24364501, "10051_24194001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Foto Muistikortit",
                AjaxUrl = AjaxUrls.FotoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Foto, 24461501, "10051_24194001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Foto Albumit",
                AjaxUrl = AjaxUrls.FotoAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Foto, 24468501, "10051_24194001")
            });

            // Autohifi
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Autohifi Kaapelit ja tarvikkeet",
                AjaxUrl = AjaxUrls.AutohifiAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Autohifi, 24234501, "10051_24196001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Autohifi Kaiuttimet",
                AjaxUrl = AjaxUrls.AutohifiAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Autohifi, 24266501, "10051_24196001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Autohifi Paatelaitteet",
                AjaxUrl = AjaxUrls.AutohifiAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Autohifi, 24292001, "10051_24196001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Autohifi Rakennussarjat",
                AjaxUrl = AjaxUrls.AutohifiAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Autohifi, 24315001, "10051_24196001")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Autohifi Vahvistimet",
                AjaxUrl = AjaxUrls.AutohifiAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Autohifi, 24374501, "10051_24196001")
            });

            // Kodinkone
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Keittion pienkoneet",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24459501, "10051_24201501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Kodinhoito",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24467501, "10051_24201501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Kasivalaisimet, paristot",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24470001, "10051_24201501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Pesu- ja kuivauskoneet",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24472501, "10051_24201501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Terveyden- ja kauneudenhoito",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24474501, "10051_24201501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Kylmalaitteet",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24475001, "10051_24201501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Kodinkone Uunit, liedet",
                AjaxUrl = AjaxUrls.KodinkoneAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Kodinkone, 24476501, "10051_24201501")
            });

            // Musiikkitarvikkeet
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Musiikkitarvikkeet Kaapelit ja adapterit",
                AjaxUrl = AjaxUrls.MusiikkitarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Musiikkitarvikkeet, 24319501, "10051_24205501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Musiikkitarvikkeet Kuulokkeet",
                AjaxUrl = AjaxUrls.MusiikkitarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Musiikkitarvikkeet, 24352501, "10051_24205501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Musiikkitarvikkeet Mikrofonit",
                AjaxUrl = AjaxUrls.MusiikkitarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Musiikkitarvikkeet, 24373001, "10051_24205501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde Musiikkitarvikkeet PA -laitteet",
                AjaxUrl = AjaxUrls.MusiikkitarvikkeetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.Musiikkitarvikkeet, 24388001, "10051_24205501")
            });

            // ViihdeElektroniikka
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka AV-laitteet",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24233501, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Digiboksit",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24265501, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Kaiuttimet",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24289501, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Varashalyttimet",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24355001, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Soittimet",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24398001, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Stereot ja radiot",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24416501, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Televisiot, projektorit",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24428501, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka Telineet, kotelot",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24437001, "10051_24209501")
            });
            _categories.Add(new Category()
            {
                Name = "KodintekniikkaAndViihde ViihdeElektroniikka A/V-kaapelit, tarvikkeet",
                AjaxUrl = AjaxUrls.ViihdeElektroniikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KodintekniikkaAndViihde, Catalog.ViihdeElektroniikka, 24465501, "10051_24209501")
            });

            // Puhelimet
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Puhelimet",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 25757501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Alykellot",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 27368501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Suojakalvot",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 26209501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Suojakuoret",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 26000501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Laturit",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 25642501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Telineet",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 25931001, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Kotelot",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 25498501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Handsfree",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 24908001, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Kaapelit, adapte",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 25131501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Autonavigointi",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 26099001, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Autonavig. tarvikkeet",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 26189001, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Muut tarvikkeet",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 26056501, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Akut",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 24671001, "10051_27321501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Puhelimet Ohjelmat",
                AjaxUrl = AjaxUrls.PuhelimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Puhelimet, 26135001, "10051_27321501")
            });

            // Tietokoneet
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Tietokoneet Kannettavat",
                AjaxUrl = AjaxUrls.TietokoneetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Tietokoneet, 24219501, "10051_24198501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Tietokoneet Poytakoneet",
                AjaxUrl = AjaxUrls.TietokoneetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Tietokoneet, 24285501, "10051_24198501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Tietokoneet Tablet tarvikkeet",
                AjaxUrl = AjaxUrls.TietokoneetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Tietokoneet, 24377501, "10051_24198501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Tietokoneet Tablet tietokoneet",
                AjaxUrl = AjaxUrls.TietokoneetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Tietokoneet, 24396001, "10051_24198501")
            });

            // Komponentit
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Komponentit Asemat",
                AjaxUrl = AjaxUrls.KomponentitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Komponentit, 24343001, "10051_24200501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Komponentit Kiintolevyt",
                AjaxUrl = AjaxUrls.KomponentitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Komponentit, 24377001, "10051_24200501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Komponentit Naytonohjaimet",
                AjaxUrl = AjaxUrls.KomponentitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Komponentit, 24408501, "10051_24200501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Komponentit Ohjainkortit",
                AjaxUrl = AjaxUrls.KomponentitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Komponentit, 24419001, "10051_24200501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Komponentit Aanikortit",
                AjaxUrl = AjaxUrls.KomponentitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Komponentit, 24426001, "10051_24200501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Komponentit Tv-, videoeditointikortit",
                AjaxUrl = AjaxUrls.KomponentitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Komponentit, 24435001, "10051_24200501")
            });

            // Oheislaitteet
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Tulostimet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24221501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Tulostinvarit",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 27329501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Nappaimistot ja hiiret",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24337501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Web -kamerat",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24379001, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Kaiuttimet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24397501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Headsetit ja mikrofonit",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24418501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Virransyotto",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24436501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Pc-peliohjaimet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24444001, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Muistikortinlukijat",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24448501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet USB muistit",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 25799501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Naytot",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24451501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Verkkotuotteet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24453001, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Atk-tarvikkeet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24454001, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Ohjelmistot",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24455001, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet PC-kaapelit, tarvikkeet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tietotekniikka, Catalog.Oheislaitteet, 24466501, "10051_24203501")
            });
            _categories.Add(new Category()
            {
                Name = "Tietotekniikka Oheislaitteet Tallenteet",
                AjaxUrl = AjaxUrls.OheislaitteetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Oheislaitteet, 24469501, "10051_24203501")
            });

            // Pelit
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit PC",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24247501, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit Gameboy",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24298001, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit Nintendo DS",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24347501, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit Nintendo Wii",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24357001, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit PS Vita",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24380001, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit PlayStation 3",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24415501, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit PlayStation Portable",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24429001, "10051_24195001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelit Xbox 360",
                AjaxUrl = AjaxUrls.PelitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelit, 24445501, "10051_24195001")
            });

            // Pelikonsolit
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit Gameboy Advance / SP",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24255501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit Nintendo DS",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24318501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit Nintendo Wii",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24345501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit PS Vita",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24366501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit PlayStation 4",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24388501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit PlayStation 3",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24404501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit PlayStation Portable",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24419501, "10051_24200001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Pelikonsolit Xbox 360",
                AjaxUrl = AjaxUrls.PelikonsolitAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Pelikonsolit, 24438501, "10051_24200001")
            });

            // Kirjat
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Kirjat Yleinen kirjallisuus",
                AjaxUrl = AjaxUrls.KirjatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Kirjat, 24252501, "10051_24203001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Kirjat Oppikirjat",
                AjaxUrl = AjaxUrls.KirjatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Kirjat, 24270501, "10051_24203001")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Kirjat Askartelukirjat",
                AjaxUrl = AjaxUrls.KirjatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Kirjat, 24299001, "10051_24203001")
            });

            // Elokuvat
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Elokuvat DVD",
                AjaxUrl = AjaxUrls.ElokuvatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Elokuvat, 24224001, "10051_24204501")
            });
            _categories.Add(new Category()
            {
                Name = "PelitAndViihde Elokuvat Blu-ray",
                AjaxUrl = AjaxUrls.ElokuvatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.PelitAndViihde, Catalog.Elokuvat, 24262501, "10051_24204501")
            });

            // Lemmikkielaimet
            _categories.Add(new Category()
            {
                Name = "Harraste Lemmikkielaimet Koriat",
                AjaxUrl = AjaxUrls.LemmikkielaimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Lemmikkielaimet, 24231501, "10051_24173501")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste Lemmikkielaimet Kissat",
                AjaxUrl = AjaxUrls.LemmikkielaimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Lemmikkielaimet, 24264001, "10051_24173501")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste Lemmikkielaimet Jyrsijat",
                AjaxUrl = AjaxUrls.LemmikkielaimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Lemmikkielaimet, 24284501, "10051_24173501")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste Lemmikkielaimet Kanit",
                AjaxUrl = AjaxUrls.LemmikkielaimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Lemmikkielaimet, 24311001, "10051_24173501")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste Lemmikkielaimet Linnut",
                AjaxUrl = AjaxUrls.LemmikkielaimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Lemmikkielaimet, 24336001, "10051_24173501")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste Lemmikkielaimet Kalat",
                AjaxUrl = AjaxUrls.LemmikkielaimetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Lemmikkielaimet, 24384001, "10051_24173501")
            });

            // Ratsastus
            _categories.Add(new Category()
            {
                Name = "Harraste Ratsastus Tarvikkeet",
                AjaxUrl = AjaxUrls.RatsastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Ratsastus, 24238001, "10051_24179501")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste Ratsastus Pukeutuminen",
                AjaxUrl = AjaxUrls.RatsastusAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.Ratsastus, 24259001, "10051_24179501")
            });

            // RCHarraste
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Pienoismallit",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24249501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Lennokit",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24277501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Rc-autot",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24302501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Lennokkitarvikkeet",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24330501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Polttomoottorit, tarvikkeet",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24349001, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Sahkomoottorit, tarvikkeet",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24364001, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Radiolaitteet",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24382501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Akut",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24397001, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Rakennusmateriaali",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24409501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Veneet",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24432501, "10051_24197001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste RCHarraste Helikopterit",
                AjaxUrl = AjaxUrls.RCHarrasteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.RCHarraste, 24436001, "10051_24197001")
            });

            // TaideJaAskartelu
            _categories.Add(new Category()
            {
                Name = "Harraste TaideJaAskartelu Varit",
                AjaxUrl = AjaxUrls.TaideJaAskarteluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.TaideJaAskartelu, 24237501, "10051_24202001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste TaideJaAskartelu Paperit, kankaat",
                AjaxUrl = AjaxUrls.TaideJaAskarteluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.TaideJaAskartelu, 24275501, "10051_24202001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste TaideJaAskartelu Tarvikkeet",
                AjaxUrl = AjaxUrls.TaideJaAskarteluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.TaideJaAskartelu, 24305501, "10051_24202001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste TaideJaAskartelu Maalaus, piirtovalineet",
                AjaxUrl = AjaxUrls.TaideJaAskarteluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.TaideJaAskartelu, 24335501, "10051_24202001")
            });
            _categories.Add(new Category()
            {
                Name = "Harraste TaideJaAskartelu Askartelu",
                AjaxUrl = AjaxUrls.TaideJaAskarteluAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Harraste, Catalog.TaideJaAskartelu, 24460501, "10051_24202001")
            });

            // Lelut
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Pelit",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24243501, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Poikien lelut",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24260501, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Tyttojen lelut",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24283501, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Vauvalelut",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24323001, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Rakennussarjat, Legot",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24342501, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Ulkolelut",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24371501, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Elainlelut",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24386501, "10051_24205001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lelut Muut lelut",
                AjaxUrl = AjaxUrls.LelutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lelut, 24409001, "10051_24205001")
            });

            // Lastentarvike
            _categories.Add(new Category()
            {
                Name = "Lapset Lastentarvike Hoitotarvikkeet",
                AjaxUrl = AjaxUrls.LastentarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lastentarvike, 24244001, "10051_24211001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lastentarvike Kalusteet",
                AjaxUrl = AjaxUrls.LastentarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lastentarvike, 24279501, "10051_24211001")
            });
            _categories.Add(new Category()
            {
                Name = "Lapset Lastentarvike Vaunut",
                AjaxUrl = AjaxUrls.LastentarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Lapset, Catalog.Lastentarvike, 24309501, "10051_24211001")
            });

            // Käsityokalut
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Tera-, taontatyokalut",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24230001, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Lapiot, talikot, yms",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24280501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Hionta",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24286501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Mittaustyokalut",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24311501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Vaantimet, avaimet",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24344501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Sinkilapistoolit, sinkilat",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24361501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Puristimet, ulosvetimet",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24387501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Pihdit, leikkurit",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24401501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Maalaustarvikkeet",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24420001, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Laatoitus-, muurarintyokalut",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24430501, "10051_24194501")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Kasityokalut Muut kasityokalut",
                AjaxUrl = AjaxUrls.KasityokalutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Kasityokalut, 24435501, "10051_24194501")
            });

            // Työvälineet
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Porakoneet",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24480001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Aggregaatit",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24459001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Halkomakoneet",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24467001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Lammittimet",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24300501, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Paineilmakoneet",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24471001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Pesurit, pumput",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24471501, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Imurit",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24472001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Muut sahkotyokalut",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24473501, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Lumilingot",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 25219001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Hitsaus",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24476001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Hiomakoneet",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24480501, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Sahat",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24481001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Jyrsimet",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24481501, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Metsurin tyokalut",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24482001, "10051_24199001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Tyovalineet Hoylat",
                AjaxUrl = AjaxUrls.TyovalineetAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Tyovalineet, 24482501, "10051_24199001")
            });

            // Työasut ja muut
            _categories.Add(new Category()
            {
                Name = "Tyokalut TyoasutJaMuut Tyokalujen sailytys",
                AjaxUrl = AjaxUrls.TyoasutJaMuutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.TyoasutJaMuut, 24362001, "10051_24204001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut TyoasutJaMuut Tyoasut, suojaimet",
                AjaxUrl = AjaxUrls.TyoasutJaMuutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.TyoasutJaMuut, 24378501, "10051_24204001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut TyoasutJaMuut Maatalous",
                AjaxUrl = AjaxUrls.TyoasutJaMuutAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.TyoasutJaMuut, 24394001, "10051_24204001")
            });

            // Autotarvike
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Autokemikaalit",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24243001, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Auton sisustus",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24269501, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Autotarvike",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24295501, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Peravaunutarvikkeet",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24320001, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Renkaat ja vanteet",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24342001, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Sahkojarjestelmatarvikkeet",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24356501, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Voiteluaineet",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24392001, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Suodattimet",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24417001, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Caravan",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24423501, "10051_24193001")
            });
            _categories.Add(new Category()
            {
                Name = "Tyokalut Autotarvike Venetarvikkeet",
                AjaxUrl = AjaxUrls.AutotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Tyokalut, Catalog.Autotarvike, 24433501, "10051_24193001")
            });

            // Tulisijat, kiukaat
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Arinat",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24241501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Kaminat",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24272001, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Liesitasot",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24329501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Noki-, tulipesanluuk",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24350001, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Savupellit",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24387001, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Takkaluukut",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24402501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Takkatarvikkeet",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24416001, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Uunineduspellit",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24429501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Uuninluukut",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24438001, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Puukiukaat",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24444501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Sahkokiukaat",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24449501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat Kiuastarvikkeet",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24452501, "10051_24210501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen TulisijatKiukaat LVI",
                AjaxUrl = AjaxUrls.TulisijatKiukaatAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.TulisijatKiukaat, 24464501, "10051_24210501")
            });

            // Sisustaminen
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Kiinnitystarvikkeet",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24224501, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Kaakelit",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24266001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Tapetit",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24296001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Maalit",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24327001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Lattiapinnoitteet",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24351001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Listat",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24371001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Liimat, teipit",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24390501, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Laastit, vesieristeet",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24407501, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Rakentaminen",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24412001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Ovet",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24428001, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Maalaustarvikkeet",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24437501, "10051_24212501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustaminen Laatoitus-,muurarintyokalut",
                AjaxUrl = AjaxUrls.SisustaminenAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustaminen, 24443001, "10051_24212501")
            });

            // Sisustustarvike
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustustarvike Kodintarvike",
                AjaxUrl = AjaxUrls.SisustustarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustustarvike, 24223501, "10051_24213501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustustarvike Helat ja pienrauta",
                AjaxUrl = AjaxUrls.SisustustarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustustarvike, 24258001, "10051_24213501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustustarvike Kylpyhuonetarvike",
                AjaxUrl = AjaxUrls.SisustustarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustustarvike, 24306501, "10051_24213501")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sisustustarvike Kodinturvalaitteet",
                AjaxUrl = AjaxUrls.SisustustarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sisustustarvike, 24366001, "10051_24213501")
            });

            // Sähkötarvike
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Kaapelit",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24239501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Sahkonjakelu",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24263501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Kytkimet",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24294001, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Pistorasiat",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24324501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Asennustarvikkeet",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24338501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Pistotulpat, adapterit",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24456501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Halytys ja valvontalaitteet",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24457501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Sahkotuotanto",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24458001, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Lammittimet, ilmastointi",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24458501, "10051_24215001")
            });
            _categories.Add(new Category()
            {
                Name = "Rakentaminen Sahkotarvike Jatkojohdot",
                AjaxUrl = AjaxUrls.SahkotarvikeAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.Rakentaminen, Catalog.Sahkotarvike, 24461001, "10051_24215001")
            });

            // Henkilökohtainen hygienia
            _categories.Add(new Category()
            {
                Name = "Henkilokohtainen hygienia Suunhoito",
                AjaxUrl = AjaxUrls.HenkilokohtainenHygieniaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.HenkilokohtainenHygienia, 24217001, "10051_24173001")
            });
            _categories.Add(new Category()
            {
                Name = "Henkilokohtainen hygienia Deodorantit",
                AjaxUrl = AjaxUrls.HenkilokohtainenHygieniaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.HenkilokohtainenHygienia, 24279001, "10051_24173001")
            });
            _categories.Add(new Category()
            {
                Name = "Henkilokohtainen hygienia Saippuat",
                AjaxUrl = AjaxUrls.HenkilokohtainenHygieniaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.HenkilokohtainenHygienia, 24310001, "10051_24173001")
            });
            _categories.Add(new Category()
            {
                Name = "Henkilokohtainen hygienia Hiustenhoitotuotteet",
                AjaxUrl = AjaxUrls.HenkilokohtainenHygieniaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.HenkilokohtainenHygienia, 24328501, "10051_24173001")
            });
            _categories.Add(new Category()
            {
                Name = "Henkilokohtainen hygienia Muut hygieniatuotteet",
                AjaxUrl = AjaxUrls.HenkilokohtainenHygieniaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.HenkilokohtainenHygienia, 24341501, "10051_24173001")
            });

            // Kosmetiikka
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Tuoksut",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24236501, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Ihovoiteet, oljyt, balsamit",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24282501, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Silmakosmetiikka",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24301501, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Meikkaus",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24314501, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Kynsienhoito",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24337001, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Jalkahoitovalmisteet",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24365001, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Parranhoitovalineet",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24383001, "10051_24181501")
            });
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Kosmetiikka Tarvikkeet",
                AjaxUrl = AjaxUrls.KosmetiikkaAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Kosmetiikka, 24403501, "10051_24181501")
            });

            // Luontaistuote
            _categories.Add(new Category()
            {
                Name = "KauneusAndTerveys Luontaistuote",
                AjaxUrl = AjaxUrls.LuontaistuoteAjaxUrl,
                AjaxBodyData = GetFullAjaxBodyData(Store.KauneusAndTerveys, Catalog.Luontaistuote, 24232501, "10051_24187001")
            });

            long endTime = Libs.GetUnixTimestamp();
            Console.WriteLine("Populating categories took: {0} ms", endTime - startTime);

        }

    }
}
