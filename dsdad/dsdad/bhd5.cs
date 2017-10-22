using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dsdad
{
    class BHD5
    {
        public struct EntryStruct
        {
            public UInt32  hash;
            public UInt32  size;
            public UInt64  offset;
        }

        public struct BucketStruct
        {
            public UInt32              entryCount;
            public UInt32              entryOffset;
            public List<EntryStruct>   entries;
        }

        public struct Bhd5HeaderStruct
        {
            public char[] signature;
            public UInt32 endianCheck;
            public UInt32 version;
            public UInt32 dataSize;
            public UInt32 bucketCount;
            public UInt32 bucketOffset;
        }

        public struct Bhd5Struct
        {
            public Bhd5HeaderStruct    header;
            public List<BucketStruct>  buckets;
        }

        public static Bhd5Struct GetBhd5Data(string path)
        {
            Bhd5Struct bhd5;

            BinaryReader bhd5file = new BinaryReader(File.Open(path, FileMode.Open));

            bhd5.header.signature    = bhd5file.ReadChars(4);
            bhd5.header.endianCheck  = bhd5file.ReadUInt32();
            bhd5.header.version      = bhd5file.ReadUInt32();
            bhd5.header.dataSize     = bhd5file.ReadUInt32();
            bhd5.header.bucketCount  = bhd5file.ReadUInt32();
            bhd5.header.bucketOffset = bhd5file.ReadUInt32();

            bhd5.buckets = new List<BucketStruct> { };

            for (int i = 0; i < bhd5.header.bucketCount; i++)
            {
                BucketStruct bucket;

                bucket.entryCount   = bhd5file.ReadUInt32();
                bucket.entryOffset  = bhd5file.ReadUInt32();
                bucket.entries      = new List<EntryStruct> { };

                bhd5.buckets.Add(bucket);
            }

            foreach (BucketStruct bucket in bhd5.buckets)
            {
                for (int j = 0; j < bucket.entryCount; j++)
                {
                    EntryStruct entry;

                    entry.hash      = bhd5file.ReadUInt32();
                    entry.size      = bhd5file.ReadUInt32();
                    entry.offset    = bhd5file.ReadUInt64();

                    bucket.entries.Add(entry);
                }
            }

            bhd5file.Close();

            return bhd5;
        }

        public static UInt32 GetHash(string filename)
        {
            UInt32 hash = 0;

            foreach (char i in filename)
            {
                hash = hash * 37 + (UInt32)(Char.ToLowerInvariant(i));
            }

            return hash;
        }

        public static bool FindHash(Bhd5Struct bhd5, UInt32 hash, ref int i, ref int j)
        {
            bool l = false;
            i = 0;

            while(!l & i < bhd5.header.bucketCount)
            {
                j = 0;

                while(!l & j < bhd5.buckets[i].entryCount)
                {
                    l = (hash == bhd5.buckets[i].entries[j].hash);
                    if (!l) j++;
                }

                if (!l) i++;
            }

            return l;
        }

        public static void RewriteBhd5(Bhd5Struct bhd5, string path)
        {
            BinaryWriter bhd5file = new BinaryWriter(File.Open(path, FileMode.Create));

            bhd5file.Write(bhd5.header.signature);
            bhd5file.Write(bhd5.header.endianCheck);
            bhd5file.Write(bhd5.header.version);
            bhd5file.Write(bhd5.header.dataSize);
            bhd5file.Write(bhd5.header.bucketCount);
            bhd5file.Write(bhd5.header.bucketOffset);

            foreach (BucketStruct bucket in bhd5.buckets)
            {
                bhd5file.Write(bucket.entryCount);
                bhd5file.Write(bucket.entryOffset);
            }

            foreach (BucketStruct bucket in bhd5.buckets)
            {
                foreach (EntryStruct entry in bucket.entries)
                {
                    bhd5file.Write(entry.hash);
                    bhd5file.Write(entry.size);
                    bhd5file.Write(entry.offset);
                }
            }

            bhd5file.Close();
        }
    }
}
