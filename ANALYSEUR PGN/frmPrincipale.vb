Public Class frmPrincipale

    Private analyseEnCours As Boolean

    Private tabMemCoups1(1) As String
    Private tabMemCoups2(1) As String
    Private nbAccords As Integer
    Private nbChangements As Integer
    Const rallonge = 60 'sec

    Private debAnalyse As Date
    Private matDetecte As Boolean
    Private matMoteur1 As Boolean
    Private matMoteur2 As Boolean

    Private memSortie1 As String
    Private memSortie2 As String

    Private puissance As Integer

    Private nbTransitions As Integer
    Private tpsTransition As Single
    Private debTransition As Date

    Private tempsCumule As Integer
    Private profondeurLimitee As Boolean

    'location puissance calcul
    Private depart As Date

    Private Sub frmPrincipale_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        If Me.Width < 1110 Then
            Me.Width = 1110
            pbEchiquier.Refresh()
        ElseIf Me.Width > 749 Then
            Me.Width = 749
        End If
    End Sub

    Private Sub cdrMoteur_DoubleClick(sender As Object, e As EventArgs) Handles cdrMoteur.DoubleClick
        If Me.Width = 749 Then
            Me.Width = 1110
            pbEchiquier.Refresh()
        ElseIf Me.Width = 1110 Then
            Me.Width = 749
        End If
    End Sub

    Private Sub cdrPosition_DoubleClick(sender As Object, e As EventArgs) Handles cdrPosition.DoubleClick
        If Me.Width = 749 Then
            Me.Width = 1110
            pbEchiquier.Refresh()
        ElseIf Me.Width = 1110 Then
            Me.Width = 749
        End If
    End Sub

    Private Sub cdrAnalyse_DoubleClick(sender As Object, e As EventArgs) Handles cdrAnalyse.DoubleClick
        If Me.Width = 749 Then
            Me.Width = 1110
            pbEchiquier.Refresh()
        ElseIf Me.Width = 1110 Then
            Me.Width = 749
        End If
    End Sub

    Private Sub pbEchiquier_Paint(sender As Object, e As PaintEventArgs) Handles pbEchiquier.Paint
        If Not IsNothing(tabEPD) And lblPartie.Text <> "" Then
            If 0 <= pos And pos <= UBound(tabEPD) Then
                echiquier(tabEPD(pos), e, lblCoup1.Tag, lblCoup2.Tag)
                pbMateriel.Refresh()
            End If
        End If
    End Sub

    Private Sub pbMateriel_Paint(sender As Object, e As PaintEventArgs) Handles pbMateriel.Paint
        If Not IsNothing(tabEPD) And lblPartie.Text <> "" Then
            If 0 <= pos And pos <= UBound(tabEPD) Then
                echiquier_differences(tabEPD(pos), e)
            End If
        End If
    End Sub

    Private Sub frmPrincipale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim chaine As String, tabChaine() As String, tabTmp() As String, i As Integer

        profondeurLimitee = True
        cbFixee.Enabled = False
        lblSecondes.Text = ""

        Me.Width = 749
        lblHash1.AutoSize = True
        lblHash2.AutoSize = True
        lblTB1.AutoSize = True
        lblTB2.AutoSize = True
        pbEchiquier.BackgroundImage = Image.FromFile("BMP\echiquier.bmp")
        pbMateriel.BackColor = Color.FromName("control")

        'on charge les dernières valeurs utilisées
        If My.Computer.FileSystem.FileExists(My.Computer.Name & ".ini") Then
            chaine = My.Computer.FileSystem.ReadAllText(My.Computer.Name & ".ini")
            If chaine <> "" And InStr(chaine, vbCrLf) > 0 Then
                tabChaine = Split(chaine, vbCrLf)
                For i = 0 To UBound(tabChaine)
                    If tabChaine(i) <> "" And InStr(tabChaine(i), " = ") > 0 Then
                        tabTmp = Split(tabChaine(i), " = ")
                        If tabTmp(0) <> "" And tabTmp(1) <> "" Then
                            For Each ctrl In Me.cdrMoteur.Controls
                                If ctrl.name = tabTmp(0) Then
                                    If TypeOf ctrl Is Button Then
                                        ctrl.tag = tabTmp(1) 'chemin notepad++
                                    Else
                                        ctrl.text = tabTmp(1)
                                    End If
                                    Exit For
                                End If
                                Application.DoEvents()
                            Next

                            For Each ctrl In Me.cdrPosition.Controls
                                If ctrl.name = tabTmp(0) Then
                                    ctrl.text = tabTmp(1)
                                    Exit For
                                End If
                                Application.DoEvents()
                            Next

                            For Each ctrl In Me.cdrAnalyse.Controls
                                If ctrl.name = tabTmp(0) Then
                                    ctrl.text = tabTmp(1)
                                    Exit For
                                End If
                                Application.DoEvents()
                            Next

                            For Each ctrl In Me.Controls
                                If ctrl.name = tabTmp(0) Then
                                    ctrl.text = tabTmp(1)
                                    Exit For
                                End If
                                Application.DoEvents()
                            Next

                        End If
                    End If
                    Application.DoEvents()
                Next
            End If

            'limitation puissance vs nb coeurs cpu
            If lblChemin2.Text <> "" Then
                puissance = Int(cpu() / 2)
            Else
                puissance = cpu()
            End If
            If txtPuissance.Text > puissance Then
                If InStr(lblChemin1.Text, "mpiexec.exe") = 0 Then
                    txtPuissance.Text = Format(puissance)
                End If
            End If

            'maintenir ratio puissance * durée
            txtPuissance.Tag = txtPuissance.Text
            If InStr(UCase(txtLimite.Text), "D") > 0 Then
                txtLimite.Tag = txtLimite.Text
            Else
                profondeurLimitee = False
                cbFixee.Enabled = True
                txtLimite.Tag = Format(Val(txtPuissance.Tag) * Val(txtLimite.Text))
                lblSecondes.Text = "sec"
            End If
        Else
            majConfig()
        End If

        'initialisation
        nbTransitions = 1
        tpsTransition = 5

        cmdAnalyser.Enabled = False
        cbDiffere.Enabled = False
        cmdArreter.Enabled = False
    End Sub

    Private Sub frmPrincipale_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        majConfig()

        cmdArreter_Click(sender, e)

        majDate()

        moteur1 = Nothing
        moteur2 = Nothing

        entree1 = Nothing
        entree2 = Nothing

        erreur1 = Nothing
        erreur2 = Nothing
    End Sub

    Private Sub cmdMoteur1_Click(sender As Object, e As EventArgs) Handles cmdMoteur1.Click
        dlgImport.FileName = ""
        dlgImport.InitialDirectory = Environment.CurrentDirectory
        dlgImport.Filter = "EXE engine (*.exe)|*.exe"
        dlgImport.ShowDialog()
        If dlgImport.FileName = "" Then
            Exit Sub
        End If

        lblChemin1.Text = dlgImport.FileName
    End Sub

    Private Sub cmdMoteur2_Click(sender As Object, e As EventArgs) Handles cmdMoteur2.Click
        dlgImport.FileName = ""
        dlgImport.InitialDirectory = Environment.CurrentDirectory
        dlgImport.Filter = "EXE engine (*.exe)|*.exe"
        dlgImport.ShowDialog()
        If dlgImport.FileName = "" Then
            Exit Sub
        End If

        lblChemin2.Text = dlgImport.FileName
    End Sub

    Private Sub cmdOptions1_Click(sender As Object, e As EventArgs) Handles cmdOptions1.Click
        Dim chaine As String

        If cmdOptions1.Tag = Nothing Then
            dlgImport.FileName = ""
            dlgImport.InitialDirectory = Environment.CurrentDirectory
            dlgImport.Filter = "Text editor (*.exe)|*.exe"
            dlgImport.ShowDialog()
            If dlgImport.FileName = "" Then
                Exit Sub
            End If
            cmdOptions1.Tag = dlgImport.FileName
        End If

        chaine = """" & cmdOptions1.Tag & """" & " "
        chaine = chaine & """" & Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf("\") + 1), ".exe", ".txt") & """"

        Shell(chaine, AppWinStyle.Hide, False)
    End Sub

    Private Sub cmdOptions2_Click(sender As Object, e As EventArgs) Handles cmdOptions2.Click
        Dim chaine As String

        If cmdOptions1.Tag = Nothing Then
            dlgImport.FileName = ""
            dlgImport.InitialDirectory = Environment.CurrentDirectory
            dlgImport.Filter = "Text editor (*.exe)|*.exe"
            dlgImport.ShowDialog()
            If dlgImport.FileName = "" Then
                Exit Sub
            End If
            cmdOptions1.Tag = dlgImport.FileName
        End If

        chaine = """" & cmdOptions1.Tag & """" & " "
        chaine = chaine & """" & Replace(lblChemin2.Text.Substring(lblChemin2.Text.LastIndexOf("\") + 1), ".exe", ".txt") & """"

        Shell(chaine, AppWinStyle.Hide, False)
    End Sub

    Private Sub txtPuissance_TextChanged(sender As Object, e As EventArgs) Handles txtPuissance.TextChanged
        If Val(txtPuissance.Text) > puissance And puissance > 0 Then
            If InStr(lblChemin1.Text, "mpiexec.exe") = 0 Then
                txtPuissance.Text = Format(puissance)
            End If
        End If

        'maintenir le ratio cpu * duree
        If Not txtPuissance.Tag Is Nothing And Not txtLimite.Text Is Nothing And Not profondeurLimitee Then
            If txtPuissance.Tag <> txtPuissance.Text Then
                txtLimite.Text = Format(Int(Val(txtLimite.Tag) / Val(txtPuissance.Text)))
            End If
        End If

        txtPuissance.Tag = txtPuissance.Text
    End Sub

    Private Sub txtLimite_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLimite.KeyUp
        'détecter s'il y a un "D" dans txtLimite
        If UCase(gauche(txtLimite.Text, 1)) = "D" Then
            profondeurLimitee = True
            cbFixee.Enabled = False
            txtLimite.Text = Replace(txtLimite.Text, "d", "D")
            txtLimite.Tag = txtLimite.Text
            If tempsCumule = 0 Then
                lblEstimation.Text = "Est. :"
            Else
                lblEstimation.Text = estimation(tempsCumule / (pos - Val(txtReprise.Tag)), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
            End If
            lblSecondes.Text = ""
        Else
            profondeurLimitee = False
            cbFixee.Enabled = True
            If IsNumeric(txtPuissance.Tag) And IsNumeric(txtLimite.Text) Then
                txtLimite.Tag = Format(Val(txtPuissance.Tag) * Val(txtLimite.Text))
            End If
            If tempsCumule = 0 Then
                lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), Val(txtLimite.Text) / 2, tpsTransition / nbTransitions, "Est.")
            Else
                lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
            End If
            lblSecondes.Text = "sec"
        End If

        majConfig()
    End Sub

    Private Sub txtReprise_KeyUp(sender As Object, e As KeyEventArgs) Handles txtReprise.KeyUp
        If profondeurLimitee Then
            If tempsCumule = 0 Then
                lblEstimation.Text = "Est. :"
            Else
                lblEstimation.Text = estimation(tempsCumule / (pos - Val(txtReprise.Tag)), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
            End If
        Else
            If tempsCumule = 0 Then
                lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), Val(txtLimite.Text) / 2, tpsTransition / nbTransitions, "Est.")
            Else
                lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
            End If
            majConfig()
        End If
    End Sub

    Private Sub cmdListe_Click(sender As Object, e As EventArgs) Handles cmdListe.Click
        Dim chaine As String, rep As String

        dlgImport.FileName = ""
        dlgImport.InitialDirectory = Environment.CurrentDirectory
        
        If lblChemin2.Text <> "" Or profondeurLimitee Then
            dlgImport.Filter = "PGN file (*.pgn)|*.pgn|EPD file (*.epd)|*.epd"
        Else
            dlgImport.Filter = "EPD file (*.epd)|*.epd|PGN file (*.pgn)|*.pgn"
        End If

        dlgImport.ShowDialog()
        If dlgImport.FileName = "" Then
            Exit Sub
        End If

        lblFichier.Text = dlgImport.FileName
        rep = ""
        mode = ""
        If extensionFichier(lblFichier.Text) = ".pgn" Then
            mode = ".pgn"
            lblFichier.Text = chargerPGN(lblFichier.Text)
            chaine = Replace(lblFichier.Text, mode, ".epd")
            'pour échiquier, si le fichier epd correspondant au fichier pgn n'existe pas
            If Not My.Computer.FileSystem.FileExists(chaine) Then
                'on le crée
                pgnEPD(lblFichier.Text)
            End If
            chargerEPD(chaine)

        ElseIf extensionFichier(lblFichier.Text) = ".epd" Then
            mode = ".epd"
            chargerEPD(lblFichier.Text)
        End If
        rep = Replace(lblFichier.Text, mode, ".rep")

        If My.Computer.FileSystem.FileExists(rep) Then
            chaine = My.Computer.FileSystem.ReadAllText(rep)
            idem = UBound(Split(chaine, ") - ("))
            If chaine.LastIndexOf(") ") > 0 Then
                If Len(gauche(chaine, chaine.LastIndexOf(") "))) >= 5 Then
                    chaine = droite(gauche(chaine, chaine.LastIndexOf(") ")), 5)
                    If IsNumeric(chaine) Then
                        If mode = ".pgn" Then
                            If Val(chaine) < tabPositions.Length Then
                                txtReprise.Text = Format(Val(chaine) + 1, "0")
                                lblIdem.Tag = Format(idem / Val(chaine), "0%")
                            Else
                                txtReprise.Text = "1"
                                idem = 0
                                If MsgBox("All positions have already been analyzed." & vbCrLf & "Do you want to delete the REP file ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                    My.Computer.FileSystem.DeleteFile(rep)
                                End If
                            End If
                        ElseIf mode = ".epd" Then
                            If Val(chaine) < tabEPD.Length Then
                                txtReprise.Text = Format(Val(chaine) + 1, "0")
                                lblIdem.Tag = Format(idem / Val(chaine), "0%")
                            Else
                                txtReprise.Text = "1"
                                idem = 0
                                If MsgBox("All positions have already been analyzed." & vbCrLf & "Do you want to delete the REP file ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                    My.Computer.FileSystem.DeleteFile(rep)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Else
            txtReprise.Text = "1"
            idem = 0
        End If

        If mode = ".pgn" Then
            lblProgression.Text = "/" & tabPositions.Length & " (" & Format(Val(txtReprise.Text) / tabPositions.Length, "0%") & ")"
        ElseIf mode = ".epd" Then
            lblProgression.Text = "/" & tabEPD.Length & " (" & Format(Val(txtReprise.Text) / tabEPD.Length, "0%") & ")"
        End If

        'estimation du temps d'analyse
        If profondeurLimitee Then
            If tempsCumule = 0 Then
                lblEstimation.Text = "Est. :"
            Else
                lblEstimation.Text = estimation(tempsCumule / (pos - Val(txtReprise.Tag)), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
            End If
        Else
            If tempsCumule = 0 Then
                lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), Val(txtLimite.Text) / 2, tpsTransition / nbTransitions, "Est.")
            Else
                lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
            End If
        End If

        cmdAnalyser.Enabled = True
        cbDiffere.Enabled = True
        cmdArreter.Enabled = False
    End Sub

    Private Sub cmdAnalyser_Click(sender As Object, e As EventArgs) Handles cmdAnalyser.Click
        Dim demarrage As Date

        If InStr(lblChemin1.Text, "mpiexec.exe") > 0 Then
            Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        Else
            Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Normal
        End If

        If cbDiffere.CheckState = CheckState.Checked Then
            demarrage = DateAdd(DateInterval.Minute, Int(InputBox("In how long (min) ?")), Now)
            Me.Tag = Me.Text
            Me.Text = "The analysis will start at " & Format(demarrage, "ddd dd MMM - HH' h 'mm' m 'ss")
            While Now < demarrage
                Threading.Thread.Sleep(1000) '10 sec
                Application.DoEvents()
            End While
            cbDiffere.CheckState = CheckState.Unchecked
            Me.Text = Me.Tag
        End If

        'location puissance calcul
        lblFichier.Tag = nomFichier(lblFichier.Text)
        lblFichier.Tag = Replace(lblFichier.Tag, extensionFichier(lblFichier.Tag), ".log")
        depart = Now()
        My.Computer.FileSystem.WriteAllText(lblFichier.Tag, Format(depart, "dd/MM/yyyy HH:mm:ss") & " > ", True)

        majConfig()

        verrouillage()
        Me.Tag = Me.Text
        Me.Text = "Loading engine 1..."
        chargerMoteur1(lblChemin1.Text, cbPriorite.Text, txtPuissance.Text, txtMemoire.Text, txtContempt.Text)
        If lblChemin2.Text <> "" Then
            Me.Text = "Loading engine 2..."
            chargerMoteur2(lblChemin2.Text, cbPriorite.Text, txtPuissance.Text, txtMemoire.Text, txtContempt.Text)
        End If
        Me.Text = Me.Tag

        'analyses
        analyseEnCours = False
        pos = Val(txtReprise.Text) - 1
        txtReprise.Tag = pos
        tempsCumule = 0

        timerAnalyse.Interval = 1000 'on se laisse 1 sec pour sortir de là
        timerAnalyse.Enabled = True
        cmdArreter.Enabled = True

        debTransition = Now()
    End Sub

    Private Sub cmdArreter_Click(sender As Object, e As EventArgs) Handles cmdArreter.Click
        On Error Resume Next

        entree1.WriteLine("stop")
        If lblChemin2.Text <> "" Then
            entree2.WriteLine("stop")
        End If

        entree1.WriteLine("quit")
        If lblChemin2.Text <> "" Then
            entree2.WriteLine("quit")
        End If

        moteur1.CancelOutputRead()
        If lblChemin2.Text <> "" Then
            moteur2.CancelOutputRead()
        End If

        moteur1.Close()
        If lblChemin2.Text <> "" Then
            moteur2.Close()
        End If

        entree1.Close()
        If lblChemin2.Text <> "" Then
            entree2.Close()
        End If

        erreur1.Close()
        If lblChemin2.Text <> "" Then
            erreur2.Close()
        End If

        analyseEnCours = False
        timerAnalyse.Enabled = False
        timerProgression.Enabled = False

        lblPartie.Text = ""
        lblPosition.Text = "Position :"
        lblCoup1.Text = ""
        lblCoup2.Text = ""
        lblCoup1.Tag = ""
        lblCoup2.Tag = ""
        lblProfondeur1.Text = ""
        lblProfondeur2.Text = ""
        lblProfondeur1.Tag = 0
        lblProfondeur2.Tag = 0
        lblDernier1.Text = ""
        lblDernier2.Text = ""
        lblRestant.Text = ""
        lblRestant.BackColor = Color.FromName("control")
        lblIdem.Text = ""
        lblKnps1.Text = ""
        lblKnps2.Text = ""
        lblScore1.Text = ""
        lblScore2.Text = ""
        lblHash1.Text = ""
        lblHash2.Text = ""
        lblTB1.Text = ""
        lblTB2.Text = ""
        lblPonder.Text = ""
        Me.Text = Me.Tag
        Application.DoEvents()

        'on masque l'échiquier
        If Me.Width = 1110 Then
            pbEchiquier.Refresh()
            pbMateriel.Refresh()
        End If

        deverrouillage()

        If e.ToString <> "System.Windows.Forms.FormClosingEventArgs" Then
            'location puissance calcul
            majDate()
            MsgBox("Requested stop !")
        End If

        cmdArreter.Enabled = False
        cbDiffere.Checked = False
    End Sub

    Private Sub timerAnalyse_Tick(sender As Object, e As EventArgs) Handles timerAnalyse.Tick
        Dim chaine1 As String, chaine2 As String, chaine As String, mate As Boolean, tentative As Integer
        Dim fichierLOG As String, donneesLOG As String, tempsEcoule As Integer
        Dim moteur1LOG As String, moteur2LOG As String, perfMoteur1 As String, perfMoteur2 As String

        timerAnalyse.Enabled = False
        timerProgression.Enabled = False
        tentative = 0

        'analyse en cours
        If analyseEnCours Then
            analyseEnCours = False
            tempsEcoule = DateDiff(DateInterval.Second, debAnalyse, Now())
            tempsCumule = tempsCumule + tempsEcoule

            If Not profondeurLimitee Then
                entree1.WriteLine("stop")
                If lblChemin2.Text <> "" Then
                    entree2.WriteLine("stop")
                End If
            End If

reprise_1:
            chaine1 = ""

            Do
                Application.DoEvents()
            Loop While InStr(sortie1, "bestmove") = 0

            If memSortie1 = "" Then
                memSortie1 = sortie1
                chaine1 = memSortie1
            Else
                chaine1 = Replace(sortie1, memSortie1, "")
                memSortie1 = sortie1
            End If
            mate = False
            If InStr(sortie1, " mate ") > 0 And InStr(sortie1, " mate -") = 0 Then
                mate = True
            End If
            sortie1 = sortie1.Substring(sortie1.IndexOf("bestmove "))
            sortie1 = Replace(sortie1, "bestmove ", "")
            If InStr(sortie1, " ") > 0 Then
                lblCoup1.Tag = sortie1.Substring(0, sortie1.IndexOf(" "))
            ElseIf InStr(sortie1, vbCrLf) > 0 Then
                lblCoup1.Tag = sortie1.Substring(0, sortie1.IndexOf(vbCrLf))
            Else
                lblCoup1.Tag = sortie1
            End If
            If mode = ".pgn" Then
                lblCoup1.Text = analyseCoups(lblCoup1.Tag, tabPGN(pos))
            ElseIf mode = ".epd" Then
                lblCoup1.Text = analyseCoups(lblCoup1.Tag, "", tabEPD(pos))
            End If

            If mate Then
                lblCoup1.Text = lblCoup1.Text & "#"
            End If

            If lblCoup1.Text = "in-fo" Or lblCoup1.Text = "re-ad" Then
                If tentative < 2 Then
                    tentative = tentative + 1
                    GoTo reprise_1
                Else
                    MsgBox("Problem 1")
                End If
            End If
            tentative = 0

reprise_2:
            If lblChemin2.Text <> "" And Not profondeurLimitee Then
                chaine2 = ""

                Do
                    Application.DoEvents()
                Loop While InStr(sortie2, "bestmove") = 0

                If memSortie2 = "" Then
                    memSortie2 = sortie2
                    chaine2 = memSortie2
                Else
                    chaine2 = Replace(sortie2, memSortie2, "")
                    memSortie2 = sortie2
                End If
                mate = False
                If InStr(sortie2, " mate ") > 0 And InStr(sortie2, " mate -") = 0 Then
                    mate = True
                End If
                sortie2 = sortie2.Substring(sortie2.IndexOf("bestmove "))
                sortie2 = Replace(sortie2, "bestmove ", "")
                If InStr(sortie2, " ") > 0 Then
                    lblCoup2.Tag = sortie2.Substring(0, sortie2.IndexOf(" "))
                ElseIf InStr(sortie2, vbCrLf) > 0 Then
                    lblCoup2.Tag = sortie2.Substring(0, sortie2.IndexOf(vbCrLf))
                Else
                    lblCoup2.Tag = sortie2
                End If
                If mode = ".pgn" Then
                    lblCoup2.Text = analyseCoups(lblCoup2.Tag, tabPGN(pos))
                ElseIf mode = ".epd" Then
                    lblCoup2.Text = analyseCoups(lblCoup2.Tag, "", tabEPD(pos))
                End If

                If mate Then
                    lblCoup2.Text = lblCoup2.Text & "#"
                End If

                If lblCoup2.Text = "in-fo" Or lblCoup2.Text = "re-ad" Then
                    If tentative < 2 Then
                        tentative = tentative + 1
                        GoTo reprise_2
                    Else
                        MsgBox("Problem 2")
                    End If
                End If
                tentative = 0
            End If

            If Me.Width = 1110 Then
                pbEchiquier.Refresh()
            End If

            debTransition = Now()

            'en anglais pour github, en français pour domicile
            If My.Computer.Name <> "WORKSTATION" And My.Computer.Name <> "BUREAU" And My.Computer.Name <> "HTPC" And My.Computer.Name <> "TOUR-COURTOISIE" And My.Computer.Name <> "PLEXI" And My.Computer.Name <> "BOIS" Then
                lblCoup1.Text = coupsEN(lblCoup1.Text)
                lblCoup2.Text = coupsEN(lblCoup2.Text)
            End If

            If lblChemin2.Text = "" Then
                If InStr(lblChemin1.Text, "mpiexec.exe") > 0 Then
                    chaine = Format(pos + 1, "00000") & ") " & lblCoup1.Text & " (" & Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf(" ") + 1), ".exe", "") & ")" & vbCrLf _
                           & Format(pos + 1, "00000") & ") " & "-" & " (" & Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf(" ") + 1), ".exe", "") & ")" & vbCrLf & vbCrLf
                Else
                    chaine = Format(pos + 1, "00000") & ") " & lblCoup1.Text & " (" & Replace(nomFichier(lblChemin1.Text), extensionFichier(lblChemin1.Text), "") & ")" & vbCrLf _
                           & Format(pos + 1, "00000") & ") " & "-" & " (" & Replace(nomFichier(lblChemin1.Text), extensionFichier(lblChemin1.Text), "") & ")" & vbCrLf & vbCrLf
                End If
                idem = idem + 1
            ElseIf lblCoup1.Text = lblCoup2.Text Then
                chaine = Format(pos + 1, "00000") & ") " & lblCoup1.Text & " (" & Replace(nomFichier(lblChemin1.Text), extensionFichier(lblChemin1.Text), "") & ")" & vbCrLf _
                           & Format(pos + 1, "00000") & ") " & "-" & " (" & Replace(nomFichier(lblChemin2.Text), extensionFichier(lblChemin2.Text), "") & ")" & vbCrLf & vbCrLf
                idem = idem + 1
            ElseIf InStr(lblCoup1.Text, "#") > 0 And InStr(lblCoup2.Text, "#") = 0 Then
                chaine = Format(pos + 1, "00000") & ") " & lblCoup1.Text & " (" & Replace(nomFichier(lblChemin1.Text), extensionFichier(lblChemin1.Text), "") & ")" & vbCrLf _
                       & Format(pos + 1, "00000") & ") " & "-" & " (" & Replace(nomFichier(lblChemin2.Text), extensionFichier(lblChemin2.Text), "") & ")" & vbCrLf & vbCrLf
            ElseIf InStr(lblCoup1.Text, "#") = 0 And InStr(lblCoup2.Text, "#") > 0 Then
                chaine = Format(pos + 1, "00000") & ") " & lblCoup2.Text & " (" & Replace(nomFichier(lblChemin1.Text), extensionFichier(lblChemin1.Text), "") & ")" & vbCrLf _
                       & Format(pos + 1, "00000") & ") " & "-" & " (" & Replace(nomFichier(lblChemin2.Text), extensionFichier(lblChemin2.Text), "") & ")" & vbCrLf & vbCrLf
            Else
                chaine = Format(pos + 1, "00000") & ") " & lblCoup1.Text & " (" & Replace(nomFichier(lblChemin1.Text), extensionFichier(lblChemin1.Text), "") & ")" & vbCrLf _
                       & Format(pos + 1, "00000") & ") " & lblCoup2.Text & " (" & Replace(nomFichier(lblChemin2.Text), extensionFichier(lblChemin2.Text), "") & ")" & vbCrLf & vbCrLf
            End If
            My.Computer.FileSystem.WriteAllText(Replace(lblFichier.Text, extensionFichier(lblFichier.Text), ".rep"), chaine, True)

            If cbLog.Checked Then
                If ((pos + 1) Mod 10 = 0) Or profondeurLimitee Then
                    capturer()
                End If

                '1 seul moteur
                fichierLOG = Replace(mode, ".", "") & " - "
                fichierLOG = fichierLOG & Format(Val(txtPuissance.Text), "00") & "C - " 'pgn - 20C - 
                If profondeurLimitee Then
                    fichierLOG = fichierLOG & txtLimite.Text & " - " 'pgn - 20C - D60 - 
                Else
                    fichierLOG = fichierLOG & txtLimite.Text & " sec - " 'pgn - 20C - 120 sec - 
                End If
                fichierLOG = fichierLOG & Format(Val(txtMemoire.Text), "0") & " MB.log"  'pgn - 20C - D60 - 16384 MB.log ou pgn - 20C - 120 sec - 16384 MB.log

                If profondeurLimitee Then
                    donneesLOG = Format(tempsEcoule, "000000") & " - "
                Else
                    donneesLOG = Format(Val(Replace(lblDernier1.Text, " sec", "")), "000000") & " - "
                End If
                donneesLOG = donneesLOG & Format(CSng(Replace(lblHash1.Text, " %", "")), "000.0") & " - "
                donneesLOG = donneesLOG & Format(nbChangements, "0") & vbCrLf

                If InStr(lblChemin1.Text, "mpiexec.exe") > 0 Then
                    moteur1LOG = Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf(" ") + 1), ".exe", " (" & Format(Val(txtPuissance.Text), "00") & "C).log")
                Else
                    moteur1LOG = Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf("\") + 1), ".exe", " (" & Format(Val(txtPuissance.Text), "00") & "C).log")
                End If
                perfMoteur1 = Format(Val(Replace(lblKnps1.Text, " Knps", "")), "000000") & " - "
                perfMoteur1 = perfMoteur1 & Format(lblProfondeur1.Tag, "000") & " - "
                perfMoteur1 = perfMoteur1 & Format(Val(Replace(lblTB1.Text, " tb hits", "")), "000000000") & vbCrLf

                If lblChemin2.Text = "" Then
                    If cpu() = cpu(True) And InStr(lblChemin1.Text, "mpiexec.exe") = 0 Then
                        My.Computer.FileSystem.WriteAllText(fichierLOG, donneesLOG, True)
                        My.Computer.FileSystem.WriteAllText(moteur1LOG, perfMoteur1, True)
                    Else
                        'hyperthreading activé
                        fichierLOG = Replace(mode, ".", "") & " - " 'epd - 
                        fichierLOG = fichierLOG & Format(Val(txtPuissance.Text), "00") & "T - " 'epd - 40T - 
                        If profondeurLimitee Then
                            fichierLOG = fichierLOG & txtLimite.Text & " - " 'epd - 40T - D60 - 
                        Else
                            fichierLOG = fichierLOG & txtLimite.Text & " sec - " 'epd - 40T - 120 sec - 
                        End If
                        fichierLOG = fichierLOG & Format(Val(txtMemoire.Text), "0") & " MB.log" 'epd - 40T - D60 - 16384 MB.log ou  'epd - 40T - 120 sec - 16384 MB.log

                        If InStr(lblChemin1.Text, "mpiexec.exe") > 0 Then
                            moteur1LOG = Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf(" ") + 1), ".exe", " (" & Format(Val(txtPuissance.Text), "00") & "T).log")
                        Else
                            moteur1LOG = Replace(lblChemin1.Text.Substring(lblChemin1.Text.LastIndexOf("\") + 1), ".exe", " (" & Format(Val(txtPuissance.Text), "00") & "T).log")
                        End If

                        My.Computer.FileSystem.WriteAllText(fichierLOG, donneesLOG, True)
                        My.Computer.FileSystem.WriteAllText(moteur1LOG, perfMoteur1, True)
                    End If
                Else
                    '2 moteurs
                    donneesLOG = donneesLOG & Format(Val(Replace(lblDernier2.Text, " sec", "")), "000000") & " - "
                    donneesLOG = donneesLOG & Format(CSng(Replace(lblHash2.Text, " %", "")), "000.0") & " - "
                    donneesLOG = donneesLOG & Format(nbChangements, "0") & vbCrLf

                    moteur2LOG = Replace(lblChemin2.Text.Substring(lblChemin2.Text.LastIndexOf("\") + 1), ".exe", " (" & Format(Val(txtPuissance.Text), "00") & "C).log")
                    perfMoteur2 = Format(Val(Replace(lblKnps2.Text, " Knps", "")), "000000") & " - "
                    perfMoteur2 = perfMoteur2 & Format(lblProfondeur2.Tag, "000") & " - "
                    perfMoteur2 = perfMoteur2 & Format(Val(Replace(lblTB2.Text, " tb hits", "")), "000000000") & vbCrLf

                    My.Computer.FileSystem.WriteAllText(fichierLOG, donneesLOG, True)
                    My.Computer.FileSystem.WriteAllText(moteur1LOG, perfMoteur1, True)
                    My.Computer.FileSystem.WriteAllText(moteur2LOG, perfMoteur2, True)
                End If

                'positions les plus changeantes
                If nbChangements > 17 Then
                    If My.Computer.FileSystem.FileExists("changes.log") Then
                        chaine = My.Computer.FileSystem.ReadAllText("changes.log")
                        If InStr(chaine, tabEPD(pos)) = 0 Then
                            My.Computer.FileSystem.WriteAllText("changes.log", Format(nbChangements, "000") & " : " & tabEPD(pos) & vbCrLf, True)
                        End If
                    Else
                        My.Computer.FileSystem.WriteAllText("changes.log", Format(nbChangements, "000") & " : " & tabEPD(pos) & vbCrLf, True)
                    End If
                End If
            End If

            pos = pos + 1

            'fin de l'analyse
            If mode = ".pgn" Then
                If pos > UBound(tabPositions) Then
                    deverrouillage()

                    'location puissance calcul
                    majDate(True)

                    If cbArret.Checked Then
                        Process.Start("C:\Windows\System32\shutdown.exe", "-s -t 10")
                    Else
                        MsgBox("End !")
                    End If
                    End
                Else
                    timerAnalyse.Interval = 1000 'on se laisse 1 sec pour sortir de là
                    timerAnalyse.Enabled = True
                End If
            ElseIf mode = ".epd" Then
                If pos > UBound(tabEPD) Then
                    deverrouillage()

                    'location puissance calcul
                    majDate(True)

                    If cbArret.Checked Then
                        Process.Start("C:\Windows\System32\shutdown.exe", "-s -t 10")
                    Else
                        MsgBox("End !")
                    End If
                    End
                Else
                    timerAnalyse.Interval = 1000 'on se laisse 1 sec pour sortir de là
                    timerAnalyse.Enabled = True
                End If
            End If
        Else
            'nouvelle analyse
            If mode = ".pgn" Then
                lblPartie.Text = coupsEN(formaterCoups("", tabPGN(pos)))
                If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                    lblPosition.Text = "White to" & vbCrLf & "play :"
                Else
                    lblPosition.Text = "Black to" & vbCrLf & "play :"
                End If
            ElseIf mode = ".epd" Then
                lblPartie.Text = tabEPD(pos)
                If InStr(tabEPD(pos), " w ") > 0 Then
                    lblPosition.Text = "White to" & vbCrLf & "play :"
                ElseIf InStr(tabEPD(pos), " b ") > 0 Then
                    lblPosition.Text = "Black to" & vbCrLf & "play :"
                End If
            End If

            lblCoup1.Text = ""
            lblCoup2.Text = ""
            lblCoup1.Tag = ""
            lblCoup2.Tag = ""
            lblProfondeur1.Text = ""
            lblProfondeur2.Text = ""
            lblProfondeur1.Tag = 0
            lblProfondeur2.Tag = 0
            lblDernier1.Text = ""
            lblDernier2.Text = ""
            lblRestant.Text = ""
            lblRestant.BackColor = Color.FromName("control")
            lblIdem.Text = ""
            lblKnps1.Text = ""
            lblKnps2.Text = ""
            lblScore1.Text = ""
            lblScore2.Text = ""
            lblHash1.Text = ""
            lblHash2.Text = ""
            lblTB1.Text = ""
            lblTB2.Text = ""
            lblPonder.Text = ""
            memSortie1 = ""
            memSortie2 = ""
            nbAccords = 0
            nbChangements = 0
            sortie1 = ""
            sortie2 = ""
            txtReprise.Text = pos + 1
            Me.Text = Me.Tag

            'échiquier
            If Me.Width = 1110 Then
                pbEchiquier.Refresh()
            End If

            If mode = ".pgn" Then
                lblProgression.Text = "/" & tabPositions.Length & " (" & Format(Val(txtReprise.Text) / tabPositions.Length, "0%") & ")"
            ElseIf mode = ".epd" Then
                lblProgression.Text = "/" & tabEPD.Length & " (" & Format(Val(txtReprise.Text) / tabEPD.Length, "0%") & ")"
            End If
            majConfig()

            'estimation du temps d'analyse
            If profondeurLimitee Then
                If tempsCumule = 0 Then
                    lblEstimation.Text = "Est. :"
                Else
                    lblEstimation.Text = estimation(tempsCumule / (pos - Val(txtReprise.Tag)), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
                End If
            Else
                If tempsCumule = 0 Then
                    lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), Val(txtLimite.Text) / 2, tpsTransition / nbTransitions, "Est.")
                Else
                    lblEstimation.Text = estimation(Val(txtLimite.Text), Val(txtReprise.Text), tempsCumule / (pos - Val(txtReprise.Tag)), tpsTransition / nbTransitions, "Est.")
                End If
            End If

            entree1.WriteLine("ucinewgame")
            If lblChemin2.Text <> "" Then
                entree2.WriteLine("ucinewgame")
            End If

            entree1.WriteLine("setoption name Clear Hash")
            If lblChemin2.Text <> "" Then
                entree2.WriteLine("setoption name Clear Hash")
            End If

            entree1.WriteLine("isready")
            If lblChemin2.Text <> "" Then
                entree2.WriteLine("isready")
            End If

            Do
                Application.DoEvents()
                If moteur1 Is Nothing Or entree1 Is Nothing Or erreur1 Is Nothing Then
                    End
                End If
            Loop While InStr(sortie1, "readyok") = 0
            sortie1 = ""
            memSortie1 = ""

            If lblChemin2.Text <> "" Then
                Do
                    Application.DoEvents()
                    If moteur2 Is Nothing Or entree2 Is Nothing Or erreur2 Is Nothing Then
                        End
                    End If
                Loop While InStr(sortie2, "readyok") = 0
                sortie2 = ""
                memSortie2 = ""
            End If

            If mode = ".pgn" Then
                entree1.WriteLine("position startpos moves " & tabPositions(pos))
                If lblChemin2.Text <> "" Then
                    entree2.WriteLine("position startpos moves " & tabPositions(pos))
                End If
            ElseIf mode = ".epd" Then
                entree1.WriteLine("position fen " & tabEPD(pos))
                If lblChemin2.Text <> "" Then
                    entree2.WriteLine("position fen " & tabEPD(pos))
                End If
            End If

            If profondeurLimitee Then
                entree1.WriteLine("go depth " & Replace(txtLimite.Text, "D", ""))
            Else
                entree1.WriteLine("go infinite")
                If lblChemin2.Text <> "" Then
                    entree2.WriteLine("go infinite")
                End If
            End If

            analyseEnCours = True
            If profondeurLimitee Then
                timerAnalyse.Interval = 3600 * 168 * 1000 'max 1 semaine/coup pour D60
            Else
                timerAnalyse.Interval = Val(txtLimite.Text) * 1000
            End If
            timerAnalyse.Tag = timerAnalyse.Interval 'on mémorise la valeur originelle pour lblRestant

            debAnalyse = Now()
            nbTransitions = nbTransitions + 1
            tpsTransition = tpsTransition + DateDiff(DateInterval.Second, debTransition, debAnalyse)
            Me.Text = My.Computer.Name & " : transition avg. @ " & Format(tpsTransition / nbTransitions, "0.0") & " sec"

            If pos > Val(txtReprise.Tag) Then
                'analyse moyenne
                If profondeurLimitee Then
                    Me.Text = Me.Text & " - analysis avg. @ " & Format(tempsCumule / (pos - Val(txtReprise.Tag)) / 3600, "0") & " hrs/pos"
                Else
                    Me.Text = Me.Text & " - analysis avg. @ " & Format(tempsCumule / (pos - Val(txtReprise.Tag)), "0") & " sec/pos"
                End If
            Else
                'que la 1ère position (départ ou reprise)
                If profondeurLimitee Then
                    Me.Text = Me.Text & " - analysis fixed @ " & txtLimite.Text & "/pos"
                Else
                    Me.Text = Me.Text & " - analysis avg. @ " & Format(Val(txtLimite.Text), "0") & " sec/pos"
                End If
            End If

            Me.Tag = Me.Text
            matDetecte = False
            timerAnalyse.Enabled = True
            timerProgression.Enabled = True
        End If
    End Sub

    Private Sub timerProgression_Tick(sender As Object, e As EventArgs) Handles timerProgression.Tick
        Dim chaine As String, tabChaine() As String, tempsEcoule As Integer, tabPV() As String, ponder1 As String, ponder2 As String
        Dim depth As String

        timerProgression.Enabled = False

        If Not analyseEnCours Then
            Exit Sub
        End If

        chaine = ""
        ponder1 = "-"
        ponder2 = "-"
        tempsEcoule = DateDiff(DateInterval.Second, debAnalyse, Now)
        'désormais on utilise le tag (à cause de la rallonge)
        If tempsEcoule <= (timerAnalyse.Tag / 1000) Then
            If profondeurLimitee Then
                lblRestant.Text = Format(tempsEcoule / 60, "0 min") 'si mode profondeur limitée => temps écoulé
                If tempsEcoule Mod 900 = 0 Then 'capture toutes les 15 minutes
                    capturer()
                End If
            Else
                lblRestant.Text = Format((timerAnalyse.Tag / 1000) - tempsEcoule, "0 sec")
            End If
            If pos > 0 Then
                lblIdem.Tag = Format(idem / pos, "0%")
                If lblChemin2.Text = "" Then
                    lblIdem.Text = ""
                Else
                    lblIdem.Text = lblIdem.Tag
                End If
            End If
        Else
            Exit Sub
        End If

        If analyseEnCours Then
            chaine = Replace(sortie1, memSortie1, "")
            memSortie1 = sortie1

            tabChaine = Split(chaine, vbCrLf)
            For i = 0 To UBound(tabChaine)
                If InStr(tabChaine(i), " pv ") > 0 _
                And InStr(tabChaine(i), "upperbound") = 0 _
                And InStr(tabChaine(i), "lowerbound") = 0 Then
                    lblCoup1.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" pv ") + 4)
                    'pour récupérer le ponder
                    tabPV = Split(lblCoup1.Tag, " ")
                    'on retient que le 1er coup du PV
                    If InStr(lblCoup1.Tag, " ") > 1 Then
                        lblCoup1.Tag = gauche(lblCoup1.Tag, lblCoup1.Tag.IndexOf(" "))
                    End If
                    If mode = ".pgn" Then
                        lblCoup1.Text = analyseCoups(lblCoup1.Tag, tabPGN(pos))
                        'formatage du ponder
                        If tabPV.Length > 1 Then
                            If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                                ponder1 = analyseCoups(tabPV(1), Replace(tabPGN(pos), " *", " 0. " & lblCoup1.Text & " "))
                            Else
                                ponder1 = analyseCoups(tabPV(1), Replace(tabPGN(pos), " *", " " & lblCoup1.Text & " "))
                            End If
                        Else
                            ponder1 = "-"
                        End If
                    ElseIf mode = ".epd" Then
                        lblCoup1.Text = analyseCoups(lblCoup1.Tag, "", tabEPD(pos))
                        'formatage du ponder
                        If tabPV.Length > 1 Then
                            If InStr(tabEPD(pos), " w ") > 0 Then
                                ponder1 = analyseCoups(tabPV(1), " 0. " & lblCoup1.Text & " ", tabEPD(pos))
                            Else
                                ponder1 = analyseCoups(tabPV(1), lblCoup1.Text & " ", tabEPD(pos))
                            End If
                        Else
                            ponder1 = "-"
                        End If
                    End If

                    lblPonder.Text = ponder1 & " / -"
                    matMoteur1 = False
                    If InStr(tabChaine(i), " mate ") > 0 And InStr(tabChaine(i), " mate -") = 0 Then
                        lblCoup1.Text = lblCoup1.Text & "#"
                        matMoteur1 = True
                    End If
                    If InStr(tabChaine(i), " score cp ") > 0 Then
                        lblScore1.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" score cp ") + 10)
                        If InStr(lblScore1.Tag, " ") > 1 Then
                            lblScore1.Tag = gauche(lblScore1.Tag, lblScore1.Tag.IndexOf(" "))
                        End If

                        If mode = ".pgn" Then
                            If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                                lblScore1.Text = Format(Val(lblScore1.Tag) / 100, "0.00")
                            Else
                                lblScore1.Text = Format(-Val(lblScore1.Tag) / 100, "0.00")
                            End If
                        ElseIf mode = ".epd" Then
                            If InStr(tabEPD(pos), " w ") > 0 Then
                                lblScore1.Text = Format(Val(lblScore1.Tag) / 100, "0.00")
                            ElseIf InStr(tabEPD(pos), " b ") > 0 Then
                                lblScore1.Text = Format(-Val(lblScore1.Tag) / 100, "0.00")
                            End If
                        End If

                    ElseIf InStr(tabChaine(i), " score mate ") > 0 Then
                        lblScore1.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" score mate ") + 12)
                        If InStr(lblScore1.Tag, " ") > 1 Then
                            lblScore1.Tag = gauche(lblScore1.Tag, lblScore1.Tag.IndexOf(" "))
                        End If
                        If mode = ".pgn" Then
                            If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                                lblScore1.Text = "mat " & lblScore1.Tag
                            Else
                                lblScore1.Text = "mat -" & lblScore1.Tag
                            End If
                        ElseIf mode = ".epd" Then
                            If InStr(tabEPD(pos), " w ") > 0 Then
                                lblScore1.Text = "mat " & lblScore1.Tag
                            ElseIf InStr(tabEPD(pos), " b ") > 0 Then
                                lblScore1.Text = "mat -" & lblScore1.Tag
                            End If
                        End If
                        lblScore1.Text = Replace(lblScore1.Text, "--", "")
                        matDetecte = True
                    End If

                    'si le coup est différent
                    If tabMemCoups1(0) <> lblCoup1.Tag Then
                        tabMemCoups1(0) = lblCoup1.Tag
                        lblDernier1.Text = Format(tempsEcoule, "## ##0") & " sec"
                        If tempsEcoule > 2 Then
                            nbChangements = nbChangements + 1
                            Me.Text = Me.Tag & " - " & nbChangements & " changes"
                            capturer() 'capture à chaque changement (au delà de 2 sec pour éviter le moulon du départ)
                        End If
                        'échiquier
                        If Me.Width = 1110 Then
                            pbEchiquier.Refresh()
                        End If
                    End If
                End If

                If InStr(tabChaine(i), " depth ") > 0 _
                And InStr(tabChaine(i), " currmove ") = 0 _
                And ((Not profondeurLimitee) Or (profondeurLimitee And InStr(tabChaine(i), "upperbound") = 0 And InStr(tabChaine(i), "lowerbound") = 0)) Then
                    If tabChaine(i).LastIndexOf(" depth ") + 10 <= Len(tabChaine(i)) Then
                        depth = Trim(tabChaine(i).Substring(tabChaine(i).LastIndexOf(" depth ") + 6, 4))
                        If Not IsNumeric(depth) Then
                            If InStr(depth, " ") > 0 Then
                                depth = depth.Substring(0, depth.IndexOf(" "))
                            Else
                                depth = "0"
                            End If
                        End If
                        lblProfondeur1.Tag = CInt(depth)
                        If mode = ".pgn" _
                        And InStr(tabChaine(i), " seldepth ") > 0 _
                        And tabChaine(i).LastIndexOf(" seldepth ") + 13 <= Len(tabChaine(i)) Then
                            depth = Trim(tabChaine(i).Substring(tabChaine(i).LastIndexOf(" seldepth ") + 9, 4))
                            If Not IsNumeric(depth) Then
                                If InStr(depth, " ") > 0 Then
                                    depth = "/" & depth.Substring(0, depth.IndexOf(" "))
                                Else
                                    depth = ""
                                End If
                            Else
                                depth = "/" & depth
                            End If
                            lblProfondeur1.Text = "D" & lblProfondeur1.Tag & depth
                        Else
                            lblProfondeur1.Text = "D" & lblProfondeur1.Tag
                        End If
                    End If
                End If

                If InStr(tabChaine(i), " hashfull ") > 0 Then
                    lblHash1.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" hashfull ") + 10)
                    If InStr(lblHash1.Tag, " ") > 1 Then
                        lblHash1.Tag = gauche(lblHash1.Tag, lblHash1.Tag.IndexOf(" "))
                    End If
                    lblHash1.Text = Trim(Format(Val(lblHash1.Tag / 10), "0.0")) & " %"
                End If

                If InStr(tabChaine(i), " nps ") > 0 Then
                    lblKnps1.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" nps ") + 5)
                    If InStr(lblKnps1.Tag, " ") > 1 Then
                        lblKnps1.Tag = gauche(lblKnps1.Tag, lblKnps1.Tag.IndexOf(" "))
                    End If
                    lblKnps1.Text = Trim(Format(Val(lblKnps1.Tag / 1000), "## ##0")) & " Knps"
                End If

                If InStr(tabChaine(i), " tbhits ") > 0 Then
                    lblTB1.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" tbhits ") + 8)
                    If InStr(lblTB1.Tag, " ") > 1 Then
                        lblTB1.Tag = gauche(lblTB1.Tag, lblTB1.Tag.IndexOf(" "))
                    End If
                    lblTB1.Text = Trim(Format(Val(lblTB1.Tag), "# ### ##0")) & " tb hits"
                End If

                Application.DoEvents()
            Next
        End If

        If lblChemin2.Text <> "" Then
            chaine = ""
            If analyseEnCours Then
                chaine = Replace(sortie2, memSortie2, "")
                memSortie2 = sortie2

                tabChaine = Split(chaine, vbCrLf)
                For i = 0 To UBound(tabChaine)
                    If InStr(tabChaine(i), " pv ") > 0 _
                    And InStr(tabChaine(i), "upperbound") = 0 _
                    And InStr(tabChaine(i), "lowerbound") = 0 Then
                        lblCoup2.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" pv ") + 4)
                        'pour récupérer le ponder
                        tabPV = Split(lblCoup2.Tag, " ")
                        'on retient que le 1er coup du PV
                        If InStr(lblCoup2.Tag, " ") > 1 Then
                            lblCoup2.Tag = gauche(lblCoup2.Tag, lblCoup2.Tag.IndexOf(" "))
                        End If
                        If mode = ".pgn" Then
                            lblCoup2.Text = analyseCoups(lblCoup2.Tag, tabPGN(pos))
                            'formatage du ponder
                            If tabPV.Length > 1 Then
                                If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                                    ponder2 = analyseCoups(tabPV(1), Replace(tabPGN(pos), " *", " 0. " & lblCoup2.Text & " "))
                                Else
                                    ponder2 = analyseCoups(tabPV(1), Replace(tabPGN(pos), " *", " " & lblCoup2.Text & " "))
                                End If
                            Else
                                ponder2 = "-"
                            End If
                        ElseIf mode = ".epd" Then
                            lblCoup2.Text = analyseCoups(lblCoup2.Tag, "", tabEPD(pos))
                            'formatage du ponder
                            If tabPV.Length > 1 Then
                                If InStr(tabEPD(pos), " w ") > 0 Then
                                    ponder2 = analyseCoups(tabPV(1), " 0. " & lblCoup2.Text & " ", tabEPD(pos))
                                Else
                                    ponder2 = analyseCoups(tabPV(1), lblCoup2.Text & " ", tabEPD(pos))
                                End If
                            Else
                                ponder2 = "-"
                            End If
                        End If

                        lblPonder.Text = ponder1 & " / " & ponder2
                        matMoteur2 = False
                        If InStr(tabChaine(i), " mate ") > 0 And InStr(tabChaine(i), " mate -") = 0 Then
                            lblCoup2.Text = lblCoup2.Text & "#"
                            matMoteur2 = True
                        End If
                        If InStr(tabChaine(i), " score cp ") > 0 Then
                            lblScore2.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" score cp ") + 10)
                            If InStr(lblScore2.Tag, " ") > 1 Then
                                lblScore2.Tag = gauche(lblScore2.Tag, lblScore2.Tag.IndexOf(" "))
                            End If

                            If mode = ".pgn" Then
                                If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                                    lblScore2.Text = Format(Val(lblScore2.Tag) / 100, "0.00")
                                Else
                                    lblScore2.Text = Format(-Val(lblScore2.Tag) / 100, "0.00")
                                End If
                            ElseIf mode = ".epd" Then
                                If InStr(tabEPD(pos), " w ") > 0 Then
                                    lblScore2.Text = Format(Val(lblScore2.Tag) / 100, "0.00")
                                ElseIf InStr(tabEPD(pos), " b ") > 0 Then
                                    lblScore2.Text = Format(-Val(lblScore2.Tag) / 100, "0.00")
                                End If
                            End If
                        ElseIf InStr(tabChaine(i), " score mate ") > 0 Then
                            lblScore2.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" score mate ") + 12)
                            If InStr(lblScore2.Tag, " ") > 1 Then
                                lblScore2.Tag = gauche(lblScore2.Tag, lblScore2.Tag.IndexOf(" "))
                            End If
                            If mode = ".pgn" Then
                                If tabPositions(pos).Split(" ").Length Mod 2 = 0 Then
                                    lblScore2.Text = "mat " & lblScore2.Tag
                                Else
                                    lblScore2.Text = "mat -" & lblScore2.Tag
                                End If
                            ElseIf mode = ".epd" Then
                                If InStr(tabEPD(pos), " w ") > 0 Then
                                    lblScore2.Text = "mat " & lblScore2.Tag
                                ElseIf InStr(tabEPD(pos), " b ") > 0 Then
                                    lblScore2.Text = "mat -" & lblScore2.Tag
                                End If
                            End If
                            lblScore2.Text = Replace(lblScore2.Text, "--", "")
                            matDetecte = True
                        End If

                        'si le coup est différent
                        If tabMemCoups2(0) <> lblCoup2.Tag Then
                            tabMemCoups2(0) = lblCoup2.Tag
                            lblDernier2.Text = Format(tempsEcoule, "## ##0") & " sec"
                            If tempsEcoule > 2 Then
                                nbChangements = nbChangements + 1
                                Me.Text = Me.Tag & " - " & nbChangements & " changes"
                            End If
                            'échiquier
                            If Me.Width = 1110 Then
                                pbEchiquier.Refresh()
                            End If
                        End If
                    End If

                    If InStr(tabChaine(i), " depth ") > 0 _
                    And InStr(tabChaine(i), " currmove ") = 0 Then
                        If tabChaine(i).LastIndexOf(" depth ") + 10 <= Len(tabChaine(i)) Then
                            depth = Trim(tabChaine(i).Substring(tabChaine(i).LastIndexOf(" depth ") + 6, 4))
                            If Not IsNumeric(depth) Then
                                If InStr(depth, " ") > 0 Then
                                    depth = depth.Substring(0, depth.IndexOf(" "))
                                Else
                                    depth = "0"
                                End If
                            End If
                            lblProfondeur2.Tag = CInt(depth)
                            If mode = ".pgn" _
                            And InStr(tabChaine(i), " seldepth ") > 0 _
                            And tabChaine(i).LastIndexOf(" seldepth ") + 13 <= Len(tabChaine(i)) Then
                                depth = Trim(tabChaine(i).Substring(tabChaine(i).LastIndexOf(" seldepth ") + 9, 4))
                                If Not IsNumeric(depth) Then
                                    If InStr(depth, " ") > 0 Then
                                        depth = "/" & depth.Substring(0, depth.IndexOf(" "))
                                    Else
                                        depth = ""
                                    End If
                                Else
                                    depth = "/" & depth
                                End If
                                lblProfondeur2.Text = "D" & lblProfondeur2.Tag & depth
                            Else
                                lblProfondeur2.Text = "D" & lblProfondeur2.Tag
                            End If
                        End If
                    End If


                    If InStr(tabChaine(i), " hashfull ") > 0 Then
                        lblHash2.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" hashfull ") + 10)
                        If InStr(lblHash2.Tag, " ") > 1 Then
                            lblHash2.Tag = gauche(lblHash2.Tag, lblHash2.Tag.IndexOf(" "))
                        End If
                        lblHash2.Text = Trim(Format(Val(lblHash2.Tag / 10), "0.0")) & " %"
                    End If

                    If InStr(tabChaine(i), " nps ") > 0 Then
                        lblKnps2.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" nps ") + 5)
                        If InStr(lblKnps2.Tag, " ") > 1 Then
                            lblKnps2.Tag = gauche(lblKnps2.Tag, lblKnps2.Tag.IndexOf(" "))
                        End If
                        lblKnps2.Text = Trim(Format(Val(lblKnps2.Tag / 1000), "## ##0")) & " Knps"
                    End If

                    If InStr(tabChaine(i), " tbhits ") > 0 Then
                        lblTB2.Tag = tabChaine(i).Substring(tabChaine(i).LastIndexOf(" tbhits ") + 8)
                        If InStr(lblTB2.Tag, " ") > 1 Then
                            lblTB2.Tag = gauche(lblTB2.Tag, lblTB2.Tag.IndexOf(" "))
                        End If
                        lblTB2.Text = Trim(Format(Val(lblTB2.Tag), "# ### ##0")) & " tb hits"
                    End If
                    Application.DoEvents()
                Next
            End If
        End If

        tabMemCoups1(1) = lblCoup1.Tag
        If lblChemin2.Text <> "" Then
            tabMemCoups2(1) = lblCoup2.Tag
        End If

        'si les moteurs sont d'accord, on le comptabilise
        If lblChemin2.Text <> "" Then
            If Not tabMemCoups1(1) Is Nothing And Not tabMemCoups2(1) Is Nothing Then
                If Replace(tabMemCoups1(1), "#", "") = Replace(tabMemCoups2(1), "#", "") Then
                    nbAccords = nbAccords + 1
                Else
                    'les moteurs ne sont pas d'accord
                    nbAccords = 0
                    'et si en plus on est à 5 sec de la fin de l'analyse
                    'et s'il y a eu 6 changements ou + (genre 3 par moteur)
                    'et si la rallonge n'est pas supérieure à la moitié de la durée originelle (ex : en mode epd à 30sec faut éviter une rallonge de 60sec)
                    'et si on n'a pas déjà ajouté une rallonge
                    If (timerAnalyse.Interval - 5000) <= tempsEcoule * 1000 _
                    And nbChangements >= 6 _
                    And rallonge <= Val(txtLimite.Text) / 2 _
                    And timerAnalyse.Tag = Val(txtLimite.Text) * 1000 _
                    And Not cbFixee.Checked Then
                        'on arrête le timerAnalyse
                        timerAnalyse.Enabled = False
                        'on ajoute une rallonge à sa durée originelle
                        timerAnalyse.Tag = timerAnalyse.Tag + rallonge * 1000
                        'on met à jour son intervalle
                        timerAnalyse.Interval = timerAnalyse.Tag - tempsEcoule * 1000
                        'on relance le timerAnalyse
                        timerAnalyse.Enabled = True
                        'on met en surbrillance la zone de texte
                        lblRestant.BackColor = Color.Yellow
                    End If
                End If
            End If

            'après 30 sec d'analyse, si les 2 moteurs sont d'accord à 75% du temps
            If 30 <= tempsEcoule _
            And (Not matDetecte Or (matDetecte And matMoteur1 And matMoteur2) Or (matDetecte And Not matMoteur1 And Not matMoteur2)) Then
                If tempsEcoule * 0.75 <= nbAccords _
                And Not cbFixee.Checked Then
                    'on écourte l'analyse
                    timerAnalyse.Interval = 1000
                    Exit Sub
                ElseIf tempsEcoule * 0.5 < nbAccords Then
                    lblIdem.Text = Format(nbAccords / tempsEcoule, "0%") & " / " & lblIdem.Tag
                End If
            End If
        End If

        'si on a atteint la profondeur max du moteur ou la profondeur limite demandée, on écourte l'analyse
        If timerAnalyse.Interval > 1000 And Not cbFixee.Checked Then
            If (lblChemin1.Text <> "" And lblProfondeur1.Tag = maxProfMoteur(lblChemin1.Text)) _
            Or (lblChemin2.Text <> "" And lblProfondeur2.Tag = maxProfMoteur(lblChemin2.Text)) _
            Or (profondeurLimitee And InStr(sortie1, "bestmove") > 0) Then
                timerAnalyse.Interval = 1000
            End If
        End If

        'traduction (juste visuelle dans timerProgression, on ne le fera pas dans timerAnalyse)
        lblCoup1.Text = coupsEN(lblCoup1.Text)
        If lblChemin2.Text <> "" Then
            lblCoup2.Text = coupsEN(lblCoup2.Text)
        End If
        lblPonder.Text = coupsEN(lblPonder.Text)

        timerProgression.Enabled = True
    End Sub

    Private Sub majConfig()
        If My.Computer.FileSystem.FileExists(My.Computer.Name & ".ini") Then
            My.Computer.FileSystem.DeleteFile(My.Computer.Name & ".ini")
        End If

        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", lblChemin1.Name & " = " & lblChemin1.Text & vbCrLf, True)
        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", lblChemin2.Name & " = " & lblChemin2.Text & vbCrLf, True)
        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", txtPuissance.Name & " = " & txtPuissance.Text & vbCrLf, True)
        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", txtMemoire.Name & " = " & txtMemoire.Text & vbCrLf, True)
        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", cbPriorite.Name & " = " & cbPriorite.Text & vbCrLf, True)
        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", txtContempt.Name & " = " & txtContempt.Text & vbCrLf, True)

        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", txtLimite.Name & " = " & txtLimite.Text & vbCrLf, True)

        My.Computer.FileSystem.WriteAllText(My.Computer.Name & ".ini", cmdOptions1.Name & " = " & cmdOptions1.Tag & vbCrLf, True)

    End Sub

    Private Sub majDate(Optional fin As Boolean = False)
        'location puissance calcul
        If Not lblFichier.Tag Is Nothing Then
            My.Computer.FileSystem.WriteAllText(lblFichier.Tag, Format(Now(), "dd/MM/yyyy HH:mm:ss") & " = " & DateDiff(DateInterval.Second, depart, Now()) & " sec" & vbCrLf, True)

            If fin Then
                If mode = ".pgn" Then
                    My.Computer.FileSystem.WriteAllText(lblFichier.Tag, vbCrLf & tabPositions.Length & " pos." & vbCrLf, True)
                ElseIf mode = ".epd" Then
                    My.Computer.FileSystem.WriteAllText(lblFichier.Tag, vbCrLf & tabEPD.Length & " pos." & vbCrLf, True)
                End If
            End If

            lblFichier.Tag = Nothing
        End If
    End Sub


    Private Sub verrouillage()
        cmdMoteur1.Enabled = False
        cmdOptions1.Enabled = False

        cmdMoteur2.Enabled = False
        cmdOptions2.Enabled = False

        txtPuissance.ReadOnly = True
        txtMemoire.ReadOnly = True
        cbPriorite.Enabled = False
        txtContempt.ReadOnly = True

        cmdListe.Enabled = False
        txtLimite.ReadOnly = True
        txtReprise.ReadOnly = True

        cmdAnalyser.Enabled = False
        cbDiffere.Enabled = False

    End Sub

    Private Sub deverrouillage()
        cmdMoteur1.Enabled = True
        cmdOptions1.Enabled = True

        cmdMoteur2.Enabled = True
        cmdOptions2.Enabled = True

        txtPuissance.ReadOnly = False
        txtMemoire.ReadOnly = False
        cbPriorite.Enabled = True
        txtContempt.ReadOnly = False

        cmdListe.Enabled = True
        txtLimite.ReadOnly = False
        txtReprise.ReadOnly = False

        cmdAnalyser.Enabled = True
        cbDiffere.Enabled = True

    End Sub

    Private Function chargerPGN(fichierPGN As String) As String
        Dim chaine As String, tabChaine() As String, i As Integer, j As Integer, nbCoups As Integer
        Dim fichierTmp As String

        Me.Tag = Me.Text
        verrouillage()
        cmdArreter.Enabled = False

        Me.Text = "PGN loading..."
        chaine = My.Computer.FileSystem.ReadAllText(fichierPGN)
        If InStr(chaine, "]" & vbLf) > 0 _
        Or InStr(chaine, "{") > 0 _
        Or InStr(chaine, "}") > 0 _
        Or InStr(chaine, "[WhiteElo") > 0 _
        Or InStr(chaine, "[BlackElo") > 0 _
        Or InStr(chaine, "[GameDuration") > 0 _
        Or InStr(chaine, " a3 ") > 0 _
        Or InStr(chaine, " a4 ") > 0 _
        Or InStr(chaine, " b3 ") > 0 _
        Or InStr(chaine, " b4 ") > 0 _
        Or InStr(chaine, " c3 ") > 0 _
        Or InStr(chaine, " c4 ") > 0 _
        Or InStr(chaine, " d3 ") > 0 _
        Or InStr(chaine, " d4 ") > 0 _
        Or InStr(chaine, " e3 ") > 0 _
        Or InStr(chaine, " e4 ") > 0 _
        Or InStr(chaine, " f3 ") > 0 _
        Or InStr(chaine, " f4 ") > 0 _
        Or InStr(chaine, " g3 ") > 0 _
        Or InStr(chaine, " g4 ") > 0 _
        Or InStr(chaine, " h3 ") > 0 _
        Or InStr(chaine, " h4 ") > 0 _
        Or InStr(chaine, " Na3 ") > 0 _
        Or InStr(chaine, " Nc3 ") > 0 _
        Or InStr(chaine, " Nf3 ") > 0 _
        Or InStr(chaine, " Nh3 ") > 0 Then

            'pgn hors du dossier de l'application ?
            fichierTmp = ""
            If InStr(fichierPGN, Environment.CurrentDirectory) = 0 Then
                'on copie temporairement le fichier dans le dossier de l'application
                My.Computer.FileSystem.CopyFile(fichierPGN, Environment.CurrentDirectory & "\" & nomFichier(fichierPGN))
                fichierTmp = fichierPGN
                fichierPGN = Environment.CurrentDirectory & "\" & nomFichier(fichierPGN)
            End If

            'on détaille le pgn et on crée le epd
            Shell("detailler_pgn.exe " & fichierPGN, AppWinStyle.NormalFocus, True)

            'doit on rappatrier les résultats ?
            If InStr(fichierTmp, Environment.CurrentDirectory) = 0 Then
                'on supprime le pgn temporaire
                My.Computer.FileSystem.DeleteFile(fichierPGN)
                'on déplace le pgn détaillé
                For Each fichierComplete In My.Computer.FileSystem.GetFiles(Environment.CurrentDirectory, FileIO.SearchOption.SearchTopLevelOnly, Replace(nomFichier(fichierPGN), ".pgn", "_complete.*"))
                    My.Computer.FileSystem.MoveFile(fichierComplete, fichierTmp.Substring(0, fichierTmp.LastIndexOf("\")) & "\" & nomFichier(fichierComplete))
                Next
                fichierPGN = fichierTmp
            End If

            'on met à jour l'application et on charge le pgn détaillé
            fichierPGN = Replace(fichierPGN, ".pgn", "_complete.pgn")
            If My.Computer.FileSystem.FileExists(fichierPGN) Then
                chaine = My.Computer.FileSystem.ReadAllText(fichierPGN)
            Else
                Return ""
            End If
        End If

        tabPGN = Split(chaine, vbCrLf)

        'on retient que les suites de coups (en notation fr juste pour l'affichage)
        chaine = ""
        For i = 0 To UBound(tabPGN)
            If gauche(tabPGN(i), 3) = "1. " Then
                chaine = chaine & coupsFR(tabPGN(i)) & vbCrLf
            End If
            Me.Text = "PGN loading @ " & Format((i + 1) / tabPGN.Length, "0%")
            Application.DoEvents()
        Next
        chaine = gauche(chaine, Len(chaine) - 2)
        tabPGN = Split(chaine, vbCrLf)

        ReDim tabPositions(UBound(tabPGN))

        'on ne retient que les coups de base (position de départ, position d'arrivée)
        For i = 0 To UBound(tabPGN)
            tabChaine = Split(tabPGN(i), " ")

            chaine = ""
            nbCoups = 1
            For j = 0 To UBound(tabChaine)
                If InStr(tabChaine(j), ".") = 0 _
                And tabChaine(j) <> "" _
                And tabChaine(j) <> "*" Then
                    'coups blanc
                    If (nbCoups Mod 2) = 1 Then
                        chaine = chaine & formaterCoups("moteur", tabChaine(j), nbCoups) & " "
                    Else
                        'coups noir
                        chaine = chaine & formaterCoups("moteur", tabChaine(j)) & " "
                    End If
                    nbCoups = nbCoups + 1
                End If
                Application.DoEvents()
            Next

            If chaine <> "" Then
                tabPositions(i) = Trim(chaine)
            End If

            Me.Text = "PGN simplifying @ " & Format((i + 1) / tabPGN.Length, "0%")
            Application.DoEvents()
        Next

        Me.Text = Me.Tag
        deverrouillage()
        cmdArreter.Enabled = True

        Return fichierPGN
    End Function

    Private Sub capturer()
        captureEcran(Me, "analyseur.png")
    End Sub

    Private Sub chargerEPD(fichier As String)
        Dim chaine As String

        Me.Tag = Me.Text
        verrouillage()
        cmdArreter.Enabled = False

        Me.Text = "EPD loading..."
        chaine = My.Computer.FileSystem.ReadAllText(fichier)
        tabEPD = Split(chaine, vbCrLf)

        If tabEPD(UBound(tabEPD)) = "" Then
            ReDim Preserve tabEPD(UBound(tabEPD) - 1)
        End If

        Me.Text = Me.Tag
        deverrouillage()
        cmdArreter.Enabled = True
    End Sub

    Private Sub lblChemin2_DoubleClick(sender As Object, e As EventArgs) Handles lblChemin2.DoubleClick
        If MsgBox("Deactivate the 2nd engine ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            lblChemin2.Text = ""
            puissance = cpu()
        End If
    End Sub
End Class
