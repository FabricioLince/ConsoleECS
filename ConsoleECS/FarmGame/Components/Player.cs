using ConsoleECS.Core.Components;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS.FarmGame.Components
{
    using Sound = Core.Sound.SoundModule;
    using Note = Core.Sound.Note;

    [Dependecies(typeof(Position), typeof(Collider))]
    class Player : Script
    {
        [AssignDependence]
        Position position;

        [AssignDependence]
        Collider collider;

        public double speed = 20;
        public Vector2Int direction = Vector2Int.Up;

        bool keyDown = false;

        public KeyCode upKey = KeyCode.Up;
        public KeyCode downKey = KeyCode.Down;
        public KeyCode leftKey = KeyCode.Left;
        public KeyCode rightKey = KeyCode.Right;
        public KeyCode actKey = KeyCode.X;

        Vector2Int lastStep;
        List<Note> walkNotes = Sound.Parse("C:1, C#:1");
        int i = 0;
        Note walkNote => walkNotes[i++ % walkNotes.Count];
        Sound.Player soundPlayer;

        public override void Loop()
        {
            Move();
            Act();
        }

        void Act()
        {
            if (NativeKeyboard.IsKeyDown(actKey))
            {
                var list = position.system.FindPosition(position.Vector2Int + direction);
                foreach (var p in list)
                {
                    var crop = p.Entity.GetComponent<Crop>();
                    if (crop != null)
                    {
                        if(crop.needsWater)
                        {
                            crop.WaterCrop();
                            PlayImmediatly(Sound.Parse("E:1, F:1, F#:2"));
                        }
                    }
                }
            }
        }

        void Move()
        {
            if (!NativeKeyboard.IsKeyToggled(KeyCode.CapsLock))
            {
                keyDown = false;
            }

            Vector2 movement = Vector2.Zero;

            if (NativeKeyboard.IsKeyDown(upKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Up;
                movement += Vector2.Up;
            }
            else if (NativeKeyboard.IsKeyDown(downKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Down;
                movement += Vector2.Down;
            }
            else if (NativeKeyboard.IsKeyDown(leftKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Left;
                movement += Vector2.Left;
            }
            else if (NativeKeyboard.IsKeyDown(rightKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Right;
                movement += (Vector2.Right);
            }
            else
            {
                keyDown = false;
            }

            if (Math.Abs(movement.x) > 0 ||
                Math.Abs(movement.y) > 0)
            {
                //if (!collider.Move(movement, speed * Engine.DeltaTime)) Console.Beep(740, 100);
                if (collider.Move(movement, speed * Engine.DeltaTime))
                {
                    if (lastStep != position.Vector2Int)
                    {
                        lastStep = position.Vector2Int;
                        PlayIfCan(walkNote);
                    }
                }
                else
                {
                    PlayIfCan(Note.Parse("C3:2"));
                }
            }
        }


        // Will probably move to its own component
        void PlayIfCan(Note note)
        {
            if (soundPlayer == null || !soundPlayer.IsPlaying)
            {
                soundPlayer = Sound.Play(note);
            }
        }
        void PlayImmediatly(IEnumerable<Note> notes)
        {
            if (soundPlayer != null) soundPlayer.Stop();
            soundPlayer = Sound.Play(notes);
        }
    }
}
