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
            BBug.Log("PlayerController.OnStart() called");

            playerRect = new RectCollider(gameObject.transform.position.Z, gameObject.transform.position.Y, width, height); //z is forward/backward in this case not x
        }

        public override void OnUpdate()
        {
            Vector3 movement = Vector3.Zero;
            //gravity
            float gravity = 9.81f * GameTime.DeltaTime;
            movement += Vector3.UnitY * -gravity * 0.5f;


            float speed = 50f * GameTime.DeltaTime;
            if (Input.IsDown(KeyCode.UpArrow))
            {
                movement += Vector3.UnitY * speed * 0.2f;
            }
            if (Input.IsDown(KeyCode.RightArrow))
            {
                movement += Vector3.UnitZ * speed * 0.2f;
            }
            if (Input.IsDown(KeyCode.LeftArrow))
            {
                movement +=  -Vector3.UnitZ * speed * 0.2f;
            }

            gameObject.transform.Move(movement);

            //collisions
            foreach (RectCollider rect in ((BeSafeApp)Program.app).environCollider)
            {
                playerRect.xPos = gameObject.transform.position.Z; //z is forward/backward in this case not x
                playerRect.yPos = gameObject.transform.position.Y;
                collision = RectCollider.CheckCollision(playerRect, rect);



                if (collision.xOverlap > 0f && movement.Z != 0f)
                {
                    BBug.Log("xOverlap: " + collision.xOverlap);
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_GreenRubber"));
                    gameObject.transform.Move(new Vector3(0f, 0f, collision.xOverlap)); //z is forward/backward in this case not x
                }
                else if(collision.yOverlap > 0f && movement.Y != 0f)
                {
                    //BBug.Log("yOverlap: " + collision.yOverlap);
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_GreenRubber"));
                    gameObject.transform.Move(new Vector3(0f, collision.yOverlap, 0f)); //z is forward/backward in this case not x
                }
                else
                {
                    gameObject.GetComp<Model>().SetMaterial(AssetManager.GetMaterial("Mat_CelShading"));
                }
            }
        }

    }
}
