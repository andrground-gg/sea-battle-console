using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public enum Direction { Left, Right, Up, Down };
    class Ship
    {
        private int decks;
        private Direction? direction;
        private List<int> xCoords = new List<int>();
        private List<int> yCoords = new List<int>();

        public Ship(int decks, int startY, int startX, Direction? direction, ref int[][] field) {
            this.decks = decks;
            this.direction = direction;         
            this.yCoords.Add(startY);
            this.xCoords.Add(startX);
            this.locateShip(ref field);
        }
        public List<int> YCoords
        {
            get { return yCoords; }
            set { yCoords = value; }
        }
        public List<int> XCoords
        {
            get { return xCoords; }
            set { xCoords = value; }
        }
        public void Delete() { }
        public void locateShip(ref int[][] field)
        {
            switch (this.direction)
            {
                case Direction.Left:
                    for (int j = this.xCoords[0]; j > this.xCoords[0] - this.decks; j--) {
                        field[this.yCoords[0]][j] = 1;
                        if (j != this.xCoords[0]){
                            this.yCoords.Add(this.yCoords[0]);
                            this.xCoords.Add(j);
                        }
                    }
                    break;
                case Direction.Right:
                    for (int j = this.xCoords[0]; j < this.xCoords[0] + this.decks; j++) {
                        field[this.yCoords[0]][j] = 1;
                        if (j != this.xCoords[0]){
                            this.yCoords.Add(this.yCoords[0]);
                            this.xCoords.Add(j);
                        }
                    }
                    break;
                case Direction.Up:
                    for (int j = this.yCoords[0]; j > this.yCoords[0] - this.decks; j--) {
                        field[j][this.xCoords[0]] = 1;
                        if (j != this.yCoords[0]){
                            this.yCoords.Add(j);
                            this.xCoords.Add(this.xCoords[0]);
                        }
                    }
                    break;
                case Direction.Down:
                    for (int j = this.yCoords[0]; j < this.yCoords[0] + this.decks; j++) {
                        field[j][this.xCoords[0]] = 1;
                        if (j != this.yCoords[0]){
                            this.yCoords.Add(j);
                            this.xCoords.Add(this.xCoords[0]);
                        }
                    }
                    break;
            }
        }
        static public bool checkPosition(int decks, int startY, int startX, Direction? direction, int[][] field){
            if (startY == -1 || startX == -1)
                return false;
            switch (direction)
            {
                case Direction.Left:
                    for (int i = (startX + 1 >= field[0].Length ? startX : startX + 1); i >= (startX - decks < 0 ? startX - decks +1 : startX - decks); i--)
                    {
                        if (i < 0 || field[startY][i] == 1)                       
                            return false;
                        if (startY + 1 < field.Length)
                            if (field[startY + 1][i] == 1)
                                return false;
                        if (startY - 1 >= 0)
                            if (field[startY - 1][i] == 1)
                                return false;
                    }; break;
                case Direction.Right:
                    for (int i = (startX - 1 < 0 ? startX : startX - 1); i <= (startX + decks >= field[0].Length ? startX + decks -1: startX + decks ); i++)
                    {
                        if (i >= field[0].Length || field[startY][i] == 1)
                            return false;
                        if (startY + 1 < field.Length)
                            if (field[startY + 1][i] == 1)
                                return false;
                        if (startY - 1 >= 0)
                            if (field[startY - 1][i] == 1)
                                return false;
                    }; break;
                case Direction.Up:
                    for (int i = (startY + 1 >= field.Length ? startY : startY + 1); i >= (startY - decks < 0 ? startY - decks +1: startY - decks); i--)
                    {
                        if (i < 0 || field[i][startX] == 1)
                            return false;
                        if (startX + 1 < field[0].Length)
                            if (field[i][startX + 1] == 1)
                                return false;
                        if (startX - 1 >= 0)
                            if (field[i][startX - 1] == 1)
                                return false;
                    }; break;
                case Direction.Down:
                    for (int i = (startY - 1 < 0 ? startY : startY - 1); i <= (startY + decks >= field.Length ? startY + decks-1: startY + decks ); i++)
                    {
                        if (i >= field.Length || field[i][startX] == 1)
                            return false;
                        if (startX + 1 < field[0].Length)
                            if (field[i][startX + 1] == 1)
                                return false;
                        if (startX - 1 >= 0)
                            if (field[i][startX - 1] == 1)
                                return false;
                    }; break;
                default: return false; 
            }
            return true;
        }
        public bool checkIfDestroyed(ref int[][] field)
        {
            for (int i = 0; i < this.yCoords.Count; i++)
            {
                if (field[this.yCoords[i]][this.xCoords[i]] != 3)
                    break;
                if (i == this.decks - 1)
                {
                    switch(this.direction){
                        case Direction.Left: 
                            for(int n = (this.xCoords[0] + 1 >= field[0].Length ? this.xCoords[0] : this.xCoords[0] + 1); n >= (this.xCoords[0] - this.decks < 0 ? this.xCoords[0] - this.decks + 1 : this.xCoords[0] - this.decks); n--){
                                field[this.yCoords[0]][n] = (n <= this.xCoords[0] - this.decks || n > this.xCoords[0] ? 4 : 5);
                                if (this.yCoords[0] - 1 >= 0)
                                    field[this.yCoords[0] - 1][n] = 4;
                                if (this.yCoords[0] + 1 < field.Length)
                                    field[this.yCoords[0] + 1][n] = 4;
                            }; break;
                        case Direction.Right:
                            for (int n = (this.xCoords[0] - 1 < 0 ? this.xCoords[0] : this.xCoords[0] - 1); n <= (this.xCoords[0] + this.decks >= field[0].Length ? this.xCoords[0] + this.decks - 1 : this.xCoords[0] + this.decks); n++)
                            {
                                field[this.yCoords[0]][n] = (n < this.xCoords[0] || n >= this.xCoords[0] + this.decks ? 4 : 5);
                                if (this.yCoords[0] - 1 >= 0)
                                    field[this.yCoords[0] - 1][n] = 4;
                                if (this.yCoords[0] + 1 < field.Length)
                                    field[this.yCoords[0] + 1][n] = 4;
                            }; break;
                        case Direction.Up:
                            for (int n = (this.yCoords[0] + 1 >= field.Length ? this.yCoords[0] : this.yCoords[0] + 1); n >= (this.yCoords[0] - this.decks < 0 ? this.yCoords[0] - this.decks + 1: this.yCoords[0] - this.decks); n--)
                            {
                                field[n][this.xCoords[0]] = (n <= this.yCoords[0] - this.decks || n > this.yCoords[0] ? 4 : 5);
                                if (this.xCoords[0] - 1 >= 0)
                                    field[n][this.xCoords[0] - 1] = 4;
                                if (this.xCoords[0] + 1 < field[0].Length)
                                    field[n][this.xCoords[0] + 1] = 4;
                            }; break;
                        case Direction.Down:
                            for (int n = (this.yCoords[0] - 1 < 0 ? this.yCoords[0] : this.yCoords[0] - 1); n <= (this.yCoords[0] + this.decks >= field.Length ? this.yCoords[0] + this.decks - 1 : this.yCoords[0] + this.decks); n++)
                            {
                                field[n][this.xCoords[0]] = (n < this.yCoords[0] || n >= this.yCoords[0] + this.decks ? 4 : 5);
                                if (this.xCoords[0] - 1 >= 0)
                                    field[n][this.xCoords[0] - 1] = 4;
                                if (this.xCoords[0] + 1 < field[0].Length)
                                    field[n][this.xCoords[0] + 1] = 4;
                            }; break;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
