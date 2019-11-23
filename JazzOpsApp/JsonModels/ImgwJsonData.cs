using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JazzOpsApp.JsonModels
{
    public class ImgwJsonData
    {        
            public int id_stacji { get; set; }
            public string stacja { get; set; }
            public DateTime data_pomiaru { get; set; }
            public int godzina_pomiaru { get; set; }
            public double temperatura { get; set; }
            public double predkosc_wiatru { get; set; }
            public int kierunek_wiatru { get; set; }
            public double wilgotnosc_wzgledna { get; set; }
            public double suma_opadu { get; set; }
            public double cisnienie { get; set; }
    }
}
