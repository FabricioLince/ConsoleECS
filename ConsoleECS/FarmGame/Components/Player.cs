using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Input;
using ConsoleECS.Core.Vector;
using System;
using System.Collections.Generic;
#pragma warning disable 649, 169

namespace ConsoleECS.FarmGame.Components
{
    using Sound = Core.Sound.SoundModule;
    using Note = Core.Sound.Note;

    [Dependecies(typeof(Position), typeof(Collider), typeof(Holder))]
    class Player : Script
    {
        [AssignDependence] public Position Position { get; private set; }
        [AssignDependence] public Collider Collider { get; private set; }
        [AssignDependence] public Holder Holder { get; private set; }

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

        bool actKeyDown = false;
        void Act()
        {
            if (Keyboard.IsKeyDown(actKey))
            {
                if (actKeyDown) return;
                actKeyDown = true;

                if (Holder.holding)
                {
                    var plantBox = ComponentInFront<PlantBox>();
                    if (plantBox && plantBox.Empty)
                    {
                        if (Holder.holding is Seed)
                        {
                            plantBox.Plant(Holder.holding as Seed);
                            Engine.DestroyEntity(Holder.holding.Entity);
                            return;
                        }
                    }

                    var coll = ComponentInFront<Collider>();
                    if (!coll || !coll.enabled)
                    {
                        Holder.DropItemOn(Position.Vector2Int + direction);
                        return;
                    }

                    return;
                }

                var list = Position.system.FindPosition(Position.Vector2Int + direction);
                foreach (var p in list)
                {
                    var crop = p.Entity.GetComponent<Crop>();
                    if (crop)
                    {
                        if (crop.needsWater)
                        {
                            crop.WaterCrop();
                            PlayImmediatly(Sound.Parse("E:1, F:1, F#:2"));
                        }
                        else if (crop.Dead)
                        {
                            Engine.DestroyEntity(crop.Entity);
                        }
                        break;
                    }
                    else
                    {
                        var item = p.Entity.GetComponent<HoldableItem>();
                        if (item)
                        {
                            Holder.PickItem(item);
                            break;
                        }
                    }
                }
            }
            else
            {
                actKeyDown = false;
            }
        }

        void Move()
        {
            if (!Keyboard.IsKeyToggled(KeyCode.CapsLock))
            {
                keyDown = false;
            }

            Vector2 movement = Vector2.Zero;

            if (Keyboard.IsKeyDown(upKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Up;
                movement += Vector2.Up;
            }
            else if (Keyboard.IsKeyDown(downKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Down;
                movement += Vector2.Down;
            }
            else if (Keyboard.IsKeyDown(leftKey))
            {
                if (keyDown) return;
                keyDown = true;
                direction = Vector2Int.Left;
                movement += Vector2.Left;
            }
            else if (Keyboard.IsKeyDown(rightKey))
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
                if (Collider.Move(movement, speed * Engine.DeltaTime))
                {
                    if (lastStep != Position.Vector2Int)
                    {
                        lastStep = Position.Vector2Int;
                        PlayIfCan(walkNote);
                    }
                }
                else
                {
                    PlayIfCan(Note.Parse("C3:2"));
                }
            }
        }

        Component ComponentInFront<Component>() where Component : ComponentBase
        {
            var list = Position.system.FindPosition(Position.Vector2Int + direction);
            foreach (var pos in list)
            {
                var ent = pos.Entity.GetComponent<Component>();
                if (ent) return ent;
            }
            return null;
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
