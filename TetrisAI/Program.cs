using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TetrisAI
{
    class Program
    {

        static void Main(string[] args)
        {
            int mostLines = 0;
            int tmpLines = 0;
            float cFullLines;
            float cHolesMade;
            float cHeightSum;
            float cBumpiness;
            float cHighest;
            float cCohesion;
            float cFullLines_best;
            float cHolesMade_best;
            float cHeightSum_best;
            float cBumpiness_best;
            float cHighest_best;
            float cCohesion_best;
            while (true)
            {
                var solver = new TetrisSolver();
                Random RNG = new Random();
                //score = cohesion / 2 - holesMade * 5 - highest + fullLines * 4;
                cFullLines = 4; //76
                cHolesMade = -10;
                cHeightSum = 0;
                cBumpiness = 0;
                cHighest = -1;
                cCohesion = 1;
                tmpLines = solver.Run(cFullLines, cHolesMade, cHeightSum, cBumpiness, cHighest, cCohesion);
                Console.Out.Write(tmpLines + ",");
                Console.ReadKey();
                //Console.Out.WriteLine("New best! Cleared " + tmpLines + " lines.");
                //Console.Out.WriteLine("cFullLines: " + cFullLines);
                //Console.Out.WriteLine("cHolesMade: " + cHolesMade);
                //Console.Out.WriteLine("cHeightSum: " + cHeightSum);
                //Console.Out.WriteLine("cBumpiness: " + cBumpiness);
                //Console.Out.WriteLine("cHighest: " + cHighest);
                //Console.Out.WriteLine("cCohesion: " + cCohesion);
                //mostLines = tmpLines;
                //cFullLines_best = cFullLines;
                //cHolesMade_best = cHolesMade;
                //cHeightSum_best = cHeightSum;
                //cBumpiness_best = cBumpiness;
                //cHighest_best = cHighest;
                //cCohesion_best = cCohesion;


            }
        }
    }

    class TetrisSolver
    {

        //A Tetris piece
        public struct Piece
        {
            public string Name { get; set; }
            public int Orientation { get; set; }
            public int Width { get; set; }
            public int[] yOffset { get; set; }
            public Piece(string name, int orientations, int width)
            {
                Name = name;
                Orientation = orientations;
                Width = width;
                yOffset = new int[2];
            }

        }
        //A move that consists of a piece, a position and an orientation
        public struct Move
        {
            public int Index { get; set; }
            public int Orientation { get; set; }
            public double Score { get; set; }
            public String Piece { get; set; }
            public Move(int index, int orientation)
            {
                Index = index;
                Orientation = orientation;
                Score = 0;
                Piece = "";
            }
        }

        public int Run(float cFullLines, float cHolesMade, float cHeightSum, float cBumpiness, float cHighest, float cCohesion)
        {

            Piece O = new Piece("O", 1, 2);
            Piece I0 = new Piece("I0", 0, 4);
            Piece I1 = new Piece("I1", 1, 1);
            Piece S0 = new Piece("S0", 0, 3);
            Piece S1 = new Piece("S1", 1, 2);
            Piece Z0 = new Piece("Z0", 0, 3);
            Piece Z1 = new Piece("Z1", 1, 2);
            Piece L0 = new Piece("L0", 0, 3);
            Piece L1 = new Piece("L1", 1, 2);
            Piece L2 = new Piece("L2", 2, 3);
            Piece L3 = new Piece("L3", 3, 2);
            Piece J0 = new Piece("J0", 0, 3);
            Piece J1 = new Piece("J1", 1, 2);
            Piece J2 = new Piece("J2", 2, 3);
            Piece J3 = new Piece("J3", 3, 2);
            Piece T0 = new Piece("T0", 0, 3);
            Piece T1 = new Piece("T1", 1, 2);
            Piece T2 = new Piece("T2", 2, 3);
            Piece T3 = new Piece("T3", 3, 2);
            S0.yOffset[0] = 0;
            S0.yOffset[1] = -1;
            S1.yOffset[0] = 1;
            Z0.yOffset[0] = 1;
            Z0.yOffset[1] = 1;
            Z1.yOffset[0] = -1;
            L2.yOffset[0] = -1;
            L2.yOffset[1] = -1;
            L3.yOffset[0] = 2;
            J1.yOffset[0] = -2;
            J2.yOffset[0] = 0;
            J2.yOffset[1] = 1;
            T1.yOffset[0] = -1;
            T2.yOffset[0] = 1;
            T2.yOffset[1] = 0;
            T3.yOffset[0] = 1;
            Piece[] pieces = new Piece[] { O, I0, I1, S0, S1, Z0, Z1, L0, L1, L2, L3, J0, J1, J2, J3, T0, T1, T2, T3 };
            Piece[] iPieces = new Piece[] { I0, I1 };
            Piece[] sPieces = new Piece[] { S0, S1 };
            Piece[] zPieces = new Piece[] { Z0, Z1 };
            Piece[] lPieces = new Piece[] { L0, L1, L2, L3 };
            Piece[] jPieces = new Piece[] { J0, J1, J2, J3 };
            Piece[] tPieces = new Piece[] { T0, T1, T2, T3 };
            Piece[] oPiece = new Piece[] { O };
            Piece[][] typesArrays = new Piece[7][];
            typesArrays[0] = new Piece[2];
            typesArrays[1] = new Piece[2];
            typesArrays[2] = new Piece[2];
            typesArrays[3] = new Piece[4];
            typesArrays[4] = new Piece[4];
            typesArrays[5] = new Piece[4];
            typesArrays[6] = new Piece[1];
            typesArrays[0] = iPieces;
            typesArrays[1] = sPieces;
            typesArrays[2] = zPieces;
            typesArrays[3] = lPieces;
            typesArrays[4] = jPieces;
            typesArrays[5] = tPieces;
            typesArrays[6] = oPiece;


            Dictionary<string, string> pieceColorDict = new Dictionary<string, string>();
            pieceColorDict.Add("teal", "I0");
            pieceColorDict.Add("green", "S0");
            pieceColorDict.Add("red", "Z0");
            pieceColorDict.Add("orange", "L0");
            pieceColorDict.Add("blue", "J0");
            pieceColorDict.Add("pink", "T0");
            pieceColorDict.Add("yellow", "O");

            Boolean keepLooping = true;
            Boolean rowCleared = false;
            String nextPieceName;
            String nextPieceColor;
            String nextNextPieceName;
            String nextNextPieceColor;
            Piece nextPiece = new Piece(" ", 0, 0);
            Piece nextNextPiece = new Piece(" ", 0, 0);
            Piece bestPiece = new Piece(" ", 0, 0);
            Piece[] pieceArray = new Piece[4];
            Piece[] nextPieceArray = new Piece[4];
            bool[,] board = new bool[20, 10];
            bool[,] tmpBoard = new bool[20, 10];
            bool[,] tmpBoard2 = new bool[20, 10];
            Move bestMove = new Move(0, 0);
            bestMove.Score = -100;
            Move tmpMove = new Move(0, 0);
            Move tmpMove2 = new Move(0, 0);
            int positions;
            int position = 0;
            int _positions;
            int _position = 0;
            int rowsCleared = 0;
            int cohesion = 0;
            int holesMade = 0;
            bool firstPiece = true;
            nextPieceColor = getNextPieceColor(true);
            nextNextPieceColor = getNextPieceColor(false);
            Console.WriteLine("Press any key");
            Console.ReadKey();
            IntPtr chromeWindow = NativeMethods.FindWindow("Chrome_WidgetWin_1", null);
            IntPtr chrome = NativeMethods.GetWindow(chromeWindow, NativeMethods.GetWindow_Cmd.GW_HWNDNEXT);
            NativeMethods.SetForegroundWindow(chrome);
            LeftMouseClick(400, 300);
            while (keepLooping)
            {

                if (firstPiece)
                {
                    firstPiece = false;
                    nextPieceColor = getNextPieceColor(true);
                    nextNextPieceColor = getNextPieceColor(false);
                }

                nextPieceName = pieceColorDict[nextPieceColor];
                nextNextPieceName = pieceColorDict[nextNextPieceColor];
                Console.Out.WriteLine("Next is " + nextNextPieceColor);
                //Loop all pieces and find by name
                foreach (Piece piece in pieces)
                {
                    if (piece.Name == nextPieceName)
                    {
                        nextPiece = piece;
                    }
                    if (piece.Name == nextNextPieceName)
                    {
                        nextNextPiece = piece;
                    }
                }
                //Find array for all possible orientations of current piece
                for (int i = 0; i < typesArrays.Length; i++)
                {
                    if (Array.Exists(typesArrays[i], element => element.Name == nextPiece.Name))
                    {
                        pieceArray = typesArrays[i];
                    }
                    if (Array.Exists(typesArrays[i], element => element.Name == nextNextPiece.Name))
                    {
                        nextPieceArray = typesArrays[i];
                    }
                }
                //Test piece in all positions and in all orientations
                bestMove.Score = -100000;
                foreach (Piece piece in pieceArray)
                {
                    positions = 11 - piece.Width;

                    for (position = 0; position < positions; position++)
                    {
                        //Console.WriteLine(piece.Name);
                        //Reset temporary board
                        Array.Copy(board, 0, tmpBoard, 0, board.Length);
                        //Fit piece on temporary board for evaluation
                        tmpBoard = addPiece(board, piece, position, out cohesion, out holesMade);
                        //Score for first move
                        tmpMove.Score = evaluateBoard(tmpBoard, cFullLines, cHolesMade, cHeightSum, cBumpiness, cHighest, cCohesion, cohesion, holesMade);
                        tmpBoard = removeFullLines(tmpBoard);
                        foreach (Piece _piece in nextPieceArray)
                        {
                            _positions = 11 - _piece.Width;
                            for (_position = 0; _position < _positions; _position++)
                            {
                                Array.Copy(tmpBoard, 0, tmpBoard2, 0, board.Length);
                                tmpBoard2 = addPiece(tmpBoard, _piece, _position, out cohesion, out holesMade);
                                //Evaluate temporary board

                                tmpMove2.Score = evaluateBoard(tmpBoard2, cFullLines, cHolesMade, cHeightSum, cBumpiness, cHighest, cCohesion, cohesion, holesMade);
                                tmpMove2.Score += tmpMove.Score;
                                if (tmpMove2.Index == 9)
                                {
                                    tmpMove2.Score -= 1;
                                }
                                if (tmpMove2.Score < -9999999)
                                {
                                    Console.WriteLine("Warning!");
                                }
                                //If highest score so far, save move
                                if (tmpMove2.Score > bestMove.Score)
                                {
                                    bestMove.Index = position;
                                    bestMove.Piece = piece.Name;
                                    bestMove.Score = tmpMove2.Score;
                                    bestPiece = piece;
                                }
                            }
                        }
                    }
                }
                //Returns board with added piece at given position
                board = addPiece(board, bestPiece, bestMove.Index, out cohesion, out holesMade);
                //Removes full lines
                rowsCleared += getFullLines(board);
                if (getFullLines(board) != 0) { rowCleared = true; }
                else { rowCleared = false; }
                board = removeFullLines(board);

                //Press keys to drop in right position
                dropPiece(bestPiece, bestMove.Index);
                if (rowCleared)
                {
                    LeftMouseClick(240, 485);
                    Thread.Sleep(600);
                }
                else
                {

                    Thread.Sleep(80);
                }
                //Give browser time to process
                nextPieceColor = nextNextPieceColor;
                nextNextPieceColor = getNextPieceColor(false);
                printBoard(board);

            }
            return -1;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        //Reads a color value from a pixel at the screen
        static public System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                            (int)(pixel & 0x0000FF00) >> 8,
                            (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        private class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

            public enum GetWindow_Cmd : uint
            {
                GW_HWNDFIRST = 0,
                GW_HWNDLAST = 1,
                GW_HWNDNEXT = 2,
                GW_HWNDPREV = 3,
                GW_OWNER = 4,
                GW_CHILD = 5,
                GW_ENABLEDPOPUP = 6
            }
        }

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        //Read a color from the screen and returns a color as a string
        public String getNextPieceColor(bool firstPiece)
        {
            Color pixelColor;
            float hue;
            int x = 0;
            int y = 0;


            if (firstPiece)
            {
                x = 467;
                y = 272;
                pixelColor = GetPixelColor(x, y);
            }
            else
            {
                x = 673;
                y = 333;
                pixelColor = GetPixelColor(x, y);
            }

            float sat = pixelColor.GetSaturation();
            float lgt = pixelColor.GetBrightness();
            hue = pixelColor.GetHue();
            while (lgt < 0.3 || lgt > 0.7 || sat < 0.3)
            {
                x++;

                pixelColor = GetPixelColor(x, y);
                sat = pixelColor.GetSaturation();
                lgt = pixelColor.GetBrightness();
                hue = pixelColor.GetHue();
            }
            Console.WriteLine("Hue: " + pixelColor.GetHue());
            Console.WriteLine("y: " + (y - 332));
            Console.WriteLine("x: " + (x - 673));

            if (hue < 28) return "red";
            if (hue < 40) return "orange";
            if (hue < 80) return "yellow";
            if (hue < 150) return "green";
            if (hue < 195) return "teal";
            if (hue < 255)
            {
                return "blue";
            }
            if (hue < 330) return "pink";
            return "red";

            //string[] colors = new string[7] { "red", "green", "blue", "teal", "orange", "pink", "yellow" };
            //int index;
            //Random RNG = new Random();
            //index = RNG.Next(0, colors.Length);
            //return colors[index];
        }

        //Get a score for the given board state
        public double evaluateBoard(bool[,] board, float cFullLines, float cHolesMade, float cHeightSum, float cBumpiness, float cHighest, float cCohesion, int cohesion, int holesMade)
        {
            double score = 0;
            float fullLines = getFullLines(board);
            board = removeFullLines(board);
            float heightSum = 0;
            float bumpiness = 0;
            float highest = 0;
            for (int row = 1; row < 19; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    if (board[row, column] == true && (18 - row) > highest)
                    {
                        highest = 19 - row;
                    }
                }

            }
            for (int column = 0; column < 10; column++)
            {
                heightSum += getColumnHeight(board, column);
                if (column < 9)
                {
                    bumpiness += Math.Abs(getColumnHeight(board, column)) - Math.Abs(getColumnHeight(board, column + 1));
                }
                if (getColumnHeight(board, column) > 17)
                {
                    score -= 100000;
                }
            }

            //Add blocked black holes
            if (score != -100000)
            {
                score = cFullLines * fullLines + cHolesMade * holesMade + cHeightSum * heightSum + cBumpiness * bumpiness + cHighest * highest + cCohesion * cohesion;
            }

            if (score == 0)
            {
                score = -100;
            }
            return score;
        }

        //Press keys to drop piece
        public void dropPiece(Piece piece, int position)
        {
            int rotations = 0;
            if (piece.Name.Length != 1)
            {
                rotations = Int32.Parse(piece.Name.Substring(1));
            }

            for (int i = 0; i < rotations; i++)
            {
                System.Windows.Forms.SendKeys.SendWait("x");
                Thread.Sleep(25);
            }
            int steps = 0;
            if (piece.Width % 2 == 0)
            {
                steps = position - 5 + (piece.Width / 2);
            }
            else if (piece.Width == 3)
            {
                steps = position - 3;
            }
            else if (piece.Width == 1)
            {
                steps = position - 5;
            }
            if (rotations == 3) { steps++; }
            if (steps < 0)
            {

                steps *= -1;
                for (int i = 0; i < steps; i++)
                {
                    System.Windows.Forms.SendKeys.SendWait("{LEFT}");
                    Thread.Sleep(25);
                }
            }
            else
            {
                for (int i = 0; i < steps; i++)
                {

                    System.Windows.Forms.SendKeys.SendWait("{RIGHT}");
                    Thread.Sleep(25);
                }
            }
            System.Windows.Forms.SendKeys.SendWait(" ");
        }

        //Add piece to board
        public bool[,] addPiece(bool[,] board, Piece piece, int position, out int cohesion, out int holesMade)
        {
            cohesion = 0;
            holesMade = 0;
            bool[,] _board = new bool[20, 10];
            Array.Copy(board, 0, _board, 0, board.Length);
            bool pieceCanBeLower = true;
            while (pieceCanBeLower)
            {
                for (int i = 4; i < 19; i++)
                {
                    if (piece.Width == 1)
                    {
                        if (i == 18 || _board[i + 1, position] != false)
                        {
                            _board[i, position] = true;
                            _board[i - 1, position] = true;
                            _board[i - 2, position] = true;
                            _board[i - 3, position] = true;
                            if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                            if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                            if (position == 0 || _board[i - 2, position - 1] == true) { cohesion++; }
                            if (position == 0 || _board[i - 3, position - 1] == true) { cohesion++; }
                            if (position == 9 || _board[i, position + 1] == true) { cohesion++; }
                            if (position == 9 || _board[i - 1, position + 1] == true) { cohesion++; }
                            if (position == 9 || _board[i - 2, position + 1] == true) { cohesion++; }
                            if (position == 9 || _board[i - 3, position + 1] == true) { cohesion++; }
                            holesMade += getHolesMade(board, i, position);
                            pieceCanBeLower = false;
                            i = 19;
                        }
                    }
                    else if (piece.Width == 2)
                    {
                        if (piece.Name == "O")
                        {
                            if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i - 1, position] = true;
                                _board[i - 1, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (position == 8 || _board[i, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i - 1, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "S1")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i - 1, position] = true;
                                _board[i + 1, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (position == 8 || _board[i, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i + 1, position + 2] == true) { cohesion++; }
                                if (_board[i + 1, position] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i + 1, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "Z1")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i - 1, position] = true;
                                _board[i - 1, position + 1] = true;
                                _board[i - 2, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (_board[i, position + 1] == true) { cohesion++; }
                                if (position == 8 || _board[i - 1, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i - 2, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i - 1, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "L1")
                        {
                            if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i - 1, position] = true;
                                _board[i - 2, position] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 2, position - 1] == true) { cohesion++; }
                                if (position == 8 || _board[i, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "L3")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i + 1, position + 1] = true;
                                _board[i + 2, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (_board[i + 1, position] == true) { cohesion++; }
                                if (_board[i + 2, position] == true) { cohesion++; }
                                if (position == 8 || _board[i, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i + 1, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i + 2, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i + 2, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "J1")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i - 1, position] = true;
                                _board[i - 2, position] = true;
                                _board[i - 2, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 2, position - 1] == true) { cohesion++; }
                                if (_board[i, position + 1] == true) { cohesion++; }
                                if (_board[i - 1, position + 1] == true) { cohesion++; }
                                if (position == 8 || _board[i - 2, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i - 2, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "J3")
                        {
                            if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i - 1, position + 1] = true;
                                _board[i - 2, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 8 || _board[i, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i - 1, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i - 2, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "T1")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i - 1, position] = true;
                                _board[i - 2, position] = true;
                                _board[i - 1, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 2, position - 1] == true) { cohesion++; }
                                if (position == 8 || _board[i - 1, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i - 1, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "T3")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i + 1, position + 1] = true;
                                _board[i - 1, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 8 || _board[i, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i + 1, position + 2] == true) { cohesion++; }
                                if (position == 8 || _board[i - 1, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i + 1, position + 1);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                    }
                    else if (piece.Width == 3)
                    {
                        if (piece.Name == "S0")
                        {
                            if (i == 18 || i + piece.yOffset[1] > 17 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false || _board[i + 1 + piece.yOffset[1], position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i - 1, position + 1] = true;
                                _board[i - 1, position + 2] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (_board[i - 1, position] == true) { cohesion++; }
                                if (_board[i, position + 2] == true) { cohesion++; }
                                if (position == 7 || _board[i - 1, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                holesMade += getHolesMade(board, i - 1, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "Z0")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || i + piece.yOffset[1] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false || _board[i + 1 + piece.yOffset[1], position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i + 1, position + 1] = true;
                                _board[i + 1, position + 2] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (_board[i + 1, position] == true) { cohesion++; }
                                if (_board[i, position + 2] == true) { cohesion++; }
                                if (position == 7 || _board[i + 1, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i + 1, position + 1);
                                holesMade += getHolesMade(board, i + 1, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "L0")
                        {
                            if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false || _board[i + 1, position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i, position + 2] = true;
                                _board[i - 1, position + 2] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 7 || _board[i, position + 3] == true) { cohesion++; }
                                if (position == 7 || _board[i - 1, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                holesMade += getHolesMade(board, i, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "L2")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || i + piece.yOffset[1] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false || _board[i + 1 + piece.yOffset[1], position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i - 1, position] = true;
                                _board[i - 1, position + 1] = true;
                                _board[i - 1, position + 2] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (_board[i, position + 1] == true) { cohesion++; }
                                if (position == 7 || _board[i - 1, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i - 1, position + 1);
                                holesMade += getHolesMade(board, i - 1, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "J0")
                        {
                            if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false || _board[i + 1, position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i, position + 2] = true;
                                _board[i - 1, position] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 0 || _board[i - 1, position - 1] == true) { cohesion++; }
                                if (position == 7 || _board[i, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                holesMade += getHolesMade(board, i, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "J2")
                        {
                            if (i == 18 || i + piece.yOffset[1] > 17 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false || _board[i + 1 + piece.yOffset[1], position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i, position + 2] = true;
                                _board[i + 1, position + 2] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (_board[i + 1, position + 1] == true) { cohesion++; }
                                if (position == 7 || _board[i, position + 3] == true) { cohesion++; }
                                if (position == 7 || _board[i + 1, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                holesMade += getHolesMade(board, i + 1, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "T0")
                        {
                            if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false || _board[i + 1, position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i, position + 2] = true;
                                _board[i - 1, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 7 || _board[i, position + 3] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i, position + 1);
                                holesMade += getHolesMade(board, i, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                        else if (piece.Name == "T2")
                        {
                            if (i == 18 || i + piece.yOffset[0] > 17 || _board[i + 1, position] != false || _board[i + 1 + piece.yOffset[0], position + 1] != false || _board[i + 1, position + 2] != false)
                            {
                                _board[i, position] = true;
                                _board[i, position + 1] = true;
                                _board[i, position + 2] = true;
                                _board[i + 1, position + 1] = true;
                                if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                                if (position == 7 || _board[i, position + 3] == true) { cohesion++; }
                                if (position == 0 || _board[i + 1, position] == true) { cohesion++; }
                                if (position == 7 || _board[i + 1, position + 2] == true) { cohesion++; }
                                holesMade += getHolesMade(board, i, position);
                                holesMade += getHolesMade(board, i + 1, position + 1);
                                holesMade += getHolesMade(board, i, position + 2);
                                pieceCanBeLower = false;
                                i = 19;
                            }
                        }
                    }
                    else if (piece.Width == 4)
                    {
                        if (i == 18 || _board[i + 1, position] != false || _board[i + 1, position + 1] != false || _board[i + 1, position + 2] != false || _board[i + 1, position + 3] != false)
                        {
                            _board[i, position] = true;
                            _board[i, position + 1] = true;
                            _board[i, position + 2] = true;
                            _board[i, position + 3] = true;
                            if (position == 0 || _board[i, position - 1] == true) { cohesion++; }
                            if (position == 6 || _board[i, position + 4] == true) { cohesion++; }
                            holesMade += getHolesMade(board, i, position);
                            holesMade += getHolesMade(board, i, position + 1);
                            holesMade += getHolesMade(board, i, position + 2);
                            holesMade += getHolesMade(board, i, position + 3);
                            pieceCanBeLower = false;
                            i = 19;
                        }
                    }
                }
            }
            return _board;
        }

        //Returns the number of full lines
        public int getFullLines(bool[,] board)
        {
            int lines = 0;
            bool holeFound = false;
            for (int k = 0; k < board.GetLength(0); k++)
            {
                holeFound = false;
                for (int l = 0; l < board.GetLength(1); l++)
                {
                    if (board[k, l] == false)
                    {
                        holeFound = true;
                    }
                }
                if (!holeFound)
                {
                    lines++;
                }
            }
            return lines;
        }

        //Return height of specified column
        public int getColumnHeight(bool[,] board, int column)
        {
            int i = 0;
            for (i = 0; i < 20; i++)
            {
                if (board[i, column] == true)
                {
                    return 19 - i;
                }
            }
            return 0;
        }

        //Return num of holes under coordinate
        public int getHolesMade(bool[,] board, int row, int column)
        {
            int holes = 0;
            for (int i = row + 1; i < board.GetLength(0) - 1; i++)
            {
                if (board[i, column] == false)
                {
                    holes++;
                }
                if (board[i, column] == true)
                {
                    return holes;
                }
            }
            return holes;
        }

        //Prints the board
        public void printBoard(bool[,] board)
        {
            for (int k = 0; k < board.GetLength(0) - 1; k++)
            {

                for (int l = 0; l < board.GetLength(1); l++)
                {
                    if (board[k, l] == false)

                    {
                        Console.Write("_");
                    }
                    else
                    {
                        Console.Write("X");
                    }

                }
                Console.Write("\n");

            }
            Console.Write("\n");
        }

        //Returns a board with the full lines removed
        public bool[,] removeFullLines(bool[,] board)
        {

            bool holeFound = false;
            for (int k = 0; k < board.GetLength(0); k++)
            {
                holeFound = false;
                for (int l = 0; l < board.GetLength(1); l++)
                {
                    if (board[k, l] == false)
                    {
                        holeFound = true;
                    }
                    if (holeFound == false && l + 1 == board.GetLength(1))
                    {
                        while (k > 0)
                        {
                            for (int m = 0; m < board.GetLength(1); m++)
                            {
                                board[k, m] = board[k - 1, m];
                            }
                            k--;
                        }
                    }
                }

            }
            return board;
        }
    }
}






