using System;

namespace PuzzleWindows
{
    internal class PuzzleGameEngine
    {
        int[] theViewIndices;
        private int puzzleSize = 16;
        public PuzzleGameEngine()
        {
            //이미지 index 가져오자
            theViewIndices = new int[puzzleSize];
            for(int i = 0; i < 16; i++)
            {
                theViewIndices[i] = i;
            }
            //이미지 index 섞자
            //theViewIndices[0] = 1;
            //theViewIndices[1] = 0;
            Random r = new Random();
            for(int i = 0; i < 10000; i++)//영원히 맞춰지지 않는 퍼즐이 나오지 않도록.
            {
                int i1 = r.Next(16);
               // int i2 = r.Next(16);
                Change(i1);
            }
           
           
        }

        private void Swap(int i1, int i2)
        {
            int temp=theViewIndices[i1];
            theViewIndices[i1]=theViewIndices[i2];
            theViewIndices[i2]=temp;
        }

        internal int GetViewIndex(int index)
        {
            return theViewIndices[index];
        }

        internal void Change(int touchedindex)
        {
            //터치한 인텍스 상하좌우에 인덱스가 있다면 
            //교환하자
            //이게 핵심
            if (GetEmptyIndex()/4==touchedindex/4
                &&(GetEmptyIndex()==touchedindex-1
                ||GetEmptyIndex()==touchedindex+1)
                || GetEmptyIndex() == touchedindex-4
                ||GetEmptyIndex() == touchedindex +4)
            {//상하좌우를  눌렀을 때 
                Swap(GetEmptyIndex(), touchedindex);
            }
           
            
        }
        

        private int GetEmptyIndex()
        {
            for (int i = 0;i< puzzleSize; i++){
                if (theViewIndices[i] == puzzleSize - 1)
                    return i;
            }
            return -1;
        }

        internal bool isEnd()
        {
            int count = 0;
            for(int i = 0; i < puzzleSize; i++)
            {
                if (theViewIndices[i] == i)
                {
                    count++;
                }
            }
            return count == puzzleSize;
        }
    }
}