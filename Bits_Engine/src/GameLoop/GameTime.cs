namespace BitsCore
{
    public static class GameTime
    {
        /// <summary> 
        /// The time that has passed updating and rendering the last frame. 
        /// <para> Gets scaled by <see cref="TimeScale"/>. </para>
        /// </summary>
        public static float DeltaTime { get; set; }
        /// <summary> 
        /// The time that has passed updating and rendering the last frame. 
        /// <para> Doesn't get scaled by <see cref="TimeScale"/>. </para>
        /// </summary>
        public static float UnscaledDeltaTime { get; set; }
        /// <summary> The time that has passed since the game started running. </summary>
        public static float TotalElapsedSeconds { get; set; }
        /// <summary> This scales 'DeltaTime' has and therefore all transformations, etc. that are effected by it. </summary>
        public static float TimeScale { get; set; }
    }
}
