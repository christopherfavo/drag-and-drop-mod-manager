using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dsdad
{
    class EXE
    {
        public static List<bool> isModified = new List<bool>(new bool[21]);

        public static List<UInt32> defOffsets = new List<UInt32>
        {
            0xD57F14,   //menu
            0xD65DAC,   //shader
            0xD65DF4,   //font
            0xD65E34,   //msg
            0xD65EA4,   //chr
            0xD65FFC,   //obj
            //0xD660F8, //item
            0xD6613C,   //parts
            0xD66180,   //map
            0xD6627C,   //remo
            0xD662C8,   //map/breakobj
            0xD662C8,   //map/mapstudio
            0xD6636C,   //mtd
            0xD663B0,   //param
            0xD66434,   //paramdef
            0xD66484,   //facegen
            0xD665F0,   //sound
            0xD666E4,   //other
            0xD66734,   //script
            0xD667C4,   //script/talk
            0xD66C90,   //sfx
            0xD66EAC    //event
        };

        public static List<UInt32> debOffsets = new List<UInt32>
        {
            0xD5C2D4,   //menu
            0xD68074,   //shader
            0xD680BC,   //font
            0xD680FC,   //msg
            0xD6816C,   //chr
            0xD682C4,   //obj
            //0xD683C0  //item
            0xD68404,   //parts
            0xD68448,   //map
            0xD68544,   //remo
            0xD68590,   //map/breakobj
            0xD685E0,   //map/mapstudio
            0xD68634,   //mtd
            0xD68678,   //param
            0xD686FC,   //paramdef
            0xD6874C,   //facegen
            0xD688B8,   //sound
            0xD689AC,   //other
            0xD689FC,   //script
            0xD68A8C,   //script/talk
            0xD68F58,   //sfx
            0xD69174,   //event
        };

        public static List<string> dvdbnds = new List<string>
        {
            "dvdbnd1",
            "dvdbnd1",
            "dvdbnd1",
            "dvdbnd3",
            "dvdbnd0",
            //"dvdbnd0",
            "dvdbnd1",
            "dvdbnd1",
            "dvdbnd0",
            "dvdbnd0",
            "dvdbnd0",
            "dvdbnd0",
            "dvdbnd1",
            "dvdbnd3",
            "dvdbnd3",
            "dvdbnd1",
            "dvdbnd1",
            "dvdbnd1",
            "dvdbnd2",
            "dvdbnd2",
            "dvdbnd0",
            "dvdbnd2",
        };

        public static void exeCheck(BinaryReader exe, bool isDebug)
        {
            char[] dvdbnd = { };
            List<UInt32> offsets = new List<UInt32> { };

            if (isDebug) offsets = debOffsets;
            else offsets = defOffsets;

            for (int i = 0; i < offsets.Count; i++)
            {
                exe.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
                dvdbnd = exe.ReadChars(7);
                isModified[i] = new string (dvdbnd) != dvdbnds[i];
            }
        }

        public static bool debCheck(BinaryReader exe)
        {
            exe.BaseStream.Seek(0x80, SeekOrigin.Begin);

            return exe.ReadByte() == 0xB4;
        }

        public static void modifyExe(bool isDebug, bool dcxEnabled)
        {
            string path = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"));
            BinaryWriter exe = new BinaryWriter(File.Open(path, FileMode.Open), System.Text.Encoding.Unicode);

            List<UInt32> offsets = new List<UInt32> { };

            if (isDebug) offsets = debOffsets;
            else offsets = defOffsets;

            for (int i = 0; i < offsets.Count; i++)
            {
                exe.BaseStream.Seek(offsets[i], SeekOrigin.Begin);

                if (isModified[i]) exe.Write("dvdroot".ToCharArray());
                else exe.Write(dvdbnds[i].ToCharArray());
            }

            byte dcxByte = 0x00;
            if (dcxEnabled) dcxByte = 0x74;
            else dcxByte = 0xEB;

            if (!isDebug)
            {
                exe.BaseStream.Seek(0x8FB816, SeekOrigin.Begin);
            }
            else
            {
                exe.BaseStream.Seek(0x8FB726, SeekOrigin.Begin);
            }

            exe.Write(dcxByte);

            exe.Close();
        }

        public static bool dcxCheck(BinaryReader exe, bool isDebug)
        {
            if (!isDebug)
            {
                exe.BaseStream.Seek(0x8FB816, SeekOrigin.Begin);
            }
            else
            {
                exe.BaseStream.Seek(0x8FB726, SeekOrigin.Begin);
            }

            return exe.ReadByte() == 0x74;
        }

    }
}
