using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleWindows
{
    public partial class Form1 : Form
    {
        PuzzleGameEngine pge;
        List<Image> imgList = new List<Image>(); //java ArrayList와 비슷하게 생각하면 돼.
        private int puzzleSize = 16;//4x4
        int imgWidth = 100;
        int imgHeight = 100;
        private Font theFont;
        private Brush theBrush;
        private int theTick;
        private int theGameTick;
        private Pen thePen;


        public Form1()
        {
            InitializeComponent();

            //image를 가져오자 
            //image를 List에 넣자 
            for (int i = 0; i < puzzleSize; i++)
            {
                string fileName = string.Format("pic_{0}.png", (char)('a' + i));//file이름에 따라 이렇게 조작?해야해
                Image tmpI = Image.FromFile(fileName);//C:\Users\Mirim\source\repos\PuzzleWindows\PuzzleWindows\bin\Debug 이 경로에 이미지 옮겨줘야함.
                imgList.Add(tmpI);
             }

            //PuzzleGameEngine 생성하자
            //이미지를 옮기고 줄이고 그래도 계속 그림 계속 그리는 것이기 때문에 paint함수 계속 불러!

            //여기 생성자라서 값넣는거얌
            pge = new PuzzleGameEngine();

            //timer
            theFont = new Font("굴림", 15);
            theBrush = new SolidBrush(Color.Green);
            thePen = new Pen(Color.Red);
            theTick = 0;
            theGameTick = 0;//창이 생성될 때 0으로 초기화 
            timer1.Start();

        }





        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(558, 502);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            //this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint_1);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown_1);
            this.ResumeLayout(false);

        }

       

        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {
            //timer 표시하자.
            int time = 100 - (theTick - theGameTick) / (1000 / timer1.Interval);//초가 나오는거 (99초부터 시간이 줄어든다.)
            string timeString = string.Format("Time:{0:D3}", time);
            //string theFont=string.
            e.Graphics.DrawString(timeString, theFont, theBrush, 0, 15);//어떤 문자열, 폰트, 브러쉬(펜)
            e.Graphics.DrawRectangle(thePen, 0, 0, time, 10);//0,0의 좌표  가로 100 
            e.Graphics.FillRectangle(theBrush, 0, 0, time, 20);

            if (time == 0)
            {
                timer1.Stop();
                MessageBox.Show("GAME OVER");

                return;
            }


            //그릴때, 4X4로 그리자
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (pge.GetViewIndex(i + j * 4) != puzzleSize - 1) //15번에 해당하는 그림은 그리지 말고 나머지는 그려라.
                    {
                        e.Graphics.DrawImage(imgList[pge.GetViewIndex(i + j * 4)], 30 + i * imgWidth, 30 + j * imgHeight, imgWidth, imgHeight);
                    }

                }
            }
        }

        //클릭했을 때, 클릭한 index와 빈 칸 index를 교체하자
        private void Form1_MouseDown_1(object sender, MouseEventArgs e)
        {
            int tmpX = e.X;
            int tmpY = e.Y;
            if (!(50 <= tmpX && tmpX <= 4 * imgWidth + 50 && 50 <= tmpY && tmpY <= 4 * imgWidth + 50)) return; //알파벳이 아닌 부분을 클릭하면 밑 부분 작동 안합니다.
            //MessageBox.Show(tmpX + ", " + tmpY);
            //x,y좌표를 구역으로 바꾸자
            tmpX -= 50;
            tmpY -= 50; //패딩만큼 더해서 50,50  50,150, 이렇게 시작하는 걸 50을 빼줌으로서 0,0부터 시작할 수 있게!
            tmpX /= imgWidth;//0,100,200을 나누면 0,1,2,3, 이렇게 가는거야
            tmpY /= imgHeight;

            int index = tmpX + tmpY * 4;//왜 4?
            // MessageBox.Show(index + "");//string 으로 변환 할 수 없다. 가장 쉬운 방법은 뒤에 +""
            pge.Change(index);
            Invalidate();

            //다 맞췄으면 , 메시지 박스 ㅊㅋㅊㅋ
            //창 닫자
            if (pge.isEnd())
            {
                MessageBox.Show("다 맞추셨어요!!! 축하드립니다!!!!");
                Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            theTick++;
            Invalidate();
        }
    }
}
