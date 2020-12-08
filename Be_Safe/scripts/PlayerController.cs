/*
using BitsCore;
using BitsCore.Debugging;
using BitsCore.InputSystem;
using BitsCore.ObjectData.Components;
using System.Numerics;

namespace BeSafe.Scripts
{
    class PlayerController : ScriptComponent
    {
        public float width, height;

        RectCollider playerRect;
        Collision collision; //used in OnUpdate()

        public PlayerController(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public override void OnStart()
        {
            playerRect = new RectCollider(gameObject.transform.position.Z, gameObject.transform.position.Y, width, height); //z is forward/backward in this case not x
        }

        public override void OnUpdate()
        {
            Vector3 movement = Vector3.Zero;
            //gravity
            float gravity = 9.81f * GameTime.DeltaTime;
            movement += Vector3.UnitY * -gravity * 2.0f;

            float speed = 50f * GameTime.DeltaTime;
            if (Input.IsDown(KeyCode.UpArrow))
            {
                movement += Vector3.UnitY * speed * 0.75f;
            }
            if (Input.IsDown(KeyCode.RightArrow))
            {
                movement += Vector3.UnitZ * speed * 0.25f;
            }
            if (Input.IsDown(KeyCode.LeftArrow))
            {
                movement += -Vector3.UnitZ * speed * 0.25f;
            }

            Vector3 movementBefCol = movement;

            //collisions
            foreach (RectCollider rect in ((BeSafeApp)Program.app).environCollider)
            {
                playerRect.xPos = gameObject.transform.position.Z; //z is forward/backward in this case not x
                playerRect.yPos = gameObject.transform.position.Y;
                collision = RectCollider.CheckCollision(playerRect, rect);

                if (collision.xOverlap > 0f && movement.Z != 0f)
                {
                    BBug.Log("xOverlap: " + collision.xOverlap + ", Collision Dir: " + collision.direction);
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_GreenRubber"));
                
                    if(collision.direction.HasFlag(CollisionDir.Right))
                    {
                        movement = new Vector3(0f, movement.Y, movement.Z - collision.xOverlap);
                    }
                    else if (collision.direction.HasFlag(CollisionDir.Left))
                    {
                        movement = new Vector3(0f, movement.Y, movement.Z + collision.xOverlap);
                    }

                }
                else
                {
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_CelShading"));
                }
                if (collision.yOverlap > 0f && movement.Y != 0f)
                {
                    //BBug.Log("yOverlap: " + collision.yOverlap);
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_GreenRubber"));

                    //movement = new Vector3(0f, 0f, movement.Z);

                    if (collision.direction.HasFlag(CollisionDir.Bottom) && movement.Y < 0f)
                    {
                        movement = new Vector3(0f, 0f, movement.Z);
                    }
                    else if (collision.direction.HasFlag(CollisionDir.Top) && movement.Y > 0f)
                    {
                        movement = new Vector3(0f, 0f, movement.Z);
                    }
                }
                else
                {
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_CelShading"));
                }
            }

            gameObject.transform.Move(movement);
        }

    }
}
*/
