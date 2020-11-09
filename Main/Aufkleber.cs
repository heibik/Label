using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Main
{
    class Aufkleber
    {
        public string ArtBezeichnung{ get; set; }
        public string ModBezeichnung { get; set; }
        //public decimal GroesseUS { get; set; }

          public decimal GroesseD { get; set; }
        public BitmapImage Symbolbild { get; set; }
        public string Geschlecht { get; set; }
        public string Farbe { get; set; }



        public Aufkleber(string artbezeichnung, string modbezeichnung, decimal groesseD, byte[] symbolbild, char geschlecht, string farbe)
        {
            ArtBezeichnung = artbezeichnung;
            ModBezeichnung = modbezeichnung;
            GroesseD = groesseD;
            Symbolbild = Byte2Pic(symbolbild);
            Geschlecht = GeschlechtFull(geschlecht);
            Farbe = farbe;
        }

        public Aufkleber()
        {
        }

    private BitmapImage Byte2Pic(byte[] by)
    {
        if (by == null)
        {
            return null;
        }
        BitmapImage bild = new BitmapImage();
        bild.BeginInit();
        bild.CacheOption = BitmapCacheOption.OnLoad;
        bild.StreamSource = new MemoryStream(by);
        bild.EndInit();


        return bild;
    }

    private string GeschlechtFull(char c)
        {
            string ganz = "";
            switch (c)
            {
                case 'm':
                    ganz = "männlich";
                    break;
                case 'w':
                    ganz = "weiblich";
                    break;
                case 'u':
                    ganz = "unisex";
                    break;
                default:
                    break;
            }

            return ganz;
        }

}
}
