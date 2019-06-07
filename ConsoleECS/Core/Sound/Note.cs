using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleECS.Core.Sound
{
    public class Note
    {
        public enum Key
        {
            C,
            Db,
            D,
            Eb,
            E,
            F,
            Gb,
            G,
            Ab,
            A,
            Bb,
            B
        }
        public bool pause = false;
        public Key key = Key.C;
        public int octave = 4;
        public int duration = 500;

        public int Frequency => (int)Fn(HalfStepsFromF0(key, octave));

        public void Beep()
        {
            if (pause)
            {
                Thread.Sleep(duration);
            }
            else
            {
                Console.Beep(Frequency, duration);
            }
        }

        public static Note Create(Key key, int octave, int duration = 1000)
        {
            return new Note()
            {
                key = key,
                octave = octave,
                duration = duration
            };
        }
        public static Note Create(string keyName, int octave, int duration = 1000)
        {
            return Create(NameToKey(keyName), octave, duration);
        }

        /**
         *  Usage
         *  A note must start with the key name (e.g: C, Ab, F#)
         *  After the key name you can put a number representing the octave (e.g: C4, Ab5, F#3)
         *  If the octave is missing the note will have the default value of 4
         *  After that you can put a colon and the number of miliseconds you want the note to last (e.g: C4:800, Ab5:350, F#3:1000)
         *  The number of miliseconds must always be bigger than 100
         *  if the duration is missing the note will have default duration of 500 miliseconds
         *  if you put a number smaller than 100, this number will be multiplied by 100 to obtain the final duration
         *  (e.g: C4:8 will have duration of 800 ms)
         *  To put a pause, just put the number of miliseconds for the duration of the pause
         *  
         *  Valid input examples:
         *  "C#" = C#/Db, octave 4, 500 ms duration
         *  "F:800" = F, octave 4, 800 ms duration
         *  "400" = is a pause of 400 ms
         *  "E5" = E, octave 5, 500 ms duration
         *  "Bb" = A#/Bb, octave 4, 500 ms duration
         *  "A#5" = A#/Bb, octave 5, 500 ms duration
         */
        public static Note Parse(string noteString)
        {
            if (char.IsLetter(noteString[0]))
            {
                string keyName = "" + noteString[0];
                if (noteString.Length > 1 && (noteString[1] == '#' || noteString[1] == 'b'))
                {
                    keyName += noteString[1];
                }
                Note.Key key = Note.NameToKey(keyName);
                //Console.Write(key.ToString());

                noteString = noteString.Substring(keyName.Length);
                var octaveString = "";
                for (int p = 0; p < noteString.Length && char.IsDigit(noteString[p]); p++)
                {
                    octaveString += noteString[p];
                }

                //Console.Write(octaveString);
                int.TryParse(octaveString, out int octave);
                if (octave == 0) octave = 4;


                noteString = noteString.Substring(octaveString.Length);
                if (noteString.Length > 1) noteString = noteString.Substring(1);

                //Console.Write(piece);
                int.TryParse(noteString, out int duration);
                if (duration == 0) duration = 500;
                if (duration < 100) duration *= 100;

                //Console.WriteLine(" = " + Note.Create(key, octave, duration) + " " + duration);
                return Note.Create(key, octave, duration);
            }
            else
            {
                //Console.Write(piece);
                int.TryParse(noteString, out int duration);
                //if (duration < 100) duration *= 100;
                return Note.Pause(duration);
                //Console.WriteLine(" = p" + duration);
            }
        }

        public static Note Pause(int duration)
        {
            return new Note()
            {
                pause = true,
                duration = duration
            };
        }

        public override string ToString()
        {
            return key.ToString() + octave;
        }

        static Dictionary<string, Key> nameToKeys = new Dictionary<string, Key>()
                {
                    { "C", Key.C },
                    { "C#", Key.Db },
                    { "Db", Key.Db },
                    { "D", Key.D },
                    { "D#", Key.Eb },
                    { "Eb", Key.Eb },
                    { "E", Key.E },
                    { "F", Key.F },
                    { "F#", Key.Gb },
                    { "Gb", Key.Gb },
                    { "G", Key.G },
                    { "G#", Key.Ab },
                    { "Ab", Key.Ab },
                    { "A", Key.A },
                    { "A#", Key.Bb },
                    { "Bb", Key.Bb },
                    { "B", Key.B },
                };
        public static Key NameToKey(string name)
        {
            return nameToKeys[name];
        }

        const double a = 1.059463094359;
        const Key f0Key = Key.A;
        const int f0Octave = 4;
        const double f0 = 440; // A0 = 440Hz

        public static int FrequencyOf(Key key, int octave)
        {
            return (int)Fn(HalfStepsFromF0(key, octave));
        }
        static double Fn(int halfSteps)
        {
            return f0 * Math.Pow(a, halfSteps);
        }
        public static int HalfStepsFromF0(Note note)
        {
            return HalfStepsFromF0(note.key, note.octave);
        }
        public static int HalfStepsFromF0(Key key, int octave)
        {
            return (octave - f0Octave) * 12 - (f0Key - key);
            /*
            int delta = 0;

            int octaveDelta = octave - f0Octave;
            delta += 12 * octaveDelta;

            int keyDelta = f0Key - key;
            delta -= keyDelta;

            return delta;
            */
        }
    }
}
