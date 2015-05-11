using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public class MusicDictionary
    {
        int CSharpLow, E, FSharp, Gsharp, B, CSharpHigh,Rest;
        public int[] NoteArray;
        
        public MusicDictionary()
        {
            CSharpLow = 554;
            E = 659;
            FSharp = 740;
            Gsharp = 831;
            B = 988;
            CSharpHigh = 1108;
            Rest = 37;
            NoteArray = new int[] {CSharpLow,E,FSharp,Gsharp,B,CSharpHigh,Rest,Rest};

        }
    }
}
