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
    public class ImageEncryptDecrypt
    {
        public Bitmap current;
        LockBitmap currentLock;
        const int ImageChannels = 4;
        static byte[] startSequenceArr = new byte[] { 227, 57, 109, 141, 220, 123, 31, 99, 42, 74, 99, 12, 108, 199, 212, 240 };

        public ImageEncryptDecrypt(Bitmap current)
        {
            this.current = Convert(current);
            currentLock = new LockBitmap(this.current);
        }

        Bitmap Convert(Bitmap bmp)
        {
            int Depth = Bitmap.GetPixelFormatSize(bmp.PixelFormat);
            if (Depth != 32)
            {
                Bitmap clone = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);

                using (Graphics gr = Graphics.FromImage(clone))
                {
                    gr.DrawImage(bmp, new Rectangle(0, 0, clone.Width, clone.Height));
                }
                return clone;
            }
            return bmp;
        }

        int MaxBitContent
        {
            get
            {
                //Image width multiplied by image height multiplied by 3 (RGB channels)
                return current.Size.Width * current.Size.Height * ImageChannels;
            }
        }

        int TotalBits(List<BitArray> files)
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

        public List<DecryptedFile> Decrypt()
        {
            try
            {
                currentLock.LockBits();
                if (IsEncryptedImage)
                {
                    List<DecryptedFile> files = new List<DecryptedFile>();
                    int fileLength = BitConverter.ToInt32(ReadBytes(128, 32), 0);
                    int position = 160;
                    for (int i = 0; i < fileLength; i++)
                    {
                        string extention = Encoding.ASCII.GetString(ReadBytes(position, 24));
                        int fileSize = BitConverter.ToInt32(ReadBytes(position + 24, 32), 0);
                        byte[] data = ReadBytes(position + 56, fileSize);
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
            catch (Exception ex)
            {

            }
            finally
            {
                currentLock.UnlockBits();
            }
            return null;
        }

        public bool IsEncryptedImage
        {
            get
            {
                bool unlockAgain = !currentLock.Locked;
                if(!currentLock.Locked)
                {
                    currentLock.LockBits();
                }
                try
                {
                    byte[] startSequence = ReadBytes(0, startSequenceArr.Length * 8);
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
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if(unlockAgain)
                    {
                        currentLock.UnlockBits();
                    }
                }
            }
        }

        public void Encrypt(string[] encFiles, string saveLocation)
        {
            try
            {
                currentLock.LockBits();

                List<BitArray> files = new List<BitArray>();
                foreach (string path in encFiles)
                {
                    BitArray fileArray = GetFileBits(path);
                    files.Add(fileArray);
                }

                int requiredBytes = TotalBits(files);
                bool changeSize = MaxBitContent < requiredBytes;
                if (changeSize)
                {
                    Size requiredSize = GetRequiredImageSize(requiredBytes);
                    current = ResizeImage(requiredSize.Width, requiredSize.Height);
                    currentLock = new LockBitmap(current);
                }

                //Add parity and size bits
                int position = 0;
                Write(startSequenceArr, ref position);
                Write(BitConverter.GetBytes(files.Count), ref position);

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
                    Write(Encoding.ASCII.GetBytes(extention), ref position);
                    Write(BitConverter.GetBytes(file.Length), ref position);
                    Write(file, ref position);
                }

                Bitmap newImg = new Bitmap(current.Size.Width, current.Size.Height, PixelFormat.Format32bppArgb);
                LockBitmap newImgLock = new LockBitmap(newImg);
                newImgLock.LockBits();
                for (int x = 0; x < current.Size.Width; x++)
                {
                    for (int y = 0; y < current.Size.Height; y++)
                    {
                        newImgLock.SetPixel(x, y, currentLock.GetPixel(x, y));
                    }
                }
                newImgLock.UnlockBits();
                newImg.Save(saveLocation);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                currentLock.UnlockBits();
            }
        }

        void Write(byte[] bytes, ref int position)
        {
            BitArray bits = new BitArray(bytes);
            Write(bits, ref position);
        }

        void Write(BitArray bits, ref int position)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                byte value = GetIndexValueByte(position);
                bool valueBool = bits[i];
                bool oddIsTrue = IsOdd(position);
                bool currentValueIsOdd = IsOdd(value);
                bool isWronfullyOddOrEven = (currentValueIsOdd == oddIsTrue) != valueBool;
                if (isWronfullyOddOrEven)
                {
                    SetIndexValueByte(position, ChangeOddEvenByte(value));
                }
                position++;
            }
        }

        byte[] ReadBytes(int position, int bitLength)
        {
            var bits = Read(position, bitLength);
            byte[] bytes = new byte[bitLength / 8];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        BitArray Read(int position, int bitLength)
        {
            bool[] bits = new bool[bitLength];
            for (int i = 0; i < bitLength; i++)
            {
                int index = position + i;
                byte value = GetIndexValueByte(index);
                bool oddIsTrue = IsOdd(index);
                bool currentValueIsOdd = IsOdd(value);
                bits[i] = oddIsTrue == currentValueIsOdd;
            }
            return new BitArray(bits);
        }

        byte ChangeOddEvenByte(byte b)
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

        bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        bool IsOdd(byte value)
        {
            return value % 2 != 0;
        }

        byte GetIndexValueByte(int index)//Byte index (not bit)
        {
            return currentLock.GetPixelSingle(index);
        }

        void SetIndexValueByte(int index, byte value)//Byte index (not bit)
        {
            currentLock.SetPixelSingle(index, value);
        }

        Size GetRequiredImageSize(int requiredBytes)
        {
            Size size = current.Size;
            int amount = 1;
            while (MaxBitContent < requiredBytes)
            {
                size = ResizeKeepAspect(current.Size, current.Width + amount, current.Height + amount);
                amount++;
            }
            return size;
        }

        Bitmap ResizeImage(int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(current.HorizontalResolution, current.VerticalResolution);

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
                    graphics.DrawImage(current, destRect, 0, 0, current.Width, current.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight)
        {
            decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }

        BitArray GetFileBits(string file)
        {
            return new BitArray(GetFileBytes(file));
        }

        byte[] GetFileBytes(string file)
        {
            return File.ReadAllBytes(file);
        }
    }
}
