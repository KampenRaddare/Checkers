namespace Checkers {
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    internal sealed partial class CheckerBoard:Form {
        private PictureBox[,] PictureBoxes;
        private int BoardSize;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd,int Msg,int wParam,int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        internal CheckerBoard(int boardSize) {
            InitializeComponent();
            BoardSize = boardSize;
            PictureBoxes = new PictureBox[BoardSize,BoardSize];
            int squareSize = Width / BoardSize;
            for(int y = 0;y < BoardSize;y += 1) {
                for(int x = 0;x < BoardSize;x += 1) {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Height = squareSize;
                    pictureBox.Width = squareSize;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Location = new Point(x * squareSize,y * squareSize);
                    pictureBox.Visible = true;
                    pictureBox.MouseDown += new MouseEventHandler(CheckerBoard_MouseDown);
                    Controls.Add(pictureBox);
                    PictureBoxes[x,y] = pictureBox;
                }
            }
        }
        internal void UpdateBoard(Tile[,] board) {
            for(int y = 0;y < BoardSize;y += 1) {
                for(int x = 0;x < BoardSize;x += 1) {
                    Bitmap image = null;
                    switch(board[x,y]) {
                        case Tile.White:
                            image = CheckerBoardImages.White;
                            break;
                        case Tile.Black:
                            image = CheckerBoardImages.Black;
                            break;
                        case Tile.RedChecker:
                            image = CheckerBoardImages.RedChecker;
                            break;
                        case Tile.WhiteChecker:
                            image = CheckerBoardImages.WhiteChecker;
                            break;
                        case Tile.KingedRedChecker:
                            image = CheckerBoardImages.RedCheckerKing;
                            break;
                        case Tile.KingedWhiteChecker:
                            image = CheckerBoardImages.WhiteCheckerKing;
                            break;
                    }
                    PictureBoxes[x,y].Image = image;
                }
            }
        }
        private void InitializeComponent() {
            SuspendLayout();
            Size size = new Size(440,440);
            AccessibleRole = AccessibleRole.None;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.Disable;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.None;
            CausesValidation = false;
            ClientSize = size;
            DoubleBuffered = true;
            Font = new Font("Segoe UI",12F);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.None;
            ImeMode = ImeMode.Disable;
            Margin = new Padding(0);
            MaximizeBox = false;
            MaximumSize = size;
            MinimumSize = size;
            Name = "CheckerBoard";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            ResumeLayout(false);
        }
        private void CheckerBoard_MouseDown(object sender,MouseEventArgs e) {
            if(e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle,WM_NCLBUTTONDOWN,HT_CAPTION,0);
            }
        }
    }
}