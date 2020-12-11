using BitsCore;
using BitsCore.Debugging;
using BitsCore.ObjectData.Components;
using System.Numerics;

namespace BeSafe.Scripts
{
    public class SelecRingBehaviour : ScriptComponent
    {

        BeSafeApp app;
        public override void OnStart()
        {
            app = ((BeSafeApp)Program.app);
        }

        public override void OnUpdate()
        {
            app.GetCurTilePos(out int xPos, out int zPos);
            gameObject.transform.position = new Vector3(xPos, 0f, zPos);
        }

    }
}
