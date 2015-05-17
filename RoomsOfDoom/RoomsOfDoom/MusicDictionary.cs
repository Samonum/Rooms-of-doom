using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public class MusicDictionary
    {
        int C, CSharpLow, DSharp, E, F, FSharp, G, Gsharp,BFlat, B, CHigh, CSharpHigh,Rest;
        public int[] NoteArray,NoteArrayBlues;
        
        public MusicDictionary()
        {
            C = 523; 
            CSharpLow = 554;
            DSharp = 622;
            E = 659;
            F = 698;
            FSharp = 740;
            G = 784;
            Gsharp = 831;
            BFlat = 932;
            B = 988;
            CHigh = 1047;
            CSharpHigh = 1108;
            Rest = 37;
            NoteArray = new int[] {CSharpLow,E,FSharp,Gsharp,B,CSharpHigh,Rest,Rest};
            NoteArrayBlues = new int[] {C,DSharp, F,FSharp,G,BFlat,CHigh };

        }
    }
}
