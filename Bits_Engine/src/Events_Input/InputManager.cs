using BitsCore.Debugging;
using BitsCore.Events_Input;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BitsCore.InputSystem
{
    internal static class InputManager
    {
        #region INPUT_STATES
        /// <summary> 
        /// MouseButton state last frame. 
        /// <para> true: was pressed last frame, false: wasn't pressed last frame </para>
        /// </summary>
        public static bool mouseButton1, mouseButton2, mouseButton3, mouseButton4, mouseButton5, mouseButton6, mouseButton7, mouseButton8;

        /// <summary> 
        /// Key state last frame. 
        /// <para> true: was pressed last frame, false: wasn't pressed last frame </para>
        /// </summary>
        public static bool
            Space_st, Apostrophe_st, Comma_st, Minus_st, Period_st, Slash_st,
            Alpha0_st, Alpha1_st, Alpha2_st, Alpha3_st, Alpha4_st, Alpha5_st, Alpha6_st, Alpha7_st, Alpha8_st, Alpha9_st,
            SemiColon_st, Equal_st,
            A_st, B_st, C_st, D_st, E_st, F_st, G_st, H_st, I_st, J_st, K_st, L_st, M_st,
            N_st, O_st, P_st, Q_st, R_st, S_st, T_st, U_st, V_st, W_st, X_st, Y_st, Z_st,
            LeftBracket_st, Backslash_st, RightBracket_st, GraveAccent_st, World1_st,
            World2_st, Escape_st, Enter_st, Tab_st, Backspace_st, Insert_st, Delete_st,
            RightArrow_st, LeftArrow_st, DownArrow_st, UpArrow_st,
            PageUp_st, PageDown_st, Home_st, End_st, CapsLock_st, ScrollLock_st, NumLock_st, PrintScreen_st, Pause_st,
            F1_st, F2_st, F3_st, F4_st, F5_st, F6_st, F7_st, F8_st, F9_st, F10_st, F11_st, F12_st, F13_st, F14_st, F15_st,
            F16_st, F17_st, F18_st, F19_st, F20_st, F21_st, F22_st, F23_st, F24_st, F25_st,
            Numpad0_st, Numpad1_st, Numpad2_st, Numpad3_st, Numpad4_st, Numpad5_st, Numpad6_st, Numpad7_st, Numpad8_st, Numpad9_st,
            NumpadDecimal_st, NumpadDivide_st, NumpadMultiply_st, NumpadSubtract_st, NumpadAdd_st, NumpadEnter_st, NumpadEqual_st,
            LeftShift_st, LeftControl_st, LeftAlt_st, LeftSuper_st, RightShift_st, RightControl_st, RightAlt_st, RightSuper_st, Menu_st,
            LeftWinMacSymbol_st, RightWinMacSymbol_st;
        #endregion

        /// <summary> 
        /// Initialize the InputManager. 
        /// <para> Needs to be called before using Input.IsPressed() or Input.IsMousePressed(). </para>
        /// </summary>
        public static void Init()
        {
            //currently using Glfw, because we don't any other libraries yet
            EventManager.Input_Key += KeyCallback;
            EventManager.Input_Mouse += MouseCallback;
            EventManager.Input_MouseMove += MouseMoveCallback;
            EventManager.Input_MouseScroll += MouseScrollCallback;
        }

        /// <summary> Resets the mouse/key states from last frame before gathering them again for the next frame. </summary>
        public static void ResetLastFrameKeyButtonStates()
        {
            MouseStateReset();
            KeyStateReset();
        }

        /// <summary> Sets the key states for the last frame. </summary>
        private static void KeyCallback(KeyCode key, InputState state)
        {
            if (state == InputState.Press)
            {
                //mapping glfw keys to our keycodes
                if (key == KeyCode.Unknown) { return; }
                else if (key == KeyCode.Space) { Space_st = true; }
                else if (key == KeyCode.Apostrophe) { Apostrophe_st = true; }
                else if (key == KeyCode.Comma) { Comma_st = true; }
                else if (key == KeyCode.Minus) { Minus_st = true; }
                else if (key == KeyCode.Period) { Period_st = true; }
                else if (key == KeyCode.Slash) { Slash_st = true; }
                else if (key == KeyCode.Alpha0) { Alpha0_st = true; }
                else if (key == KeyCode.Alpha1) { Alpha1_st = true; }
                else if (key == KeyCode.Alpha2) { Alpha2_st = true; }
                else if (key == KeyCode.Alpha3) { Alpha3_st = true; }
                else if (key == KeyCode.Alpha4) { Alpha4_st = true; }
                else if (key == KeyCode.Alpha5) { Alpha5_st = true; }
                else if (key == KeyCode.Alpha6) { Alpha6_st = true; }
                else if (key == KeyCode.Alpha7) { Alpha7_st = true; }
                else if (key == KeyCode.Alpha8) { Alpha8_st = true; }
                else if (key == KeyCode.Alpha9) { Alpha9_st = true; }
                else if (key == KeyCode.SemiColon) { SemiColon_st = true; }
                else if (key == KeyCode.Equal) { Equal_st = true; }
                else if (key == KeyCode.A) { A_st = true; }
                else if (key == KeyCode.B) { B_st = true; }
                else if (key == KeyCode.C) { C_st = true; }
                else if (key == KeyCode.D) { D_st = true; }
                else if (key == KeyCode.E) { E_st = true; }
                else if (key == KeyCode.F) { F_st = true; }
                else if (key == KeyCode.G) { G_st = true; }
                else if (key == KeyCode.H) { H_st = true; }
                else if (key == KeyCode.I) { I_st = true; }
                else if (key == KeyCode.J) { J_st = true; }
                else if (key == KeyCode.K) { K_st = true; }
                else if (key == KeyCode.L) { L_st = true; }
                else if (key == KeyCode.M) { M_st = true; }
                else if (key == KeyCode.N) { N_st = true; }
                else if (key == KeyCode.O) { O_st = true; }
                else if (key == KeyCode.P) { P_st = true; }
                else if (key == KeyCode.Q) { Q_st = true; }
                else if (key == KeyCode.R) { R_st = true; }
                else if (key == KeyCode.S) { S_st = true; }
                else if (key == KeyCode.T) { T_st = true; }
                else if (key == KeyCode.U) { U_st = true; }
                else if (key == KeyCode.V) { V_st = true; }
                else if (key == KeyCode.W) { W_st = true; }
                else if (key == KeyCode.X) { X_st = true; }
                else if (key == KeyCode.Y) { Y_st = true; }
                else if (key == KeyCode.Z) { Z_st = true; }
                else if (key == KeyCode.LeftBracket) { LeftBracket_st = true; }
                else if (key == KeyCode.Backslash) { Backslash_st = true; }
                else if (key == KeyCode.RightBracket) { RightBracket_st = true; }
                else if (key == KeyCode.GraveAccent) { GraveAccent_st = true; }
                else if (key == KeyCode.World1) { World1_st = true; }
                else if (key == KeyCode.World2) { World1_st = true; }
                else if (key == KeyCode.Escape) { Escape_st = true; }
                else if (key == KeyCode.Enter) { Enter_st = true; }
                else if (key == KeyCode.Tab) { Tab_st = true; }
                else if (key == KeyCode.Backspace) { Backspace_st = true; }
                else if (key == KeyCode.Insert) { Insert_st = true; }
                else if (key == KeyCode.Delete) { Delete_st = true; }
                else if (key == KeyCode.RightArrow) { RightArrow_st = true; }
                else if (key == KeyCode.LeftArrow) { LeftArrow_st = true; }
                else if (key == KeyCode.DownArrow) { DownArrow_st = true; }
                else if (key == KeyCode.UpArrow) { UpArrow_st = true; }
                else if (key == KeyCode.PageUp) { PageUp_st = true; }
                else if (key == KeyCode.PageDown) { PageDown_st = true; }
                else if (key == KeyCode.Home) { Home_st = true; }
                else if (key == KeyCode.End) { End_st = true; }
                else if (key == KeyCode.CapsLock) { CapsLock_st = true; }
                else if (key == KeyCode.ScrollLock) { ScrollLock_st = true; }
                else if (key == KeyCode.NumLock) { NumLock_st = true; }
                else if (key == KeyCode.PrintScreen) { PrintScreen_st = true; }
                else if (key == KeyCode.Pause) { Pause_st = true; }
                else if (key == KeyCode.F1) { F1_st = true; }
                else if (key == KeyCode.F2) { F2_st = true; }
                else if (key == KeyCode.F3) { F3_st = true; }
                else if (key == KeyCode.F4) { F4_st = true; }
                else if (key == KeyCode.F5) { F5_st = true; }
                else if (key == KeyCode.F6) { F6_st = true; }
                else if (key == KeyCode.F7) { F7_st = true; }
                else if (key == KeyCode.F8) { F8_st = true; }
                else if (key == KeyCode.F9) { F9_st = true; }
                else if (key == KeyCode.F10) { F10_st = true; }
                else if (key == KeyCode.F11) { F11_st = true; }
                else if (key == KeyCode.F12) { F12_st = true; }
                else if (key == KeyCode.F13) { F13_st = true; }
                else if (key == KeyCode.F14) { F14_st = true; }
                else if (key == KeyCode.F15) { F15_st = true; }
                else if (key == KeyCode.F16) { F16_st = true; }
                else if (key == KeyCode.F17) { F17_st = true; }
                else if (key == KeyCode.F18) { F18_st = true; }
                else if (key == KeyCode.F19) { F19_st = true; }
                else if (key == KeyCode.F20) { F20_st = true; }
                else if (key == KeyCode.F21) { F21_st = true; }
                else if (key == KeyCode.F22) { F22_st = true; }
                else if (key == KeyCode.F23) { F23_st = true; }
                else if (key == KeyCode.F24) { F24_st = true; }
                else if (key == KeyCode.F25) { F25_st = true; }
                else if (key == KeyCode.Numpad0) { Numpad0_st = true; }
                else if (key == KeyCode.Numpad1) { Numpad1_st = true; }
                else if (key == KeyCode.Numpad2) { Numpad2_st = true; }
                else if (key == KeyCode.Numpad3) { Numpad3_st = true; }
                else if (key == KeyCode.Numpad4) { Numpad4_st = true; }
                else if (key == KeyCode.Numpad5) { Numpad5_st = true; }
                else if (key == KeyCode.Numpad6) { Numpad6_st = true; }
                else if (key == KeyCode.Numpad7) { Numpad7_st = true; }
                else if (key == KeyCode.Numpad8) { Numpad8_st = true; }
                else if (key == KeyCode.Numpad9) { Numpad9_st = true; }
                else if (key == KeyCode.NumpadDecimal) { NumpadDecimal_st = true; }
                else if (key == KeyCode.NumpadDivide) { NumpadDivide_st = true; }
                else if (key == KeyCode.NumpadMultiply) { NumpadMultiply_st = true; }
                else if (key == KeyCode.NumpadSubtract) { NumpadSubtract_st = true; }
                else if (key == KeyCode.NumpadAdd) { NumpadAdd_st = true; }
                else if (key == KeyCode.NumpadEnter) { NumpadEnter_st = true; }
                else if (key == KeyCode.NumpadEqual) { NumpadEqual_st = true; }
                else if (key == KeyCode.LeftShift) { LeftShift_st = true; }
                else if (key == KeyCode.LeftControl) { LeftControl_st = true; }
                else if (key == KeyCode.LeftAlt) { LeftAlt_st = true; }
                else if (key == KeyCode.LeftSuper || key == KeyCode.LeftWinMacSymbol) { LeftSuper_st = true; LeftWinMacSymbol_st = true; }
                else if (key == KeyCode.RightSuper || key == KeyCode.RightWinMacSymbol) { RightSuper_st = true; RightWinMacSymbol_st = true; }
                else if (key == KeyCode.RightShift) { RightShift_st = true; }
                else if (key == KeyCode.RightControl) { RightControl_st = true; }
                else if (key == KeyCode.RightAlt) { RightAlt_st = true; }
                else if (key == KeyCode.Menu) { Menu_st = true; }

                //additional keycodes
                else if (key == KeyCode.LeftWinMacSymbol) { LeftWinMacSymbol_st = true; LeftSuper_st = true; }
                else if (key == KeyCode.RightWinMacSymbol) { RightWinMacSymbol_st = true; RightSuper_st = true; }
            }
        }

        /// <summary> Sets the mouse-button states for the last frame. </summary>
        private static void MouseCallback(MouseButton btn, InputState state)
        {
            if (state == InputState.Press)
            {
                if (btn == MouseButton.Button1) { mouseButton1 = true; }
                if (btn == MouseButton.Button2) { mouseButton2 = true; }
                if (btn == MouseButton.Button3) { mouseButton3 = true; }
                if (btn == MouseButton.Button4) { mouseButton4 = true; }
                if (btn == MouseButton.Button5) { mouseButton5 = true; }
                if (btn == MouseButton.Button6) { mouseButton6 = true; }
                if (btn == MouseButton.Button7) { mouseButton7 = true; }
                if (btn == MouseButton.Button8) { mouseButton8 = true; }

                else if (btn == MouseButton.Left) { mouseButton1 = true; }
                else if (btn == MouseButton.Right) { mouseButton2 = true; }
                else if (btn == MouseButton.Middle) { mouseButton3 = true; }
            }
        }

        /// <summary> 
        /// Gets the state of the MouseButton last frame. 
        /// <para> true: was pressed last frame, false: wasn't pressed last frame </para>
        /// </summary>
        /// <param name="btn"> The MouseButton to check. </param>
        public static bool GetMouseButtonState(MouseButton btn)
        {
            if (btn == MouseButton.Button1) { return mouseButton1; }
            else if (btn == MouseButton.Button2) { return mouseButton2; }
            else if (btn == MouseButton.Button3) { return mouseButton3; }
            else if (btn == MouseButton.Button4) { return mouseButton4; }
            else if (btn == MouseButton.Button5) { return mouseButton5; }
            else if (btn == MouseButton.Button6) { return mouseButton6; }
            else if (btn == MouseButton.Button7) { return mouseButton7; }
            else if (btn == MouseButton.Button8) { return mouseButton8; }

            else if (btn == MouseButton.Left) { return mouseButton1; }
            else if (btn == MouseButton.Right) { return mouseButton2; }
            else if (btn == MouseButton.Middle) { return mouseButton3; }

            return false;
        }
        /// <summary> 
        /// Gets the state of the key last frame. 
        /// <para> true: was pressed last frame, false: wasn't pressed last frame </para>
        /// </summary>
        /// <param name="key"> The key to check. </param>
        public static bool GetKeyState(KeyCode key)
        {
            if (key == KeyCode.Unknown) { return false; }
            else if (key == KeyCode.Space) { return Space_st; }
            else if (key == KeyCode.Apostrophe) { return Apostrophe_st; }
            else if (key == KeyCode.Comma) { return Comma_st; }
            else if (key == KeyCode.Minus) { return Minus_st; }
            else if (key == KeyCode.Period) { return Period_st; }
            else if (key == KeyCode.Slash) { return Slash_st; }
            else if (key == KeyCode.Alpha0) { return Alpha0_st; }
            else if (key == KeyCode.Alpha1) { return Alpha1_st; }
            else if (key == KeyCode.Alpha2) { return Alpha2_st; }
            else if (key == KeyCode.Alpha3) { return Alpha3_st; }
            else if (key == KeyCode.Alpha4) { return Alpha4_st; }
            else if (key == KeyCode.Alpha5) { return Alpha5_st; }
            else if (key == KeyCode.Alpha6) { return Alpha6_st; }
            else if (key == KeyCode.Alpha7) { return Alpha7_st; }
            else if (key == KeyCode.Alpha8) { return Alpha8_st; }
            else if (key == KeyCode.Alpha9) { return Alpha9_st; }
            else if (key == KeyCode.SemiColon) { return SemiColon_st; }
            else if (key == KeyCode.Equal) { return Equal_st; }
            else if (key == KeyCode.A) { return A_st; }
            else if (key == KeyCode.B) { return B_st; }
            else if (key == KeyCode.C) { return C_st; }
            else if (key == KeyCode.D) { return D_st; }
            else if (key == KeyCode.E) { return E_st; }
            else if (key == KeyCode.F) { return F_st; }
            else if (key == KeyCode.G) { return G_st; }
            else if (key == KeyCode.H) { return H_st; }
            else if (key == KeyCode.I) { return I_st; }
            else if (key == KeyCode.J) { return J_st; }
            else if (key == KeyCode.K) { return K_st; }
            else if (key == KeyCode.L) { return L_st; }
            else if (key == KeyCode.M) { return M_st; }
            else if (key == KeyCode.N) { return N_st; }
            else if (key == KeyCode.O) { return O_st; }
            else if (key == KeyCode.P) { return P_st; }
            else if (key == KeyCode.Q) { return Q_st; }
            else if (key == KeyCode.R) { return R_st; }
            else if (key == KeyCode.S) { return S_st; }
            else if (key == KeyCode.T) { return T_st; }
            else if (key == KeyCode.U) { return U_st; }
            else if (key == KeyCode.V) { return V_st; }
            else if (key == KeyCode.W) { return W_st; }
            else if (key == KeyCode.X) { return X_st; }
            else if (key == KeyCode.Y) { return Y_st; }
            else if (key == KeyCode.Z) { return Z_st; }
            else if (key == KeyCode.LeftBracket) { return LeftBracket_st; }
            else if (key == KeyCode.Backslash) { return Backslash_st; }
            else if (key == KeyCode.RightBracket) { return RightBracket_st; }
            else if (key == KeyCode.GraveAccent) { return GraveAccent_st; }
            else if (key == KeyCode.World1) { return World1_st; }
            else if (key == KeyCode.World2) { return World1_st; }
            else if (key == KeyCode.Escape) { return Escape_st; }
            else if (key == KeyCode.Enter) { return Enter_st; }
            else if (key == KeyCode.Tab) { return Tab_st; }
            else if (key == KeyCode.Backspace) { return Backspace_st; }
            else if (key == KeyCode.Insert) { return Insert_st; }
            else if (key == KeyCode.Delete) { return Delete_st; }
            else if (key == KeyCode.RightArrow) { return RightArrow_st; }
            else if (key == KeyCode.LeftArrow) { return LeftArrow_st; }
            else if (key == KeyCode.DownArrow) { return DownArrow_st; }
            else if (key == KeyCode.UpArrow) { return UpArrow_st; }
            else if (key == KeyCode.PageUp) { return PageUp_st; }
            else if (key == KeyCode.PageDown) { return PageDown_st; }
            else if (key == KeyCode.Home) { return Home_st; }
            else if (key == KeyCode.End) { return End_st; }
            else if (key == KeyCode.CapsLock) { return CapsLock_st; }
            else if (key == KeyCode.ScrollLock) { return ScrollLock_st; }
            else if (key == KeyCode.NumLock) { return NumLock_st; }
            else if (key == KeyCode.PrintScreen) { return PrintScreen_st; }
            else if (key == KeyCode.Pause) { return Pause_st; }
            else if (key == KeyCode.F1) { return F1_st; }
            else if (key == KeyCode.F2) { return F2_st; }
            else if (key == KeyCode.F3) { return F3_st; }
            else if (key == KeyCode.F4) { return F4_st; }
            else if (key == KeyCode.F5) { return F5_st; }
            else if (key == KeyCode.F6) { return F6_st; }
            else if (key == KeyCode.F7) { return F7_st; }
            else if (key == KeyCode.F8) { return F8_st; }
            else if (key == KeyCode.F9) { return F9_st; }
            else if (key == KeyCode.F10) { return F10_st; }
            else if (key == KeyCode.F11) { return F11_st; }
            else if (key == KeyCode.F12) { return F12_st; }
            else if (key == KeyCode.F13) { return F13_st; }
            else if (key == KeyCode.F14) { return F14_st; }
            else if (key == KeyCode.F15) { return F15_st; }
            else if (key == KeyCode.F16) { return F16_st; }
            else if (key == KeyCode.F17) { return F17_st; }
            else if (key == KeyCode.F18) { return F18_st; }
            else if (key == KeyCode.F19) { return F19_st; }
            else if (key == KeyCode.F20) { return F20_st; }
            else if (key == KeyCode.F21) { return F21_st; }
            else if (key == KeyCode.F22) { return F22_st; }
            else if (key == KeyCode.F23) { return F23_st; }
            else if (key == KeyCode.F24) { return F24_st; }
            else if (key == KeyCode.F25) { return F25_st; }
            else if (key == KeyCode.Numpad0) { return Numpad0_st; }
            else if (key == KeyCode.Numpad1) { return Numpad1_st; }
            else if (key == KeyCode.Numpad2) { return Numpad2_st; }
            else if (key == KeyCode.Numpad3) { return Numpad3_st; }
            else if (key == KeyCode.Numpad4) { return Numpad4_st; }
            else if (key == KeyCode.Numpad5) { return Numpad5_st; }
            else if (key == KeyCode.Numpad6) { return Numpad6_st; }
            else if (key == KeyCode.Numpad7) { return Numpad7_st; }
            else if (key == KeyCode.Numpad8) { return Numpad8_st; }
            else if (key == KeyCode.Numpad9) { return Numpad9_st; }
            else if (key == KeyCode.NumpadDecimal) { return NumpadDecimal_st; }
            else if (key == KeyCode.NumpadDivide) { return NumpadDivide_st; }
            else if (key == KeyCode.NumpadMultiply) { return NumpadMultiply_st; }
            else if (key == KeyCode.NumpadSubtract) { return NumpadSubtract_st; }
            else if (key == KeyCode.NumpadAdd) { return NumpadAdd_st; }
            else if (key == KeyCode.NumpadEnter) { return NumpadEnter_st; }
            else if (key == KeyCode.NumpadEqual) { return NumpadEqual_st; }
            else if (key == KeyCode.LeftShift) { return LeftShift_st; }
            else if (key == KeyCode.LeftControl) { return LeftControl_st; }
            else if (key == KeyCode.LeftAlt) { return LeftAlt_st; }
            else if (key == KeyCode.LeftSuper || key == KeyCode.LeftWinMacSymbol) { return LeftSuper_st; } //same as returning LeftWinMacSymbol_st
            else if (key == KeyCode.RightSuper || key == KeyCode.RightWinMacSymbol) { return RightSuper_st; } //same as returning RightWinMacSymbol_st
            else if (key == KeyCode.RightShift) { return RightShift_st; }
            else if (key == KeyCode.RightControl) { return RightControl_st; }
            else if (key == KeyCode.RightAlt) { return RightAlt_st; }
            else if (key == KeyCode.Menu) { return Menu_st; }

            return false;
        }

        /// <summary> Resets the mouse-button states from last frame. </summary>
        private static void MouseStateReset()
        {
            //bulk assigns the booleans
            mouseButton1 = mouseButton2 = mouseButton3 = mouseButton4 = mouseButton5 = mouseButton6 = mouseButton7 = mouseButton8 = false;
        }
        /// <summary> Resets the key states from last frame. </summary>
        private static void KeyStateReset()
        {
            //bulk assigns the booleans
            Space_st = Apostrophe_st = Comma_st = Minus_st = Period_st = Slash_st =
            Alpha0_st = Alpha1_st = Alpha2_st = Alpha3_st = Alpha4_st = Alpha5_st = Alpha6_st = Alpha7_st = Alpha8_st = Alpha9_st =
            SemiColon_st = Equal_st =
            A_st = B_st = C_st = D_st = E_st = F_st = G_st = H_st = I_st = J_st = K_st = L_st = M_st =
            N_st = O_st = P_st = Q_st = R_st = S_st = T_st = U_st = V_st = W_st = X_st = Y_st = Z_st =
            LeftBracket_st = Backslash_st = RightBracket_st = GraveAccent_st = World1_st =
            World2_st = Escape_st = Enter_st = Tab_st = Backspace_st = Insert_st = Delete_st =
            RightArrow_st = LeftArrow_st = DownArrow_st = UpArrow_st =
            PageUp_st = PageDown_st = Home_st = End_st = CapsLock_st = ScrollLock_st = NumLock_st = PrintScreen_st = Pause_st =
            F1_st = F2_st = F3_st = F4_st = F5_st = F6_st = F7_st = F8_st = F9_st = F10_st = F11_st = F12_st = F13_st = F14_st = F15_st =
            F16_st = F17_st = F18_st = F19_st = F20_st = F21_st = F22_st = F23_st = F24_st = F25_st =
            Numpad0_st = Numpad1_st = Numpad2_st = Numpad3_st = Numpad4_st = Numpad5_st = Numpad6_st = Numpad7_st = Numpad8_st = Numpad9_st =
            NumpadDecimal_st = NumpadDivide_st = NumpadMultiply_st = NumpadSubtract_st = NumpadAdd_st = NumpadEnter_st = NumpadEqual_st =
            LeftShift_st = LeftControl_st = LeftAlt_st = LeftSuper_st = RightShift_st = RightControl_st = RightAlt_st = RightSuper_st = Menu_st =
            LeftWinMacSymbol_st = RightWinMacSymbol_st = false;
        }

        /// <summary> Sets the <see cref="Input.mouseDelta"/> variable. </summary>
        /// <param name="x"> Mouse delta position x. </param>
        /// <param name="y"> Mouse delta position y.  </param>
        private static void MouseMoveCallback(double x, double y)
        {
            Input.mouseDelta = new System.Numerics.Vector2((float)x, (float)y);
        }
        /// <summary> Sets the <see cref="Input.scrollDelta"/> variable. </summary>
        /// <param name="x"> Scroll delta position x. </param>
        /// <param name="y"> Scroll delta position y.  </param>
        private static void MouseScrollCallback(double x, double y)
        {
            Input.scrollDelta = (float)y; //vertical scrolling
            BBug.Log(Input.scrollDelta);
        }
    }
}
