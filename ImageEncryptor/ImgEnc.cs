using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ImageEncryptor
{
    public static class ImgEnc
    {
        const int ImageChannels = 4;
        static byte[] startSequenceArr = new byte[] { 227, 57, 109, 141, 220, 123, 31, 99, 42, 74, 99, 12, 108, 199, 212, 240 };

        public static int MaxBitContent(string imgPath)
        {
            Bitmap img = new Bitmap(imgPath);
            return MaxBitContent(img);
        }

        public static int MaxBitContent(Bitmap img)
        {
            return MaxBitContent(img.Size);
        }

        public static int MaxBitContent(Size size)
        {
            //Image width multiplied by image height multiplied by 3 (RGB channels)
            return size.Width * size.Height * ImageChannels;
        }

        public static int TotalBits(string[] encFiles)
        {
            List<BitArray> fileData = new List<BitArray>();
            foreach (string path in encFiles)
            {
                BitArray fileArray = GetFileBits(path);
                fileData.Add(fileArray);
            }
            return TotalBits(fileData);
        }

        public static int TotalBits(List<BitArray> files)
        {
            int totalSize = 128;//The first 128 bits will contain a sequence that confirms the encryption
            totalSize += 32;//32 bits will contain the amount of files
            foreach (var file in files)
            {
                totalSize += 24;//24 bits will keep the file extention
                totalSize += 32;//32 bits will keep the file size
                totalSize += file.Length;//The amount of bits required by this file
            }
            return totalSize;
        }

        public static List<DecryptedFile> Decrypt(string filePath)
        {
            Bitmap img = new Bitmap(filePath);
            return Decrypt(img);
        }

        public static List<DecryptedFile> Decrypt(Bitmap img)
        {
            Color[,] pixels = GetImagePixels(img);
            if (IsEncryptedImage(img, ref pixels))
            {
                List<DecryptedFile> files = new List<DecryptedFile>();
                int fileLength = BitConverter.ToInt32(ReadBytes(img.Size, ref pixels, 128, 32), 0);
                int position = 160;
                for (int i = 0; i < fileLength; i++)
                {
                    string extention = Encoding.ASCII.GetString(ReadBytes(img.Size, ref pixels, position, 24));
                    int fileSize = BitConverter.ToInt32(ReadBytes(img.Size, ref pixels, position + 24, 32), 0);
                    byte[] data = ReadBytes(img.Size, ref pixels, position + 56, fileSize);
                    DecryptedFile file = new DecryptedFile()
                    {
                        Data = data,
                        Extention = extention
                    };
                    files.Add(file);
                    position += 56 + fileSize;
                }
                return files;
            }
            return null;
        }

        public static bool IsEncryptedImage(Bitmap img)
        {
            Color[,] pixels = GetImagePixels(img);
            return IsEncryptedImage(img, ref pixels);
        }

        public static bool IsEncryptedImage(Bitmap img, ref Color[,] pixels)
        {
            try
            {
                byte[] startSequence = ReadBytes(img.Size, ref pixels, 0, startSequenceArr.Length * 8);
                if (startSequence.Length == startSequenceArr.Length)
                {
                    for (int i = 0; i < startSequence.Length; i++)
                    {
                        if (startSequence[i] != startSequenceArr[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch
            {

            }
            return false;

        }

        public static void Encrypt(string imgPath, string[] encFiles, string saveLocation)
        {
            Bitmap img = new Bitmap(imgPath);
            Encrypt(img, encFiles, saveLocation);
        }

        public static void Encrypt(Bitmap img, string[] encFiles, string saveLocation)
        {
            List<BitArray> files = new List<BitArray>();
            foreach (string path in encFiles)
            {
                BitArray fileArray = GetFileBits(path);
                files.Add(fileArray);
            }

            int imageMaxSize = MaxBitContent(img);
            int requiredBytes = TotalBits(files);
            bool changeSize = imageMaxSize < requiredBytes;
            if (changeSize)
            {
                Size requiredSize = GetRequiredImageSize(img, requiredBytes);
                img = ResizeImage(img, requiredSize.Width, requiredSize.Height);
            }
            Color[,] pixels = GetImagePixels(img);

            //Add parity and size bits
            int position = 0;
            Write(img.Size, ref pixels, startSequenceArr, ref position);
            Write(img.Size, ref pixels, BitConverter.GetBytes(files.Count), ref position);

            //Fill pixels with file data         
            int pos = 0;
            foreach (BitArray file in files)
            {
                int lastDot = encFiles[pos].LastIndexOf(".");
                string extention = encFiles[pos].Substring(lastDot + 1).PadRight(3);
                if (extention.Length > 3)
                {
                    extention = extention.Substring(0, 3);
                }
                Write(img.Size, ref pixels, Encoding.ASCII.GetBytes(extention), ref position);
                Write(img.Size, ref pixels, BitConverter.GetBytes(file.Length), ref position);
                Write(img.Size, ref pixels, file, ref position);
            }

            Bitmap newImg = new Bitmap(img.Size.Width, img.Size.Height, PixelFormat.Format32bppArgb);
            for (int x = 0; x < img.Size.Width; x++)
            {
                for (int y = 0; y < img.Size.Height; y++)
                {
                    newImg.SetPixel(x, y, pixels[x, y]);
                }
            }
            newImg.Save(saveLocation);
        }

        static void Write(Size size, ref Color[,] pixels, byte[] bytes, ref int position)
        {
            BitArray bits = new BitArray(bytes);
            Write(size, ref pixels, bits, ref position);
        }

        static void Write(Size size, ref Color[,] pixels, BitArray bits, ref int position)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                byte value = GetIndexValueByte(size, pixels, position);
                bool valueBool = bits[i];
                bool oddIsTrue = IsOdd(position);
                bool currentValueIsOdd = IsOdd(value);
                bool isWronfullyOddOrEven = (currentValueIsOdd == oddIsTrue) != valueBool;
                if (isWronfullyOddOrEven)
                {
                    SetIndexValueByte(size, ref pixels, position, ChangeOddEvenByte(value));
                }
                position++;
            }
        }

        static byte[] ReadBytes(Size size, ref Color[,] pixels, int position, int bitLength)
        {
            var bits = Read(size, ref pixels, position, bitLength);
            byte[] bytes = new byte[bitLength / 8];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        static BitArray Read(Size size, ref Color[,] pixels, int position, int bitLength)
        {
            bool[] bits = new bool[bitLength];
            for (int i = 0; i < bitLength; i++)
            {
                int index = position + i;
                byte value = GetIndexValueByte(size, pixels, index);
                bool oddIsTrue = IsOdd(index);
                bool currentValueIsOdd = IsOdd(value);
                bits[i] = oddIsTrue == currentValueIsOdd;
            }
            return new BitArray(bits);
        }

        static byte ChangeOddEvenByte(byte b)
        {
            if (b < 255 && b >= 0)
            {
                b = (byte)(b + 1);
            }
            else
            {
                b = (byte)(b - 1);
            }
            return b;
        }

        static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        static bool IsOdd(byte value)
        {
            return value % 2 != 0;
        }

        static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            int index = 8 - source.Length;
            foreach (bool b in source)
            {
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }

        static bool[] ConvertByteToBoolArray(byte b)
        {
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            Array.Reverse(result);

            return result;
        }

        static byte GetIndexValueByte(Size size, Color[,] pixels, int index)//Byte index (not bit)
        {
            Vector2 coordinates = GetIndexCoordinates(size, index);
            Color color = pixels[coordinates.X, coordinates.Y];
            int rgb = GetRGBACoordinate(index);
            switch (rgb)
            {
                case 0://A
                    return color.A;
                case 1://R
                    return color.R;
                case 2://G
                    return color.G;
                case 3://B
                    return color.B;
                default:
                    throw new Exception("Colour channel not implemented");
            }
        }

        static void SetIndexValueByte(Size size, ref Color[,] pixels, int index, byte value)//Byte index (not bit)
        {
            Vector2 coordinates = GetIndexCoordinates(size, index);
            Color color = pixels[coordinates.X, coordinates.Y];
            int rgb = GetRGBACoordinate(index);
            switch (rgb)
            {
                case 0://A
                    pixels[coordinates.X, coordinates.Y] = Color.FromArgb(value, color.R, color.G, color.B);
                    break;
                case 1://R
                    pixels[coordinates.X, coordinates.Y] = Color.FromArgb(color.A, value, color.G, color.B);
                    break;
                case 2://G
                    pixels[coordinates.X, coordinates.Y] = Color.FromArgb(color.A, color.R, value, color.B);
                    break;
                case 3://B
                    pixels[coordinates.X, coordinates.Y] = Color.FromArgb(color.A, color.R, color.G, value);
                    break;
                default:
                    throw new Exception("Colour channel not implemented");
            }
        }

        static int GetRGBACoordinate(int index)
        {
            return index % 4;
        }

        static Vector2 GetIndexCoordinates(Size size, int index)
        {
            int indexCorrected = index / ImageChannels;
            int x = indexCorrected % size.Width;    // % is the "modulo operator", the remainder of i / width;
            int y = indexCorrected / size.Width;    // where "/" is an integer division
            if (x >= size.Width)
            {
                throw new IndexOutOfRangeException($"Image is not wide enough for X={x}");
            }
            if (y >= size.Height)
            {
                throw new IndexOutOfRangeException($"Image is not heigh enough for Y={y}");
            }
            return new Vector2(x, y);
        }

        static Size GetRequiredImageSize(Bitmap img, int requiredBytes)
        {
            Size size = img.Size;
            int amount = 1;
            int imageMaxSize = MaxBitContent(img);
            while (imageMaxSize < requiredBytes)
            {
                size = ResizeKeepAspect(img.Size, img.Width + amount, img.Height + amount);
                imageMaxSize = MaxBitContent(size);
                amount++;
            }
            return size;
        }

        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        static Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight)
        {
            decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }

        static BitArray GetFileBits(string file)
        {
            return new BitArray(GetFileBytes(file));
        }

        static byte[] GetFileBytes(string file)
        {
            return File.ReadAllBytes(file);
        }

        static Color[,] GetImagePixels(Bitmap image)
        {
            Color[,] pixels = new Color[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    pixels[x, y] = pixel;
                }
            }
            return pixels;
        }
    }

    public class DecryptedFile
    {
        public string Extention { get; set; }
        public byte[] Data { get; set; }

        public override string ToString()
        {
            return Extention;
        }
    }
}
