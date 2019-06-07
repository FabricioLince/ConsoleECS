using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleECS.Core.Sound
{
    static class SoundModule
    {
        public class Player
        {
            Thread thread;
            public bool IsPlaying { get; private set; }
            public bool Loop { get; set; }
            IEnumerable<Note> notes;

            public Player(IEnumerable<Note> notes)
            {
                this.notes = notes;
                IsPlaying = false;
                Loop = false;
            }

            public static Player Play(IEnumerable<Note> notes, bool loop = false)
            {
                Player p = new Player(notes) { Loop = loop };
                p.Play();
                return p;
            }

            public void Play()
            {
                if (IsPlaying) return;
                IsPlaying = true;
                thread = new Thread((p1) => (p1 as Player).PlayerThread(notes));
                thread.Start(this);
            }
            public void Stop()
            {
                if (!IsPlaying) return;
                thread.Abort();
                IsPlaying = false;
            }

            void PlayerThread(IEnumerable<Note> notes)
            {
                IsPlaying = true;
                do
                {
                    foreach (var note in notes)
                    {
                        note.Beep();
                    }
                } while (Loop);
                IsPlaying = false;
            }
        }

        public static Player Play(params Note[] notes)
        {
            return Player.Play(notes);
        }
        public static Player Play(IEnumerable<Note> notes)
        {
            return Player.Play(notes);
        }
        public static Player Play(string notes)
        {
            return Player.Play(Parse(notes));
        }

        /**
         *  Usage
         *  Pass a NoteList as a single string containing comma separated notes
         *  Each note must start with the key name (e.g: C, Ab, F#)
         *  After the key name you can put a number representing the octave (e.g: C4, Ab5, F#3)
         *  If the octave is missing the note will have the default value of 4
         *  After that you can put a colon and the number of miliseconds you want the note to last (e.g: C4:800, Ab5:350, F#3:1000)
         *  The number of miliseconds must always be bigger than 100
         *  if the duration is missing the note will have default duration of 500 miliseconds
         *  if you put a number smaller than 100, this number will be multiplied by 100 to obtain the final duration
         *  (e.g: C4:8 will have duration of 800 ms)
         *  To put a pause between note, just put the number of miliseconds for the duration of the pause
         *  
         *  Valid string example:
         *  "C#, F:800, 400, E5, Bb, A#5"
         *  which breaks down to
         *  C# = 1st note is C#/Db, octave 4, 500 ms duration
         *  F:800 = 2nd note is F, octave 4, 800 ms duration
         *  400 = is a pause of 400 ms
         *  E5 = 3rd note is E, octave 5, 500 ms duration
         *  Bb = 4th note is A#/Bb, octave 4, 500 ms duration
         *  A#5 = 5th note is A#/Bb, octave 5, 500 ms duration
         * 
         * */
        public static List<Note> Parse(string notes)
        {
            var list = new List<Note>();

            var pieces = notes.Split(',');
            for (int i = 0; i < pieces.Length; ++i) 
            {
                list.Add(Note.Parse(pieces[i].Trim()));
            }

            return list;
        }
    }
}
