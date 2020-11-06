namespace AGV_에러_체크
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.la_state = new System.Windows.Forms.Label();
            this.Search_Timer = new System.Windows.Forms.Timer(this.components);
            this.ErrChk_Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // la_state
            // 
            this.la_state.BackColor = System.Drawing.SystemColors.Control;
            this.la_state.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.la_state.Font = new System.Drawing.Font("돋움", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.la_state.Location = new System.Drawing.Point(12, 8);
            this.la_state.Name = "la_state";
            this.la_state.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.la_state.Size = new System.Drawing.Size(235, 60);
            this.la_state.TabIndex = 1;
            this.la_state.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Search_Timer
            // 
            this.Search_Timer.Interval = 10;
            this.Search_Timer.Tick += new System.EventHandler(this.Search_Timer_Tick);
            // 
            // ErrChk_Timer
            // 
            this.ErrChk_Timer.Interval = 10;
            this.ErrChk_Timer.Tick += new System.EventHandler(this.ErrChk_Timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 77);
            this.Controls.Add(this.la_state);
            this.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "AGV 에러 체크";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label la_state;
        private System.Windows.Forms.Timer Search_Timer;
        private System.Windows.Forms.Timer ErrChk_Timer;
    }
}

