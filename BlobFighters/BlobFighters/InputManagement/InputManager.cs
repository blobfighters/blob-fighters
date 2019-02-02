using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public class InputManager
    {
        static InputManager _instance;

        /// <summary>
        /// The global Input instance.
        /// </summary>
        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InputManager();

                return _instance;
            }
        }

        /// <summary>
        /// Gets the array of plugged in GamePads.
        /// </summary>
        public GamePadState[] GamePads { get; private set; }

        /// <summary>
        /// Contains the button states of each GamePad.
        /// </summary>
        private Dictionary<Buttons, ButtonState>[] buttonStates;

        /// <summary>
        /// A delegate describing the button state event parameters.
        /// </summary>
        /// <param name="button"></param>
        public delegate void ButtonStateEvent(int playerID, Buttons button, ButtonState state);

        /// <summary>
        /// Called when a GamePad button is pressed.
        /// </summary>
        public event ButtonStateEvent OnButtonStateChanged;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private InputManager()
        {
            GamePads = new GamePadState[GamePad.MaximumGamePadCount];
            buttonStates = new Dictionary<Buttons, ButtonState>[GamePads.Length];

            for (int i = 0; i < GamePads.Length; i++)
            {
                buttonStates[i] = new Dictionary<Buttons, ButtonState>();

                foreach (Buttons b in Enum.GetValues(typeof(Buttons)))
                    buttonStates[i][b] = ButtonState.Released;
            }
        }

        /// <summary>
        /// Updates all input information.
        /// </summary>
        internal void Refresh()
        {
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                GamePadState currentState = GamePads[i] = GamePad.GetState(i);

                foreach (Buttons b in Enum.GetValues(typeof(Buttons)))
                {
                    ButtonState bs = currentState.IsButtonDown(b) ? ButtonState.Pressed : ButtonState.Released;

                    if (buttonStates[i][b] != bs)
                    {
                        buttonStates[i][b] = bs;
                        OnButtonStateChanged?.Invoke(i, b, bs);
                    }
                }
            }
        }


    }
}