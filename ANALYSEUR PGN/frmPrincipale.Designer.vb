<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrincipale
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmdAnalyser = New System.Windows.Forms.Button()
        Me.cmdArreter = New System.Windows.Forms.Button()
        Me.dlgImport = New System.Windows.Forms.OpenFileDialog()
        Me.cdrMoteur = New System.Windows.Forms.GroupBox()
        Me.lblChemin2 = New System.Windows.Forms.Label()
        Me.lblChemin1 = New System.Windows.Forms.Label()
        Me.cbPriorite = New System.Windows.Forms.ComboBox()
        Me.lblPriorite = New System.Windows.Forms.Label()
        Me.txtContempt = New System.Windows.Forms.TextBox()
        Me.lblContempt = New System.Windows.Forms.Label()
        Me.lblMo = New System.Windows.Forms.Label()
        Me.txtMemoire = New System.Windows.Forms.TextBox()
        Me.lblMemoire = New System.Windows.Forms.Label()
        Me.lblProc = New System.Windows.Forms.Label()
        Me.txtPuissance = New System.Windows.Forms.TextBox()
        Me.lblPuissance = New System.Windows.Forms.Label()
        Me.cmdOptions2 = New System.Windows.Forms.Button()
        Me.cmdOptions1 = New System.Windows.Forms.Button()
        Me.lblMoteur2 = New System.Windows.Forms.Label()
        Me.cmdMoteur2 = New System.Windows.Forms.Button()
        Me.lblMoteur1 = New System.Windows.Forms.Label()
        Me.cmdMoteur1 = New System.Windows.Forms.Button()
        Me.cdrPosition = New System.Windows.Forms.GroupBox()
        Me.cbFixee = New System.Windows.Forms.CheckBox()
        Me.lblFichier = New System.Windows.Forms.Label()
        Me.lblEstimation = New System.Windows.Forms.Label()
        Me.lblProgression = New System.Windows.Forms.Label()
        Me.txtReprise = New System.Windows.Forms.TextBox()
        Me.lblIndex = New System.Windows.Forms.Label()
        Me.lblListe = New System.Windows.Forms.Label()
        Me.cmdListe = New System.Windows.Forms.Button()
        Me.lblSecondes = New System.Windows.Forms.Label()
        Me.txtLimite = New System.Windows.Forms.TextBox()
        Me.lblLimite = New System.Windows.Forms.Label()
        Me.cbArret = New System.Windows.Forms.CheckBox()
        Me.cbLog = New System.Windows.Forms.CheckBox()
        Me.cdrAnalyse = New System.Windows.Forms.GroupBox()
        Me.lblSyzygy2 = New System.Windows.Forms.Label()
        Me.lblPonder = New System.Windows.Forms.Label()
        Me.lblInfos2 = New System.Windows.Forms.Label()
        Me.lblSyzygy1 = New System.Windows.Forms.Label()
        Me.lblInfos1 = New System.Windows.Forms.Label()
        Me.lblHash2 = New System.Windows.Forms.Label()
        Me.lblHash1 = New System.Windows.Forms.Label()
        Me.lblTB2 = New System.Windows.Forms.Label()
        Me.lblScore2 = New System.Windows.Forms.Label()
        Me.lblScore1 = New System.Windows.Forms.Label()
        Me.lblTB1 = New System.Windows.Forms.Label()
        Me.lblIdem = New System.Windows.Forms.Label()
        Me.lblKnps2 = New System.Windows.Forms.Label()
        Me.lblKnps1 = New System.Windows.Forms.Label()
        Me.lblEval2 = New System.Windows.Forms.Label()
        Me.lblEval1 = New System.Windows.Forms.Label()
        Me.lblPartie = New System.Windows.Forms.Label()
        Me.lblProfondeur2 = New System.Windows.Forms.Label()
        Me.lblProfondeur1 = New System.Windows.Forms.Label()
        Me.lblCoup2 = New System.Windows.Forms.Label()
        Me.lblCoup1 = New System.Windows.Forms.Label()
        Me.lblRestant = New System.Windows.Forms.Label()
        Me.lblDernier2 = New System.Windows.Forms.Label()
        Me.lblDernier1 = New System.Windows.Forms.Label()
        Me.lblResultat2 = New System.Windows.Forms.Label()
        Me.lblResultat1 = New System.Windows.Forms.Label()
        Me.lblPosition = New System.Windows.Forms.Label()
        Me.timerAnalyse = New System.Windows.Forms.Timer(Me.components)
        Me.dlgExport = New System.Windows.Forms.SaveFileDialog()
        Me.timerProgression = New System.Windows.Forms.Timer(Me.components)
        Me.cbDiffere = New System.Windows.Forms.CheckBox()
        Me.pbEchiquier = New System.Windows.Forms.PictureBox()
        Me.pbMateriel = New System.Windows.Forms.PictureBox()
        Me.cdrMoteur.SuspendLayout()
        Me.cdrPosition.SuspendLayout()
        Me.cdrAnalyse.SuspendLayout()
        CType(Me.pbEchiquier, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMateriel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdAnalyser
        '
        Me.cmdAnalyser.AutoSize = True
        Me.cmdAnalyser.Location = New System.Drawing.Point(633, 227)
        Me.cmdAnalyser.Name = "cmdAnalyser"
        Me.cmdAnalyser.Size = New System.Drawing.Size(87, 33)
        Me.cmdAnalyser.TabIndex = 46
        Me.cmdAnalyser.Text = "ANALYSE"
        Me.cmdAnalyser.UseVisualStyleBackColor = True
        '
        'cmdArreter
        '
        Me.cmdArreter.AutoSize = True
        Me.cmdArreter.Location = New System.Drawing.Point(632, 361)
        Me.cmdArreter.Name = "cmdArreter"
        Me.cmdArreter.Size = New System.Drawing.Size(87, 33)
        Me.cmdArreter.TabIndex = 47
        Me.cmdArreter.Text = "STOP"
        Me.cmdArreter.UseVisualStyleBackColor = True
        '
        'cdrMoteur
        '
        Me.cdrMoteur.BackColor = System.Drawing.SystemColors.Control
        Me.cdrMoteur.Controls.Add(Me.lblChemin2)
        Me.cdrMoteur.Controls.Add(Me.lblChemin1)
        Me.cdrMoteur.Controls.Add(Me.cbPriorite)
        Me.cdrMoteur.Controls.Add(Me.lblPriorite)
        Me.cdrMoteur.Controls.Add(Me.txtContempt)
        Me.cdrMoteur.Controls.Add(Me.lblContempt)
        Me.cdrMoteur.Controls.Add(Me.lblMo)
        Me.cdrMoteur.Controls.Add(Me.txtMemoire)
        Me.cdrMoteur.Controls.Add(Me.lblMemoire)
        Me.cdrMoteur.Controls.Add(Me.lblProc)
        Me.cdrMoteur.Controls.Add(Me.txtPuissance)
        Me.cdrMoteur.Controls.Add(Me.lblPuissance)
        Me.cdrMoteur.Controls.Add(Me.cmdOptions2)
        Me.cdrMoteur.Controls.Add(Me.cmdOptions1)
        Me.cdrMoteur.Controls.Add(Me.lblMoteur2)
        Me.cdrMoteur.Controls.Add(Me.cmdMoteur2)
        Me.cdrMoteur.Controls.Add(Me.lblMoteur1)
        Me.cdrMoteur.Controls.Add(Me.cmdMoteur1)
        Me.cdrMoteur.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cdrMoteur.Location = New System.Drawing.Point(14, 12)
        Me.cdrMoteur.Name = "cdrMoteur"
        Me.cdrMoteur.Size = New System.Drawing.Size(707, 115)
        Me.cdrMoteur.TabIndex = 19
        Me.cdrMoteur.TabStop = False
        Me.cdrMoteur.Text = "ENGINES"
        '
        'lblChemin2
        '
        Me.lblChemin2.AutoEllipsis = True
        Me.lblChemin2.ForeColor = System.Drawing.Color.Green
        Me.lblChemin2.Location = New System.Drawing.Point(80, 54)
        Me.lblChemin2.Name = "lblChemin2"
        Me.lblChemin2.Size = New System.Drawing.Size(513, 30)
        Me.lblChemin2.TabIndex = 6
        Me.lblChemin2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblChemin1
        '
        Me.lblChemin1.AutoEllipsis = True
        Me.lblChemin1.ForeColor = System.Drawing.Color.Blue
        Me.lblChemin1.Location = New System.Drawing.Point(80, 18)
        Me.lblChemin1.Name = "lblChemin1"
        Me.lblChemin1.Size = New System.Drawing.Size(513, 30)
        Me.lblChemin1.TabIndex = 2
        Me.lblChemin1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbPriorite
        '
        Me.cbPriorite.FormattingEnabled = True
        Me.cbPriorite.Items.AddRange(New Object() {"normal", "below", "idle"})
        Me.cbPriorite.Location = New System.Drawing.Point(460, 87)
        Me.cbPriorite.Name = "cbPriorite"
        Me.cbPriorite.Size = New System.Drawing.Size(76, 21)
        Me.cbPriorite.TabIndex = 16
        Me.cbPriorite.Text = "idle"
        '
        'lblPriorite
        '
        Me.lblPriorite.AutoSize = True
        Me.lblPriorite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriorite.Location = New System.Drawing.Point(398, 90)
        Me.lblPriorite.Name = "lblPriorite"
        Me.lblPriorite.Size = New System.Drawing.Size(44, 13)
        Me.lblPriorite.TabIndex = 15
        Me.lblPriorite.Text = "Priority :"
        Me.lblPriorite.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtContempt
        '
        Me.txtContempt.Location = New System.Drawing.Point(657, 87)
        Me.txtContempt.Name = "txtContempt"
        Me.txtContempt.Size = New System.Drawing.Size(28, 20)
        Me.txtContempt.TabIndex = 18
        Me.txtContempt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblContempt
        '
        Me.lblContempt.AutoSize = True
        Me.lblContempt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContempt.Location = New System.Drawing.Point(580, 90)
        Me.lblContempt.Name = "lblContempt"
        Me.lblContempt.Size = New System.Drawing.Size(58, 13)
        Me.lblContempt.TabIndex = 17
        Me.lblContempt.Text = "Contempt :"
        Me.lblContempt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMo
        '
        Me.lblMo.AutoSize = True
        Me.lblMo.Location = New System.Drawing.Point(325, 90)
        Me.lblMo.Name = "lblMo"
        Me.lblMo.Size = New System.Drawing.Size(23, 13)
        Me.lblMo.TabIndex = 14
        Me.lblMo.Text = "MB"
        '
        'txtMemoire
        '
        Me.txtMemoire.Location = New System.Drawing.Point(274, 87)
        Me.txtMemoire.Name = "txtMemoire"
        Me.txtMemoire.Size = New System.Drawing.Size(51, 20)
        Me.txtMemoire.TabIndex = 13
        Me.txtMemoire.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblMemoire
        '
        Me.lblMemoire.AutoSize = True
        Me.lblMemoire.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMemoire.Location = New System.Drawing.Point(204, 90)
        Me.lblMemoire.Name = "lblMemoire"
        Me.lblMemoire.Size = New System.Drawing.Size(50, 13)
        Me.lblMemoire.TabIndex = 12
        Me.lblMemoire.Text = "Memory :"
        Me.lblMemoire.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblProc
        '
        Me.lblProc.AutoSize = True
        Me.lblProc.Location = New System.Drawing.Point(132, 90)
        Me.lblProc.Name = "lblProc"
        Me.lblProc.Size = New System.Drawing.Size(42, 13)
        Me.lblProc.TabIndex = 11
        Me.lblProc.Text = "threads"
        '
        'txtPuissance
        '
        Me.txtPuissance.Location = New System.Drawing.Point(97, 87)
        Me.txtPuissance.Name = "txtPuissance"
        Me.txtPuissance.Size = New System.Drawing.Size(32, 20)
        Me.txtPuissance.TabIndex = 10
        Me.txtPuissance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPuissance
        '
        Me.lblPuissance.AutoSize = True
        Me.lblPuissance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPuissance.Location = New System.Drawing.Point(8, 90)
        Me.lblPuissance.Name = "lblPuissance"
        Me.lblPuissance.Size = New System.Drawing.Size(43, 13)
        Me.lblPuissance.TabIndex = 9
        Me.lblPuissance.Text = "Power :"
        Me.lblPuissance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdOptions2
        '
        Me.cmdOptions2.AutoSize = True
        Me.cmdOptions2.ForeColor = System.Drawing.Color.Green
        Me.cmdOptions2.Location = New System.Drawing.Point(637, 54)
        Me.cmdOptions2.Name = "cmdOptions2"
        Me.cmdOptions2.Size = New System.Drawing.Size(54, 30)
        Me.cmdOptions2.TabIndex = 8
        Me.cmdOptions2.Text = "options"
        Me.cmdOptions2.UseVisualStyleBackColor = True
        '
        'cmdOptions1
        '
        Me.cmdOptions1.AutoSize = True
        Me.cmdOptions1.ForeColor = System.Drawing.Color.Blue
        Me.cmdOptions1.Location = New System.Drawing.Point(637, 18)
        Me.cmdOptions1.Name = "cmdOptions1"
        Me.cmdOptions1.Size = New System.Drawing.Size(54, 30)
        Me.cmdOptions1.TabIndex = 4
        Me.cmdOptions1.Text = "options"
        Me.cmdOptions1.UseVisualStyleBackColor = True
        '
        'lblMoteur2
        '
        Me.lblMoteur2.AutoSize = True
        Me.lblMoteur2.ForeColor = System.Drawing.Color.Green
        Me.lblMoteur2.Location = New System.Drawing.Point(9, 63)
        Me.lblMoteur2.Name = "lblMoteur2"
        Me.lblMoteur2.Size = New System.Drawing.Size(55, 13)
        Me.lblMoteur2.TabIndex = 5
        Me.lblMoteur2.Text = "Engine 2 :"
        Me.lblMoteur2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdMoteur2
        '
        Me.cmdMoteur2.AutoSize = True
        Me.cmdMoteur2.ForeColor = System.Drawing.Color.Green
        Me.cmdMoteur2.Location = New System.Drawing.Point(599, 54)
        Me.cmdMoteur2.Name = "cmdMoteur2"
        Me.cmdMoteur2.Size = New System.Drawing.Size(32, 30)
        Me.cmdMoteur2.TabIndex = 7
        Me.cmdMoteur2.Text = "..."
        Me.cmdMoteur2.UseVisualStyleBackColor = True
        '
        'lblMoteur1
        '
        Me.lblMoteur1.AutoSize = True
        Me.lblMoteur1.ForeColor = System.Drawing.Color.Blue
        Me.lblMoteur1.Location = New System.Drawing.Point(9, 27)
        Me.lblMoteur1.Name = "lblMoteur1"
        Me.lblMoteur1.Size = New System.Drawing.Size(55, 13)
        Me.lblMoteur1.TabIndex = 1
        Me.lblMoteur1.Text = "Engine 1 :"
        Me.lblMoteur1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdMoteur1
        '
        Me.cmdMoteur1.AutoSize = True
        Me.cmdMoteur1.ForeColor = System.Drawing.Color.Blue
        Me.cmdMoteur1.Location = New System.Drawing.Point(599, 18)
        Me.cmdMoteur1.Name = "cmdMoteur1"
        Me.cmdMoteur1.Size = New System.Drawing.Size(32, 30)
        Me.cmdMoteur1.TabIndex = 3
        Me.cmdMoteur1.Text = "..."
        Me.cmdMoteur1.UseVisualStyleBackColor = True
        '
        'cdrPosition
        '
        Me.cdrPosition.Controls.Add(Me.cbFixee)
        Me.cdrPosition.Controls.Add(Me.lblFichier)
        Me.cdrPosition.Controls.Add(Me.lblEstimation)
        Me.cdrPosition.Controls.Add(Me.lblProgression)
        Me.cdrPosition.Controls.Add(Me.txtReprise)
        Me.cdrPosition.Controls.Add(Me.lblIndex)
        Me.cdrPosition.Controls.Add(Me.lblListe)
        Me.cdrPosition.Controls.Add(Me.cmdListe)
        Me.cdrPosition.Controls.Add(Me.lblSecondes)
        Me.cdrPosition.Controls.Add(Me.txtLimite)
        Me.cdrPosition.Controls.Add(Me.lblLimite)
        Me.cdrPosition.Location = New System.Drawing.Point(12, 132)
        Me.cdrPosition.Name = "cdrPosition"
        Me.cdrPosition.Size = New System.Drawing.Size(707, 84)
        Me.cdrPosition.TabIndex = 20
        Me.cdrPosition.TabStop = False
        Me.cdrPosition.Text = "PGN/EPD"
        '
        'cbFixee
        '
        Me.cbFixee.AutoSize = True
        Me.cbFixee.Location = New System.Drawing.Point(175, 57)
        Me.cbFixee.Name = "cbFixee"
        Me.cbFixee.Size = New System.Drawing.Size(51, 17)
        Me.cbFixee.TabIndex = 44
        Me.cbFixee.Text = "Fixed"
        Me.cbFixee.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbFixee.UseVisualStyleBackColor = True
        '
        'lblFichier
        '
        Me.lblFichier.AutoEllipsis = True
        Me.lblFichier.ForeColor = System.Drawing.Color.Red
        Me.lblFichier.Location = New System.Drawing.Point(83, 18)
        Me.lblFichier.Name = "lblFichier"
        Me.lblFichier.Size = New System.Drawing.Size(580, 30)
        Me.lblFichier.TabIndex = 20
        Me.lblFichier.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEstimation
        '
        Me.lblEstimation.AutoSize = True
        Me.lblEstimation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEstimation.Location = New System.Drawing.Point(460, 58)
        Me.lblEstimation.Name = "lblEstimation"
        Me.lblEstimation.Size = New System.Drawing.Size(31, 13)
        Me.lblEstimation.TabIndex = 28
        Me.lblEstimation.Text = "Est. :"
        '
        'lblProgression
        '
        Me.lblProgression.AutoSize = True
        Me.lblProgression.Location = New System.Drawing.Point(362, 58)
        Me.lblProgression.Name = "lblProgression"
        Me.lblProgression.Size = New System.Drawing.Size(41, 13)
        Me.lblProgression.TabIndex = 27
        Me.lblProgression.Text = "/0 (0%)"
        '
        'txtReprise
        '
        Me.txtReprise.Location = New System.Drawing.Point(301, 55)
        Me.txtReprise.Name = "txtReprise"
        Me.txtReprise.Size = New System.Drawing.Size(61, 20)
        Me.txtReprise.TabIndex = 26
        Me.txtReprise.Text = "1"
        Me.txtReprise.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblIndex
        '
        Me.lblIndex.AutoSize = True
        Me.lblIndex.Location = New System.Drawing.Point(250, 58)
        Me.lblIndex.Name = "lblIndex"
        Me.lblIndex.Size = New System.Drawing.Size(39, 13)
        Me.lblIndex.TabIndex = 25
        Me.lblIndex.Text = "Index :"
        Me.lblIndex.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblListe
        '
        Me.lblListe.AutoSize = True
        Me.lblListe.ForeColor = System.Drawing.Color.Red
        Me.lblListe.Location = New System.Drawing.Point(11, 27)
        Me.lblListe.Name = "lblListe"
        Me.lblListe.Size = New System.Drawing.Size(29, 13)
        Me.lblListe.TabIndex = 19
        Me.lblListe.Text = "List :"
        Me.lblListe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdListe
        '
        Me.cmdListe.AutoSize = True
        Me.cmdListe.ForeColor = System.Drawing.Color.Red
        Me.cmdListe.Location = New System.Drawing.Point(669, 18)
        Me.cmdListe.Name = "cmdListe"
        Me.cmdListe.Size = New System.Drawing.Size(32, 30)
        Me.cmdListe.TabIndex = 21
        Me.cmdListe.Text = "..."
        Me.cmdListe.UseVisualStyleBackColor = True
        '
        'lblSecondes
        '
        Me.lblSecondes.AutoSize = True
        Me.lblSecondes.Location = New System.Drawing.Point(133, 58)
        Me.lblSecondes.Name = "lblSecondes"
        Me.lblSecondes.Size = New System.Drawing.Size(24, 13)
        Me.lblSecondes.TabIndex = 24
        Me.lblSecondes.Text = "sec"
        '
        'txtLimite
        '
        Me.txtLimite.Location = New System.Drawing.Point(83, 55)
        Me.txtLimite.Name = "txtLimite"
        Me.txtLimite.Size = New System.Drawing.Size(50, 20)
        Me.txtLimite.TabIndex = 23
        Me.txtLimite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblLimite
        '
        Me.lblLimite.AutoSize = True
        Me.lblLimite.Location = New System.Drawing.Point(11, 58)
        Me.lblLimite.Name = "lblLimite"
        Me.lblLimite.Size = New System.Drawing.Size(53, 13)
        Me.lblLimite.TabIndex = 22
        Me.lblLimite.Text = "Duration :"
        Me.lblLimite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbArret
        '
        Me.cbArret.AutoSize = True
        Me.cbArret.Location = New System.Drawing.Point(633, 338)
        Me.cbArret.Name = "cbArret"
        Me.cbArret.Size = New System.Drawing.Size(74, 17)
        Me.cbArret.TabIndex = 53
        Me.cbArret.Text = "Shutdown"
        Me.cbArret.UseVisualStyleBackColor = True
        '
        'cbLog
        '
        Me.cbLog.AutoSize = True
        Me.cbLog.Checked = True
        Me.cbLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbLog.Location = New System.Drawing.Point(632, 307)
        Me.cbLog.Name = "cbLog"
        Me.cbLog.Size = New System.Drawing.Size(69, 17)
        Me.cbLog.TabIndex = 43
        Me.cbLog.Text = "Log stats"
        Me.cbLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbLog.UseVisualStyleBackColor = True
        '
        'cdrAnalyse
        '
        Me.cdrAnalyse.Controls.Add(Me.lblSyzygy2)
        Me.cdrAnalyse.Controls.Add(Me.lblPonder)
        Me.cdrAnalyse.Controls.Add(Me.lblInfos2)
        Me.cdrAnalyse.Controls.Add(Me.lblSyzygy1)
        Me.cdrAnalyse.Controls.Add(Me.lblInfos1)
        Me.cdrAnalyse.Controls.Add(Me.lblHash2)
        Me.cdrAnalyse.Controls.Add(Me.lblHash1)
        Me.cdrAnalyse.Controls.Add(Me.lblTB2)
        Me.cdrAnalyse.Controls.Add(Me.lblScore2)
        Me.cdrAnalyse.Controls.Add(Me.lblScore1)
        Me.cdrAnalyse.Controls.Add(Me.lblTB1)
        Me.cdrAnalyse.Controls.Add(Me.lblIdem)
        Me.cdrAnalyse.Controls.Add(Me.lblKnps2)
        Me.cdrAnalyse.Controls.Add(Me.lblKnps1)
        Me.cdrAnalyse.Controls.Add(Me.lblEval2)
        Me.cdrAnalyse.Controls.Add(Me.lblEval1)
        Me.cdrAnalyse.Controls.Add(Me.lblPartie)
        Me.cdrAnalyse.Controls.Add(Me.lblProfondeur2)
        Me.cdrAnalyse.Controls.Add(Me.lblProfondeur1)
        Me.cdrAnalyse.Controls.Add(Me.lblCoup2)
        Me.cdrAnalyse.Controls.Add(Me.lblCoup1)
        Me.cdrAnalyse.Controls.Add(Me.lblRestant)
        Me.cdrAnalyse.Controls.Add(Me.lblDernier2)
        Me.cdrAnalyse.Controls.Add(Me.lblDernier1)
        Me.cdrAnalyse.Controls.Add(Me.lblResultat2)
        Me.cdrAnalyse.Controls.Add(Me.lblResultat1)
        Me.cdrAnalyse.Controls.Add(Me.lblPosition)
        Me.cdrAnalyse.Location = New System.Drawing.Point(12, 222)
        Me.cdrAnalyse.Name = "cdrAnalyse"
        Me.cdrAnalyse.Size = New System.Drawing.Size(615, 172)
        Me.cdrAnalyse.TabIndex = 21
        Me.cdrAnalyse.TabStop = False
        Me.cdrAnalyse.Text = "ANALYSIS"
        '
        'lblSyzygy2
        '
        Me.lblSyzygy2.AutoSize = True
        Me.lblSyzygy2.ForeColor = System.Drawing.Color.Green
        Me.lblSyzygy2.Location = New System.Drawing.Point(371, 145)
        Me.lblSyzygy2.Name = "lblSyzygy2"
        Me.lblSyzygy2.Size = New System.Drawing.Size(55, 13)
        Me.lblSyzygy2.TabIndex = 57
        Me.lblSyzygy2.Text = "Syzygy 2 :"
        Me.lblSyzygy2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPonder
        '
        Me.lblPonder.Location = New System.Drawing.Point(259, 119)
        Me.lblPonder.Name = "lblPonder"
        Me.lblPonder.Size = New System.Drawing.Size(98, 20)
        Me.lblPonder.TabIndex = 55
        Me.lblPonder.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInfos2
        '
        Me.lblInfos2.AutoSize = True
        Me.lblInfos2.ForeColor = System.Drawing.Color.Green
        Me.lblInfos2.Location = New System.Drawing.Point(371, 119)
        Me.lblInfos2.Name = "lblInfos2"
        Me.lblInfos2.Size = New System.Drawing.Size(45, 13)
        Me.lblInfos2.TabIndex = 54
        Me.lblInfos2.Text = "Data 2 :"
        Me.lblInfos2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSyzygy1
        '
        Me.lblSyzygy1.AutoSize = True
        Me.lblSyzygy1.ForeColor = System.Drawing.Color.Blue
        Me.lblSyzygy1.Location = New System.Drawing.Point(8, 145)
        Me.lblSyzygy1.Name = "lblSyzygy1"
        Me.lblSyzygy1.Size = New System.Drawing.Size(55, 13)
        Me.lblSyzygy1.TabIndex = 56
        Me.lblSyzygy1.Text = "Syzygy 1 :"
        Me.lblSyzygy1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblInfos1
        '
        Me.lblInfos1.AutoSize = True
        Me.lblInfos1.ForeColor = System.Drawing.Color.Blue
        Me.lblInfos1.Location = New System.Drawing.Point(9, 119)
        Me.lblInfos1.Name = "lblInfos1"
        Me.lblInfos1.Size = New System.Drawing.Size(45, 13)
        Me.lblInfos1.TabIndex = 53
        Me.lblInfos1.Text = "Data 1 :"
        Me.lblInfos1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblHash2
        '
        Me.lblHash2.ForeColor = System.Drawing.Color.Green
        Me.lblHash2.Location = New System.Drawing.Point(451, 119)
        Me.lblHash2.Name = "lblHash2"
        Me.lblHash2.Size = New System.Drawing.Size(62, 20)
        Me.lblHash2.TabIndex = 50
        '
        'lblHash1
        '
        Me.lblHash1.ForeColor = System.Drawing.Color.Blue
        Me.lblHash1.Location = New System.Drawing.Point(87, 119)
        Me.lblHash1.Name = "lblHash1"
        Me.lblHash1.Size = New System.Drawing.Size(65, 20)
        Me.lblHash1.TabIndex = 49
        '
        'lblTB2
        '
        Me.lblTB2.ForeColor = System.Drawing.Color.Green
        Me.lblTB2.Location = New System.Drawing.Point(451, 145)
        Me.lblTB2.Name = "lblTB2"
        Me.lblTB2.Size = New System.Drawing.Size(160, 20)
        Me.lblTB2.TabIndex = 52
        '
        'lblScore2
        '
        Me.lblScore2.ForeColor = System.Drawing.Color.Green
        Me.lblScore2.Location = New System.Drawing.Point(451, 91)
        Me.lblScore2.Name = "lblScore2"
        Me.lblScore2.Size = New System.Drawing.Size(75, 20)
        Me.lblScore2.TabIndex = 48
        '
        'lblScore1
        '
        Me.lblScore1.ForeColor = System.Drawing.Color.Blue
        Me.lblScore1.Location = New System.Drawing.Point(87, 91)
        Me.lblScore1.Name = "lblScore1"
        Me.lblScore1.Size = New System.Drawing.Size(75, 20)
        Me.lblScore1.TabIndex = 47
        '
        'lblTB1
        '
        Me.lblTB1.ForeColor = System.Drawing.Color.Blue
        Me.lblTB1.Location = New System.Drawing.Point(87, 145)
        Me.lblTB1.Name = "lblTB1"
        Me.lblTB1.Size = New System.Drawing.Size(160, 20)
        Me.lblTB1.TabIndex = 51
        '
        'lblIdem
        '
        Me.lblIdem.Location = New System.Drawing.Point(259, 91)
        Me.lblIdem.Name = "lblIdem"
        Me.lblIdem.Size = New System.Drawing.Size(98, 20)
        Me.lblIdem.TabIndex = 46
        Me.lblIdem.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblKnps2
        '
        Me.lblKnps2.ForeColor = System.Drawing.Color.Green
        Me.lblKnps2.Location = New System.Drawing.Point(519, 119)
        Me.lblKnps2.Name = "lblKnps2"
        Me.lblKnps2.Size = New System.Drawing.Size(90, 20)
        Me.lblKnps2.TabIndex = 45
        '
        'lblKnps1
        '
        Me.lblKnps1.ForeColor = System.Drawing.Color.Blue
        Me.lblKnps1.Location = New System.Drawing.Point(158, 119)
        Me.lblKnps1.Name = "lblKnps1"
        Me.lblKnps1.Size = New System.Drawing.Size(90, 20)
        Me.lblKnps1.TabIndex = 44
        '
        'lblEval2
        '
        Me.lblEval2.AutoSize = True
        Me.lblEval2.ForeColor = System.Drawing.Color.Green
        Me.lblEval2.Location = New System.Drawing.Point(371, 91)
        Me.lblEval2.Name = "lblEval2"
        Me.lblEval2.Size = New System.Drawing.Size(61, 13)
        Me.lblEval2.TabIndex = 43
        Me.lblEval2.Text = "Evaluat. 2 :"
        Me.lblEval2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEval1
        '
        Me.lblEval1.AutoSize = True
        Me.lblEval1.ForeColor = System.Drawing.Color.Blue
        Me.lblEval1.Location = New System.Drawing.Point(9, 91)
        Me.lblEval1.Name = "lblEval1"
        Me.lblEval1.Size = New System.Drawing.Size(61, 13)
        Me.lblEval1.TabIndex = 40
        Me.lblEval1.Text = "Evaluat. 1 :"
        Me.lblEval1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPartie
        '
        Me.lblPartie.AutoEllipsis = True
        Me.lblPartie.ForeColor = System.Drawing.Color.Red
        Me.lblPartie.Location = New System.Drawing.Point(87, 16)
        Me.lblPartie.Name = "lblPartie"
        Me.lblPartie.Size = New System.Drawing.Size(522, 45)
        Me.lblPartie.TabIndex = 30
        '
        'lblProfondeur2
        '
        Me.lblProfondeur2.ForeColor = System.Drawing.Color.Green
        Me.lblProfondeur2.Location = New System.Drawing.Point(534, 91)
        Me.lblProfondeur2.Name = "lblProfondeur2"
        Me.lblProfondeur2.Size = New System.Drawing.Size(75, 20)
        Me.lblProfondeur2.TabIndex = 38
        '
        'lblProfondeur1
        '
        Me.lblProfondeur1.ForeColor = System.Drawing.Color.Blue
        Me.lblProfondeur1.Location = New System.Drawing.Point(173, 90)
        Me.lblProfondeur1.Name = "lblProfondeur1"
        Me.lblProfondeur1.Size = New System.Drawing.Size(75, 20)
        Me.lblProfondeur1.TabIndex = 33
        '
        'lblCoup2
        '
        Me.lblCoup2.ForeColor = System.Drawing.Color.Green
        Me.lblCoup2.Location = New System.Drawing.Point(451, 62)
        Me.lblCoup2.Name = "lblCoup2"
        Me.lblCoup2.Size = New System.Drawing.Size(75, 20)
        Me.lblCoup2.TabIndex = 37
        '
        'lblCoup1
        '
        Me.lblCoup1.ForeColor = System.Drawing.Color.Blue
        Me.lblCoup1.Location = New System.Drawing.Point(87, 62)
        Me.lblCoup1.Name = "lblCoup1"
        Me.lblCoup1.Size = New System.Drawing.Size(75, 20)
        Me.lblCoup1.TabIndex = 32
        '
        'lblRestant
        '
        Me.lblRestant.BackColor = System.Drawing.SystemColors.Control
        Me.lblRestant.Location = New System.Drawing.Point(259, 62)
        Me.lblRestant.Name = "lblRestant"
        Me.lblRestant.Size = New System.Drawing.Size(98, 20)
        Me.lblRestant.TabIndex = 35
        Me.lblRestant.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDernier2
        '
        Me.lblDernier2.ForeColor = System.Drawing.Color.Green
        Me.lblDernier2.Location = New System.Drawing.Point(534, 62)
        Me.lblDernier2.Name = "lblDernier2"
        Me.lblDernier2.Size = New System.Drawing.Size(75, 20)
        Me.lblDernier2.TabIndex = 39
        '
        'lblDernier1
        '
        Me.lblDernier1.ForeColor = System.Drawing.Color.Blue
        Me.lblDernier1.Location = New System.Drawing.Point(170, 62)
        Me.lblDernier1.Name = "lblDernier1"
        Me.lblDernier1.Size = New System.Drawing.Size(75, 20)
        Me.lblDernier1.TabIndex = 34
        '
        'lblResultat2
        '
        Me.lblResultat2.AutoSize = True
        Me.lblResultat2.ForeColor = System.Drawing.Color.Green
        Me.lblResultat2.Location = New System.Drawing.Point(371, 63)
        Me.lblResultat2.Name = "lblResultat2"
        Me.lblResultat2.Size = New System.Drawing.Size(57, 13)
        Me.lblResultat2.TabIndex = 36
        Me.lblResultat2.Text = "Results 2 :"
        Me.lblResultat2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblResultat1
        '
        Me.lblResultat1.AutoSize = True
        Me.lblResultat1.ForeColor = System.Drawing.Color.Blue
        Me.lblResultat1.Location = New System.Drawing.Point(9, 63)
        Me.lblResultat1.Name = "lblResultat1"
        Me.lblResultat1.Size = New System.Drawing.Size(57, 13)
        Me.lblResultat1.TabIndex = 31
        Me.lblResultat1.Text = "Results 1 :"
        Me.lblResultat1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPosition
        '
        Me.lblPosition.AutoSize = True
        Me.lblPosition.ForeColor = System.Drawing.Color.Red
        Me.lblPosition.Location = New System.Drawing.Point(9, 17)
        Me.lblPosition.Name = "lblPosition"
        Me.lblPosition.Size = New System.Drawing.Size(50, 13)
        Me.lblPosition.TabIndex = 29
        Me.lblPosition.Text = "Position :"
        Me.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'timerAnalyse
        '
        Me.timerAnalyse.Interval = 24000
        '
        'timerProgression
        '
        Me.timerProgression.Interval = 1000
        '
        'cbDiffere
        '
        Me.cbDiffere.AutoSize = True
        Me.cbDiffere.Location = New System.Drawing.Point(633, 267)
        Me.cbDiffere.Name = "cbDiffere"
        Me.cbDiffere.Size = New System.Drawing.Size(65, 30)
        Me.cbDiffere.TabIndex = 48
        Me.cbDiffere.Text = "Delayed" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "start"
        Me.cbDiffere.UseVisualStyleBackColor = True
        '
        'pbEchiquier
        '
        Me.pbEchiquier.BackColor = System.Drawing.SystemColors.Control
        Me.pbEchiquier.Location = New System.Drawing.Point(734, 9)
        Me.pbEchiquier.Name = "pbEchiquier"
        Me.pbEchiquier.Size = New System.Drawing.Size(355, 355)
        Me.pbEchiquier.TabIndex = 49
        Me.pbEchiquier.TabStop = False
        '
        'pbMateriel
        '
        Me.pbMateriel.BackColor = System.Drawing.SystemColors.Control
        Me.pbMateriel.Location = New System.Drawing.Point(734, 370)
        Me.pbMateriel.Name = "pbMateriel"
        Me.pbMateriel.Size = New System.Drawing.Size(355, 25)
        Me.pbMateriel.TabIndex = 50
        Me.pbMateriel.TabStop = False
        '
        'frmPrincipale
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(733, 401)
        Me.Controls.Add(Me.cbArret)
        Me.Controls.Add(Me.pbMateriel)
        Me.Controls.Add(Me.pbEchiquier)
        Me.Controls.Add(Me.cbDiffere)
        Me.Controls.Add(Me.cbLog)
        Me.Controls.Add(Me.cdrAnalyse)
        Me.Controls.Add(Me.cdrPosition)
        Me.Controls.Add(Me.cdrMoteur)
        Me.Controls.Add(Me.cmdAnalyser)
        Me.Controls.Add(Me.cmdArreter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmPrincipale"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ANALYSEUR PGN"
        Me.cdrMoteur.ResumeLayout(False)
        Me.cdrMoteur.PerformLayout()
        Me.cdrPosition.ResumeLayout(False)
        Me.cdrPosition.PerformLayout()
        Me.cdrAnalyse.ResumeLayout(False)
        Me.cdrAnalyse.PerformLayout()
        CType(Me.pbEchiquier, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMateriel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdAnalyser As System.Windows.Forms.Button
    Friend WithEvents cmdArreter As System.Windows.Forms.Button
    Friend WithEvents dlgImport As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cdrMoteur As System.Windows.Forms.GroupBox
    Friend WithEvents lblProc As System.Windows.Forms.Label
    Friend WithEvents txtPuissance As System.Windows.Forms.TextBox
    Friend WithEvents lblPuissance As System.Windows.Forms.Label
    Friend WithEvents cmdOptions2 As System.Windows.Forms.Button
    Friend WithEvents cmdOptions1 As System.Windows.Forms.Button
    Friend WithEvents lblMoteur2 As System.Windows.Forms.Label
    Friend WithEvents cmdMoteur2 As System.Windows.Forms.Button
    Friend WithEvents lblMoteur1 As System.Windows.Forms.Label
    Friend WithEvents cmdMoteur1 As System.Windows.Forms.Button
    Friend WithEvents cdrPosition As System.Windows.Forms.GroupBox
    Friend WithEvents lblListe As System.Windows.Forms.Label
    Friend WithEvents cmdListe As System.Windows.Forms.Button
    Friend WithEvents lblSecondes As System.Windows.Forms.Label
    Friend WithEvents txtLimite As System.Windows.Forms.TextBox
    Friend WithEvents lblLimite As System.Windows.Forms.Label
    Friend WithEvents lblMo As System.Windows.Forms.Label
    Friend WithEvents txtMemoire As System.Windows.Forms.TextBox
    Friend WithEvents lblMemoire As System.Windows.Forms.Label
    Friend WithEvents lblPriorite As System.Windows.Forms.Label
    Friend WithEvents txtContempt As System.Windows.Forms.TextBox
    Friend WithEvents lblContempt As System.Windows.Forms.Label
    Friend WithEvents cdrAnalyse As System.Windows.Forms.GroupBox
    Friend WithEvents lblResultat2 As System.Windows.Forms.Label
    Friend WithEvents lblResultat1 As System.Windows.Forms.Label
    Friend WithEvents lblPosition As System.Windows.Forms.Label
    Friend WithEvents timerAnalyse As System.Windows.Forms.Timer
    Friend WithEvents txtReprise As System.Windows.Forms.TextBox
    Friend WithEvents lblIndex As System.Windows.Forms.Label
    Friend WithEvents lblProgression As System.Windows.Forms.Label
    Friend WithEvents lblEstimation As System.Windows.Forms.Label
    Friend WithEvents dlgExport As System.Windows.Forms.SaveFileDialog
    Friend WithEvents timerProgression As System.Windows.Forms.Timer
    Friend WithEvents lblDernier2 As System.Windows.Forms.Label
    Friend WithEvents lblDernier1 As System.Windows.Forms.Label
    Friend WithEvents lblRestant As System.Windows.Forms.Label
    Friend WithEvents cbPriorite As System.Windows.Forms.ComboBox
    Friend WithEvents lblCoup2 As System.Windows.Forms.Label
    Friend WithEvents lblCoup1 As System.Windows.Forms.Label
    Friend WithEvents lblProfondeur2 As System.Windows.Forms.Label
    Friend WithEvents lblProfondeur1 As System.Windows.Forms.Label
    Friend WithEvents lblPartie As System.Windows.Forms.Label
    Friend WithEvents lblFichier As System.Windows.Forms.Label
    Friend WithEvents lblChemin2 As System.Windows.Forms.Label
    Friend WithEvents lblChemin1 As System.Windows.Forms.Label
    Friend WithEvents lblEval2 As System.Windows.Forms.Label
    Friend WithEvents lblEval1 As System.Windows.Forms.Label
    Friend WithEvents lblKnps2 As System.Windows.Forms.Label
    Friend WithEvents lblKnps1 As System.Windows.Forms.Label
    Friend WithEvents cbLog As System.Windows.Forms.CheckBox
    Friend WithEvents lblIdem As System.Windows.Forms.Label
    Friend WithEvents lblScore2 As System.Windows.Forms.Label
    Friend WithEvents lblScore1 As System.Windows.Forms.Label
    Friend WithEvents lblTB2 As System.Windows.Forms.Label
    Friend WithEvents lblTB1 As System.Windows.Forms.Label
    Friend WithEvents lblHash2 As System.Windows.Forms.Label
    Friend WithEvents lblHash1 As System.Windows.Forms.Label
    Friend WithEvents lblInfos2 As System.Windows.Forms.Label
    Friend WithEvents lblInfos1 As System.Windows.Forms.Label
    Friend WithEvents lblPonder As System.Windows.Forms.Label
    Friend WithEvents cbDiffere As System.Windows.Forms.CheckBox
    Friend WithEvents pbEchiquier As System.Windows.Forms.PictureBox
    Friend WithEvents pbMateriel As System.Windows.Forms.PictureBox
    Friend WithEvents cbFixee As System.Windows.Forms.CheckBox
    Friend WithEvents cbArret As System.Windows.Forms.CheckBox
    Friend WithEvents lblSyzygy2 As System.Windows.Forms.Label
    Friend WithEvents lblSyzygy1 As System.Windows.Forms.Label

End Class
