using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityFramework;

namespace UIFramework {
    public static class KeyList {
        static KeyComboList keys;

        public static void Init() {
            keys = new KeyComboList(30, 2);

            keys.AddKeyCombo(new Keys[] { Keys.A }, "A");
            keys.AddKeyCombo(new Keys[] { Keys.B }, "B");
            keys.AddKeyCombo(new Keys[] { Keys.C }, "C");
            keys.AddKeyCombo(new Keys[] { Keys.D }, "D");
            keys.AddKeyCombo(new Keys[] { Keys.E }, "E");
            keys.AddKeyCombo(new Keys[] { Keys.F }, "F");
            keys.AddKeyCombo(new Keys[] { Keys.G }, "G");
            keys.AddKeyCombo(new Keys[] { Keys.H }, "H");
            keys.AddKeyCombo(new Keys[] { Keys.I }, "I");
            keys.AddKeyCombo(new Keys[] { Keys.J }, "J");
            keys.AddKeyCombo(new Keys[] { Keys.K }, "K");
            keys.AddKeyCombo(new Keys[] { Keys.L }, "L");
            keys.AddKeyCombo(new Keys[] { Keys.M }, "M");
            keys.AddKeyCombo(new Keys[] { Keys.N }, "N");
            keys.AddKeyCombo(new Keys[] { Keys.O }, "O");
            keys.AddKeyCombo(new Keys[] { Keys.P }, "P");
            keys.AddKeyCombo(new Keys[] { Keys.Q }, "Q");
            keys.AddKeyCombo(new Keys[] { Keys.R }, "R");
            keys.AddKeyCombo(new Keys[] { Keys.S }, "S");
            keys.AddKeyCombo(new Keys[] { Keys.T }, "T");
            keys.AddKeyCombo(new Keys[] { Keys.U }, "U");
            keys.AddKeyCombo(new Keys[] { Keys.V }, "V");
            keys.AddKeyCombo(new Keys[] { Keys.W }, "W");
            keys.AddKeyCombo(new Keys[] { Keys.X }, "X");
            keys.AddKeyCombo(new Keys[] { Keys.Y }, "Y");
            keys.AddKeyCombo(new Keys[] { Keys.Z }, "Z");

            keys.AddKeyCombo(new Keys[] { Keys.D0 }, "0");
            keys.AddKeyCombo(new Keys[] { Keys.D1 }, "1");
            keys.AddKeyCombo(new Keys[] { Keys.D2 }, "2");
            keys.AddKeyCombo(new Keys[] { Keys.D3 }, "3");
            keys.AddKeyCombo(new Keys[] { Keys.D4 }, "4");
            keys.AddKeyCombo(new Keys[] { Keys.D5 }, "5");
            keys.AddKeyCombo(new Keys[] { Keys.D6 }, "6");
            keys.AddKeyCombo(new Keys[] { Keys.D7 }, "7");
            keys.AddKeyCombo(new Keys[] { Keys.D8 }, "8");
            keys.AddKeyCombo(new Keys[] { Keys.D9 }, "9");

            keys.AddKeyCombo(new Keys[] { Keys.OemTilde }, "~");
            keys.AddKeyCombo(new Keys[] { Keys.OemSemicolon }, ":");
            keys.AddKeyCombo(new Keys[] { Keys.OemQuotes }, "\"");
            keys.AddKeyCombo(new Keys[] { Keys.OemQuestion }, "?");
            keys.AddKeyCombo(new Keys[] { Keys.OemPlus }, "+");
            keys.AddKeyCombo(new Keys[] { Keys.OemPipe }, "|");
            keys.AddKeyCombo(new Keys[] { Keys.OemPeriod }, ">");
            keys.AddKeyCombo(new Keys[] { Keys.OemOpenBrackets }, "{");
            keys.AddKeyCombo(new Keys[] { Keys.OemCloseBrackets }, "}");
            keys.AddKeyCombo(new Keys[] { Keys.OemMinus }, "_");
            keys.AddKeyCombo(new Keys[] { Keys.OemComma }, "<");
            keys.AddKeyCombo(new Keys[] { Keys.Space }, " ");
        }

        public static void Update() {
            keys.Update();
        }

        /// <summary>
        /// Tries to convert keyboard input to characters and prevents repeatedly returning the 
        /// same character if a key was pressed last frame, but not yet unpressed this frame.
        /// </summary>
        /// <param name="key">When this method returns, contains the correct character if conversion succeeded.
        /// Else contains the ö character.</param>
        /// <returns>True if conversion was successful</returns>
        public static bool TryConvertKeyboardInput(out char key, bool euKeyboard) {
            var keyboard = Keyboard.GetState();
            Keys[] pressedKeys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            key = 'ö';
            char testKey = 'ö';

            if (pressedKeys.Length > 0) {
                switch (pressedKeys[0]) {
                    //Alphabet keys
                    case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } break;
                    case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } break;
                    case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } break;
                    case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } break;
                    case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } break;
                    case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } break;
                    case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } break;
                    case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } break;
                    case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } break;
                    case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } break;
                    case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } break;
                    case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } break;
                    case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } break;
                    case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } break;
                    case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } break;
                    case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } break;
                    case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } break;
                    case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } break;
                    case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } break;
                    case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } break;
                    case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } break;
                    case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } break;
                    case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } break;
                    case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } break;
                    case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } break;
                    case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } break;

                    //Decimal keys
                    case Keys.D0: if (shift && euKeyboard) { key = '='; } else if (shift) { key = ')'; } else { key = '0'; } testKey = '0'; break;
                    case Keys.D1: if (shift && euKeyboard) { key = '!'; } else if (shift) { key = '!'; } else { key = '1'; } testKey = '1'; break;
                    case Keys.D2: if (shift && euKeyboard) { key = '"'; } else if (shift) { key = '@'; } else { key = '2'; } testKey = '2'; break;
                    case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } testKey = '3'; break;
                    case Keys.D4: if (shift && euKeyboard) { key = '$'; } else if (shift) { key = '$'; } else { key = '4'; } testKey = '4'; break;
                    case Keys.D5: if (shift && euKeyboard) { key = '%'; } else if (shift) { key = '%'; } else { key = '5'; } testKey = '5'; break;
                    case Keys.D6: if (shift && euKeyboard) { key = '&'; } else if (shift) { key = '^'; } else { key = '6'; } testKey = '6'; break;
                    case Keys.D7: if (shift && euKeyboard) { key = '/'; } else if (shift) { key = '&'; } else { key = '7'; } testKey = '7'; break;
                    case Keys.D8: if (shift && euKeyboard) { key = '('; } else if (shift) { key = '*'; } else { key = '8'; } testKey = '8'; break;
                    case Keys.D9: if (shift && euKeyboard) { key = ')'; } else if (shift) { key = '('; } else { key = '9'; } testKey = '9'; break;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; break;
                    case Keys.NumPad1: key = '1'; break;
                    case Keys.NumPad2: key = '2'; break;
                    case Keys.NumPad3: key = '3'; break;
                    case Keys.NumPad4: key = '4'; break;
                    case Keys.NumPad5: key = '5'; break;
                    case Keys.NumPad6: key = '6'; break;
                    case Keys.NumPad7: key = '7'; break;
                    case Keys.NumPad8: key = '8'; break;
                    case Keys.NumPad9: key = '9'; break;

                    //Special keys
                    case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } testKey = '~'; break;
                    case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } testKey = ':'; break;
                    case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } testKey = '"'; break;
                    case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } testKey = '?'; break;
                    case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } testKey = '+'; break;
                    case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } testKey = '|'; break;
                    case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } testKey = '>'; break;
                    case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } testKey = '{'; break;
                    case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } testKey = '}'; break;
                    case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } testKey = '_'; break;
                    case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } testKey = '<'; break;
                    case Keys.Space: key = ' '; break;
                }//switch end

                if (testKey != 'ö' && keys.CheckKeyComboStateWithPriorities(testKey.ToString()))
                    return true;
                else
                    if (key != 'ö' && keys.CheckKeyComboStateWithPriorities(key.ToString().ToUpper()))
                    return true;


            }//if end

            return false;
        }
    }
}