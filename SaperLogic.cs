using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class SaperLogic
    {
        public enum FieldTypeEnum { Bomb, Empty }
        public enum GameState { Win, Defeat, InProgress }
        private class Field
        {
            readonly FieldTypeEnum fieldType; 
            public FieldTypeEnum FieldType{ get { return fieldType; } }
            public Boolean isCovered = true;
            public int neighbours;

            public Field(FieldTypeEnum ft, int n=0)
            {
                fieldType = ft;
                neighbours = n;
            }
        }
        Random gen = new Random();
        private Field[,] Board;

        public SaperLogic(int width, int height, int bomb)
        {
            Board = new Field[width, height];
            //generowanie bomb
            do
            {
                int x = gen.Next(Width);
                int y = gen.Next(Height);

                if(Board[x,y] == null)
                {
                    Board[x, y] = new Field(FieldTypeEnum.Bomb);
                    bomb--;
                }
                
            } while (bomb > 0);

            //reszta pusta
            for(int x = 0; x<Width; x++)
            { 
                for(int y = 0; y<Height; y++ )
                {
                    if (Board[x, y] == null)
                    {
                        int count = 0;
                        for(int xx=x-1; xx<=x+1; xx++)
                        {
                            for(int yy=y-1; yy<=y+1; yy++)
                            {
                                if(xx>=0 && yy>=0 && xx<Width && yy<Height &&
                                    Board[xx, yy] != null &&
                                    Board[xx, yy].FieldType==FieldTypeEnum.Bomb)
                                {
                                    count++;
                                }
                            }
                        }
                        Board[x, y] = new Field(FieldTypeEnum.Empty, count);
                    }
                        
                }
            }
        }

        internal int GetNeighbours(int x, int y)
        {
            return Board[x, y].neighbours;
        }

        internal bool GetFieldUncovered(int x, int y)
        {
            return !Board[x, y].isCovered;
        }

        public int Width { get { return Board.GetLength(0); } }
        public int Height { get { return Board.GetLength(1); } }

        internal FieldTypeEnum GetFieldType(int x, int y)
        {
            return Board[x, y].FieldType;
        }

        internal GameState Uncover(int x, int y)
        {
            if (Board[x, y].isCovered)
            {
                Board[x, y].isCovered = false;
                if (Board[x, y].FieldType != FieldTypeEnum.Bomb &&
                    Board[x, y].neighbours == 0)
                {
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        for (int yy = y - 1; yy <= y + 1; yy++)
                        {
                            if (xx >= 0 && yy >= 0 && xx < Width && yy < Height)
                            {
                                Uncover(xx, yy);
                            }
                        }
                    }
                }

                if(Board[x, y].FieldType==FieldTypeEnum.Bomb)
                {
                    return GameState.Defeat;
                }
                else
                {
                    for (int xx = 0; xx < Width; xx++)
                    {
                        for (int yy = 0; yy < Height; yy++)
                        {
                            if (Board[xx, yy].FieldType == FieldTypeEnum.Empty && Board[xx,yy].isCovered == true)
                            {
                                return GameState.InProgress;
                            }
                        }
                    }
                    return GameState.Win;
                }
            }
            return GameState.InProgress;
        }
    }
}
