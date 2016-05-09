using System;
using System.IO;
using System.Text;

namespace DungeonMasterParser
{
    class DMEncoding : Encoding
    {
        public override int GetByteCount(char[] chars, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new NotImplementedException();
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return new DMDecoder().GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return new DMDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount / 3 * 2;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount / 2 * 3;
        }

        public override Decoder GetDecoder()
        {
            return new DMDecoder();
        }

        //TODO encoder , all methods reimplementation

        

        class DMDecoder : Decoder
        {
            const ushort fiveBitsMask = 0x001F;
            byte? remainingByte = null;

            public override int GetCharCount(byte[] bytes, int index, int count)
            {
                if (remainingByte == null)
                    return count / 2 * 3;
                else
                    return ((count - 1) / 2) * 3 + 3;
            }

            public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
            {
                int i = charIndex;
                using (var r = new BinaryReader(new MemoryStream(bytes, byteIndex, byteCount)))
                {
                    if (remainingByte != null)
                    {
                        ushort t = r.ReadByte();
                        t <<= 8;
                        t |= remainingByte.Value;
                        DecodeCharacters(t, chars, ref i);
                        remainingByte = null;
                    }

                    while (r.BaseStream.Position <= r.BaseStream.Length - 2)
                        DecodeCharacters(r.ReadUInt16(), chars, ref i);

                    if (r.BaseStream.Position == r.BaseStream.Length - 1)
                        remainingByte = r.ReadByte();
                }

                return i - charIndex;
            }


            public override void Reset()
            {
                base.Reset();
                remainingByte = null;
            }

            private void DecodeCharacters(ushort t, char[] chars, ref int i)
            {
                foreach (int j in new int[] { (t >> 10) & fiveBitsMask, (t >> 5) & fiveBitsMask, (t >> 0) & fiveBitsMask })
                {
                    if (j <= 25) //letter
                    {
                        chars[i++] = (char)('A' + j);
                    }
                    else
                    {
                        switch (j)
                        {
                            case 26: chars[i++] = ' '; break;
                            case 27: chars[i++] = '.'; break;

                            //The separator character is decoded differently depending on the type of text:
                            //  Wall texts: Separator is decoded as a 80h byte.
                            //  Message: Separator is decoded as a 'Space' character
                            //  Scroll and champion texts: Separator is decoded as a 'Line Feed' character
                            case 28: chars[i++] = '|'; break;

                            //No real use in game
                            case 29: chars[i++] = '#'; break;

                            //Link to text table (Each string in the table contains up to 7 characters (8 bytes per string). )
                            case 30: chars[i++] = '$'; break;

                            //end of text
                            case 31: chars[i++] = '\n'; break;

                            //fail to decode character
                            default: chars[i++] = '&'; break;
                        }
                    }
                }
            }
        }
    }
}
