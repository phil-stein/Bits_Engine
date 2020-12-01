using System.Numerics;
using BitsCore;
using BitsCore.Debugging;
using BitsCore.InputSystem;
using BitsCore.ObjectData.Components;

namespace TestEnvironment.Scripts
{
    class MoveByArrowKeys : ScriptComponent
    {
        public MoveByArrowKeys()
        {
        }

        public override void OnStart()
        {
            BBug.Log("MoveByArrowKeys.OnStart() called");
        }

        public override void OnUpdate()
        {
            float speed = GameTime.DeltaTime * 50f; ;
            if (Input.IsDown(KeyCode.UpArrow))
            {
                gameObject.transform.Move(Vector3.UnitX * speed * 0.2f);
            }
            if (Input.IsDown(KeyCode.DownArrow))
            {
                gameObject.transform.Move(-Vector3.UnitX * speed * 0.2f);
            }
            if (Input.IsDown(KeyCode.RightArrow))
            {
                gameObject.transform.Move(Vector3.UnitZ * speed * 0.2f);
            }
            if (Input.IsDown(KeyCode.LeftArrow))
            {
                gameObject.transform.Move(-Vector3.UnitZ * speed * 0.2f);
            }

            //BBug.Log("MoveByArrowKeys.OnUpdate() called");
        }

        public override void OnDestroy()
        {
            BBug.Log("MoveByArrowKeys.OnDestroy() called");
        }
    }
}
