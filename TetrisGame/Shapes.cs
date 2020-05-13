using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TetrisGame
{
    class Shapes
    {
        // arrays of Tetris shape

        // O
        public static int[,] O_Tetromino = new int[2, 2] { { 1, 1 },  // * *
                                                    { 1, 1 }}; // * *

        //I
        public static int[,] I_Tetromino_0 = new int[2, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 } };// * * * *

        public static int[,] I_Tetromino_90 = new int[4, 2] {{ 1,0 },   // *  
                                                       { 1,0 },  // *
                                                       { 1,0 },  // *
                                                       { 1,0 }}; // *
        //T
        public static int[,] T_Tetromino_0 = new int[2, 3] {{0,1,0},    //    * 
                                                     {1,1,1}};   //  * * *

        public static int[,] T_Tetromino_90 = new int[3, 2] {{1,0},     //  * 
                                                      {1,1},     //  * *
                                                      {1,0}};    //  *  

        public static int[,] T_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * *
                                                       {0,1,0}}; //   * 

        public static int[,] T_Tetromino_270 = new int[3, 2] {{0,1},    //   * 
                                                       {1,1},    // * *
                                                       {0,1}};   //   *  
        //S
        public static int[,] S_Tetromino_0 = new int[2, 3] {{0,1,1},    //   * *
                                                     {1,1,0}};   // * *

        public static int[,] S_Tetromino_90 = new int[3, 2] {{1,0},     // *
                                                      {1,1},     // * *
                                                      {0,1}};    //   *
        //Z 
        public static int[,] Z_Tetromino_0 = new int[2, 3] {{1,1,0},    // * *
                                                     {0,1,1}};   //   * *

        public static int[,] Z_Tetromino_90 = new int[3, 2] {{0,1},     //   *
                                                      {1,1},     // * *
                                                      {1,0}};    // *
        //J
        public static int[,] J_Tetromino_0 = new int[2, 3] {{1,0,0},    // * 
                                                     {1,1,1}};   // * * *

        public static int[,] J_Tetromino_90 = new int[3, 2] {{1,1},     // * * 
                                                      {1,0},     // *
                                                      {1,0}};    // * 

        public static int[,] J_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * * 
                                                       {0,0,1}}; //     *

        public static int[,] J_Tetromino_270 = new int[3, 2] {{0,1},    //   * 
                                                       {0,1},    //   *
                                                       {1,1 }};  // * *

        //L 
        public static int[,] L_Tetromino_0 = new int[2, 3] {{0,0,1},    //     * 
                                                     {1,1,1}};   // * * *

        public static int[,] L_Tetromino_90 = new int[3, 2] {{1,0},     // *  
                                                      {1,0},     // *
                                                      {1,1}};    // * *

        public static int[,] L_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * * 
                                                       {1,0,0}}; // *

        public static int[,] L_Tetromino_270 = new int[3, 2] {{1,1},    // * * 
                                                       {0,1},    //   *
                                                       {0,1 }};  //   *

        //Colors
        public static Color O_TetrisItemColor = Colors.GreenYellow;
        public static Color I_TetrisItemColor = Colors.Red;
        public static Color T_TetrisItemColor = Colors.Gold;
        public static Color S_TetrisItemColor = Colors.Violet;
        public static Color Z_TetrisItemColor = Colors.DeepSkyBlue;
        public static Color J_TetrisItemColor = Colors.Cyan;
        public static Color L_TetrisItemColor = Colors.LightSeaGreen;


        public static Color[] ShapeColor = {  O_TetrisItemColor,I_TetrisItemColor,
                                T_TetrisItemColor,S_TetrisItemColor,
                                Z_TetrisItemColor,J_TetrisItemColor,
                                L_TetrisItemColor
                             };
        public static string[] ArrayTetrisItems = { "","O_Tetromino" , "I_Tetromino_0",
                                        "T_Tetromino_0","S_Tetromino_0",
                                        "Z_Tetromino_0","J_Tetromino_0",
                                        "L_Tetromino_0"
                                   };
    }
}
