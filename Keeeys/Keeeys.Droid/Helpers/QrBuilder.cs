using Android.Graphics;
using ZXing;
using ZXing.Common;

namespace Keeeys.Droid.Helpers
{
    class QrBuilder
    {
        public static Bitmap BuildImage(string data, int width, int height)
        {
            EncodingOptions options = new EncodingOptions
            {
                Height = width,
                Width = height,
                PureBarcode = true,
                Margin = 0
            };
            options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options

            };
            byte[] bitmapData = writer.Write(data);
            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int pos = (width * j + i) * 4;
                    bitmap.SetPixel(i, j, Color.Argb(255, bitmapData[pos + 1], bitmapData[pos + 2], bitmapData[pos + 3]));
                }
            }

            return bitmap;
        }
    }
}