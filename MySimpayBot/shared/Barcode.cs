using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeLib.BarcodeReader;
using System.Drawing;
namespace Shared.WebService
{
    public class Barcode
    {
        public int minTextLenValid { get; set; } = 3;
        public int maxTextLenValid { get; set; } = 999;

        public string read(string fileName)
        {
            var decode = "";
            try
            {
                var rotaionIndex = 0;
                Bitmap bitMap = (Bitmap)Bitmap.FromFile(fileName);


                int[] barcodeTypes = new[] { BarcodeReader.CODE128, BarcodeReader.CODE39 };//, BarcodeReader.CODE39EX, BarcodeReader.CODABAR, BarcodeReader.DATAMATRIX, BarcodeReader.EAN13, BarcodeReader.EAN8, BarcodeReader.INTERLEAVED25, BarcodeReader.PDF417 };
                do
                {

                    foreach (var barcodeType in barcodeTypes)
                    {
                        decode = DecodeFromBarcode(bitMap, barcodeType).Replace(System.Environment.NewLine, "");
                        if (decode.Length >= minTextLenValid && decode.Length <= maxTextLenValid)
                        {
                            break;
                        }
                        decode = "";
                    }

                    if (decode != "")
                    {
                        break;
                    }
                    rotaionIndex++;
                    if (rotaionIndex > 7)
                    {
                        break;
                    }
                    //bitMap.RotateFlip(RotateFlipType.Rotate90FlipXY);
                    bitMap = RotateImage(bitMap, 45);

                } while (true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, 0);
                decode = "";
            }
            return decode;
        }

        private string DecodeFromBarcode(string fileName, int barcodeType)
        {
            var barcodes = BarcodeReader.read(fileName, barcodeType);
            StringBuilder sb = new StringBuilder();
            sb.Length = 0;
            foreach (string s in barcodes)
            {
                sb.Append(s + Environment.NewLine);
            }
            return sb.ToString();

        }

        private string DecodeFromBarcode(Bitmap imageObject, int barcodeType)
        {
            var ans = "";
            do
            {
                var barcodes = BarcodeReader.read(imageObject, barcodeType);
                if (barcodes == null)
                {
                    break;
                }
                StringBuilder sb = new StringBuilder();
                sb.Length = 0;
                foreach (string s in barcodes)
                {
                    sb.Append(s + Environment.NewLine);
                }
                ans = sb.ToString();

            } while (false);
            return ans;
        }

        private Bitmap RotateImage(Bitmap bmp, float angle)
        {
            Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(bmp.Width / 2, bmp.Height / 2); //set the rotation point as the center into the matrix
                g.RotateTransform(angle); //rotate
                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2); //restore rotation point into the matrix
                g.DrawImage(bmp, new Point(0, 0)); //draw the image on the new bitmap
            }

            return rotatedImage;
        }

    }
}
